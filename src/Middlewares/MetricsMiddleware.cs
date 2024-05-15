using Konso.Clients.Metrics.Models;
using Konso.Clients.Metrics.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Konso.Clients.Metrics.Middlewares
{
    public class MetricsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMetricsServiceClient _metricsService;
        private const string HeaderName = "X-Correlation-ID";
        private static readonly string[] IgnoreMethods = { "OPTIONS" };
        private readonly IHttpContextAccessor _contextAccessor;
        public MetricsMiddleware(RequestDelegate next,
            IMetricsServiceClient metricsService, IHttpContextAccessor contextAccessor)
        {
            _next = next;
            _metricsService = metricsService;
            _contextAccessor = contextAccessor;
        }

        public async Task Invoke(HttpContext context)
        {
            // start timer
            var stopper = Stopwatch.StartNew();

            // get correlation id from header 
            if (context.Request.Headers.TryGetValue(HeaderName, out StringValues correlationId))
            {
                context.TraceIdentifier = correlationId;
            }

            // invoke
            await _next(context);

            // stop
            stopper.Stop();

            // no need to measure ignored calls
            if (!IgnoreMethods.Contains(context.Request.Method))
            {
                await _metricsService.CreateAsync(new CreateMetricsRequest()
                {
                    Duration = stopper.ElapsedMilliseconds,
                    Name = $"{context.Request.Method} {context.Request.Path.ToString()}",
                    ResponseCode = context.Response != null ? context.Response.StatusCode : new int?(),
                    CorrelationId = context.TraceIdentifier,
                    UserAgent = context.Request.Headers[HeaderNames.UserAgent]
                });
            }
                
        }
    }
}
