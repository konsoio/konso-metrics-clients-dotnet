using Konso.Clients.Metrics.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace Konso.Clients.Metrics.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseKonsoMetrics(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MetricsMiddleware>();
        }
    }
}
