using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoOS.Platform;

namespace OpenALPRQueueConsumer.BeanstalkWorker
{
    internal class BookmarkItem
    {
        public FQID FQID { get; set; } = new FQID();
        public DateTime TimeBegin { get; set; }
        public DateTime TimrTrigged { get; set; }
        public DateTime TimeEnd { get; set; }
        public StringBuilder Reference { get; set; } = new StringBuilder();
        public StringBuilder Header { get; set; } = new StringBuilder();
        public StringBuilder Description { get; set; } = new StringBuilder();
        public OpenALPRData PlateInfo { get; set; } = new OpenALPRData();
    }
}
