// Copyright OpenALPR Technology, Inc. 2018

//using OpenALPRQueueConsumer.BeanstalkWorker;
using OpenALPRQueueConsumer.Utility;
using System;
//using System.Collections.Generic;

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
        //public IList<Coordinate> Coordinates = new List<Coordinate>();
        //public double Travel_direction;

        public override string ToString()
        {
            return JsonHelper.FromClass(this);
        }
    }
}
