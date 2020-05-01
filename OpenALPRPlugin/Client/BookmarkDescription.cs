using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenALPRPlugin.Client
{
    internal class BookmarkDescription
    {
        public string Make { get; set; }
        public string BodyType { get; set; }
        public string Color { get; set; }
        public string BestRegion { get; set; }
        public string Candidates { get; set; }
        public double TravelDirection { get; set; }
        public string PlateNumber { get; set; }
        public string Coordinates { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
