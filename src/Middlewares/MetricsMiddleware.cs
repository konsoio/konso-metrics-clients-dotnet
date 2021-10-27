using Konso.Clients.Metrics.Models;
using Konso.Clients.Metrics.Models.Requests;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Konso.Clients.Metrics.Middlewares
{
    public class MetricsMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMetricsService _metricsService;

        public MetricsMiddleware(RequestDelegate next, 
            IMetricsService metricsService)
        {
            _next = next;
            _metricsService = metricsService;

        }

        public async Task Invoke(HttpContext context)
        {
            // start timer
            var stopper = Stopwatch.StartNew();

            // invoke
            await _next(context);

            // stop
            stopper.Stop();

            // no need to measure OPTIONS calls
            if (context.Request.Method != "OPTIONS")
            {
                await _metricsService.CreateAsync(new CreateMetricsRequest()
                {
                    Duration = stopper.ElapsedMilliseconds,
                    Name = $"{context.Request.Method} {context.Request.Path.ToString()}",
                    ResponseCode = context.Response != null ? context.Response.StatusCode : new int?()
                });
            }
                
        }
    }
}
