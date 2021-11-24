using Konso.Clients.Metrics.Middlewares;
using Konso.Clients.Metrics.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Konso.Clients.Metrics.Extensions
{
    public static class MetricsMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestPerformanceMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MetricsMiddleware>();
        }

        public static void ConfigureKonsoMetricsMiddleware(this IServiceCollection services)
        {
            services.AddSingleton<IMetricsServiceClient, MetricsServiceClient>();
        }
    }
}
