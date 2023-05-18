using Konso.Clients.Metrics.Models;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Konso.Clients.Metrics.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureKonsoMetrics(this IServiceCollection services, Action<MetricServiceConfig> configureOptions)
        {
            services.AddOptions();

            // use httpclient factory
            services.AddHttpClient();
            services.AddSingleton<IMetricsServiceClient, MetricsServiceClient>();

            // setup configuration
            services.Configure<MetricServiceConfig>(configureOptions);
        }
    }
}
