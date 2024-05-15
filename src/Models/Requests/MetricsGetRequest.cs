using Konso.Clients.Metrics.Models.Requests.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konso.Clients.Metrics.Models.Requests
{
    public class MetricsGetRequest
    {
        public required string BucketId { get; set; }
        public string? AppName { get; set; }
        public string? Name { get; set; }
        public int? ResponseCode { get; set; }
        public long? DateFrom { get; set; }
        public long? DateTo { get; set; }
        public int From { get; set; } = 0;
        public int To { get; set; } = 10;
        public List<string> Tags { get; set; }
        public SortingTypes Sort { get; set; } = SortingTypes.TimeStampDesc;

        public string? CorrelationId { get; set; }
    }
}
