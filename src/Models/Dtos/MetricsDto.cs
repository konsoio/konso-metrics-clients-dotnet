using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konso.Clients.Metrics.Models.Dtos
{
    public class MetricsDto
    {
        public string Id
        {
            get;
            set;
        }

        public long TimeStamp
        {
            get;
            set;
        }

        public string AppName
        {
            get;
            set;
        }

        public List<string> Tags
        {
            get;
            set;
        }

        public string CorrelationId
        {
            get;
            set;
        }
        public string BucketId
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public long Duration
        {
            get;
            set;
        }

        public int? ResponseCode
        {
            get;
            set;
        }
    }
}
