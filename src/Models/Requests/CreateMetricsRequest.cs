using System.Collections.Generic;

namespace Konso.Clients.Metrics.Models.Requests
{
    public class CreateMetricsRequest
    {
        
        /// <summary>
        /// name of the method
        /// </summary>
        public string Name { get; set; }

        //
        // duration in milliseconds
        // 
        public long Duration { get; set; }

        
        /// <summary>
        /// response code
        /// </summary>
        public int? ResponseCode { get; set; }

        public long TimeStamp { get; set; }

        public List<string> Tags { get; set; }

        public string AppName { get; set; }

        public string? AppVersion { get; set; }
        public string CorrelationId { get; set; }

        public string? UserAgent { get; set; }

        public string? IP { get; set; }

        public byte? Runtime { get; set; }

        public string? RuntimeVersion { get; set; }

    }
}
