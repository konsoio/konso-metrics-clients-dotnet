using System.Collections.Generic;

namespace Konso.Clients.Metrics.Models
{
    public class MetricServiceConfig
    {
        public string BucketId { get; set; }
        public string Endpoint { get; set; }
        public string AppName { get; set; }
        public string ApiKey { get; set; }

        public List<string> IgnorePath { get; set; } = ["healthcheck", "healthz", "ping"];
    }
}
