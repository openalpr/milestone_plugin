using OpenALPRQueueConsumer.Utility;
using System;

namespace OpenALPRQueueConsumer.Milestone
{
    internal class PlateInfo
    {
        public string DataType;
        public long CameraId;
        public string BestPlateNumber;
        public string BestRegion;
        public DateTime EpochStart;
        public DateTime EpochEnd;
        public string Make;
        public string BodyType;
        public string MakeModel;
        public string Color;
        public string CandidatesPlate;

        public override string ToString()
        {
            return JsonHelper.FromClass(this);
        }
    }
}
