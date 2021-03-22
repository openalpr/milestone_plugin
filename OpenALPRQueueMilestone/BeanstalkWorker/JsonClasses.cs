// Copyright OpenALPR Technology, Inc. 2018

using System.Collections.Generic;

namespace OpenALPRQueueConsumer.BeanstalkWorker
{
    public class Candidate
    {
        public string Plate { get; set; }
        public double Confidence { get; set; }
        public int Matches_template { get; set; }
    }

    public class Coordinate
    {
        public long X { get; set; }
        public long Y { get; set; }
    }

    public class VehicleRegion
    {
        public long X { get; set; }
        public long Y { get; set; }
        public long Width { get; set; }
        public long Height { get; set; }
    }

    public class BestPlate
    {
        public string Plate { get; set; }
        public double Confidence { get; set; }
        public long Matches_template { get; set; }
        public long Plate_index { get; set; }
        public string Region { get; set; }
        public long Region_confidence { get; set; }
        public double Processing_time_ms { get; set; }
        public long Requested_topn { get; set; }
        public IList<Coordinate> Coordinates { get; set; }
        public VehicleRegion Vehicle_region { get; set; }
        public IList<Candidate> Candidates { get; set; }
    }

    public class Color
    {
        public string Name { get; set; }
        public double Confidence { get; set; }
    }

    public class Make
    {
        public string Name { get; set; }
        public double Confidence { get; set; }
    }

    public class MakeModel
    {
        public string Name { get; set; }
        public double Confidence { get; set; }
    }

    public class BodyType
    {
        public string Name { get; set; }
        public double Confidence { get; set; }
    }

    public class Vehicle
    {
        public IList<Color> Color { get; set; }
        public IList<Make> Make { get; set; }
        public IList<MakeModel> Make_model { get; set; }
        public IList<BodyType> Body_type { get; set; }
    }

    public class RegionsOfInterest
    {
        public long X { get; set; }
        public long Y { get; set; }
        public long Width { get; set; }
        public long Height { get; set; }
    }


    public class Result
    {
        public string Plate { get; set; }
        public double Confidence { get; set; }
        public long Matches_template { get; set; }
        public long Plate_index { get; set; }
        public string Region { get; set; }
        public long Region_confidence { get; set; }
        public double Processing_time_ms { get; set; }
        public long Requested_topn { get; set; }
        public IList<Coordinate> Coordinates { get; set; }
        public VehicleRegion Vehicle_region { get; set; }
        public IList<Candidate> Candidates { get; set; }
    }

    public class OpenALPRData
    {
        public string Data_type { get; set; }
        public long Version { get; set; }
        public long Epoch_start { get; set; }
        public long Epoch_end { get; set; }
        public long Frame_start { get; set; }
        public long Frame_end { get; set; }
        public string Company_id { get; set; }
        public string Agent_uid { get; set; }
        public string Agent_version { get; set; }
        public string Agent_type { get; set; }
        public long Camera_id { get; set; }
        public string Country { get; set; }
        public IList<string> Uuids { get; set; }
        public IList<int> Plate_indexes { get; set; }
        public IList<Candidate> Candidates { get; set; }
        public BestPlate Best_plate { get; set; }
        public double Best_confidence { get; set; }
        public string Best_uuid { get; set; }
        public string Best_plate_number { get; set; }
        public string Best_region { get; set; }
        public double Best_region_confidence { get; set; }
        public long Best_image_width { get; set; }

        public long Best_image_height { get; set; }
        public double Travel_direction { get; set; }
        public bool Is_parked { get; set; }
        public Vehicle Vehicle { get; set; }
        public IList<Coordinate> Coordinates { get; set; }// = new List<Coordinate>();
    }

    public class VideoStream
    {
        public long Camera_id { get; set; }
        public string Url { get; set; }
        public string Camera_name { get; set; }
        public object Last_update { get; set; }
        public long Fps { get; set; }
        public bool Is_streaming { get; set; }
    }

    public class Heartbeats
    {
        public IList<VideoStream> Video_streams { get; set; }
        public long Disk_quota_total_bytes { get; set; }
        public long Disk_quota_consumed_bytes { get; set; }
        public string Agent_uid { get; set; }
        public long Cpu_last_update { get; set; }
        public long Processing_threads_active { get; set; }
        public long Memory_consumed_bytes { get; set; }
        public string Company_id { get; set; }
        public long Cpu_cores { get; set; }
        public string Agent_type { get; set; }
        public long Cpu_usage_percent { get; set; }
        public string Data_type { get; set; }
        public long Memory_swaptotal_bytes { get; set; }
        public long Timestamp { get; set; }
        public long Memory_last_update { get; set; }
        public long Daemon_uptime_seconds { get; set; }
        public long Memory_total_bytes { get; set; }
        private long m_Disk_quota_earliest_result;
        public long Disk_quota_earliest_result //{ get; set; }
        {
            get 
            { 
                return m_Disk_quota_earliest_result; 
            }
            set 
            {
                if (value == -1) { value = 0; }
                m_Disk_quota_earliest_result = value;
            }
        }
        
        public string OS { get; set; }
        public long Processing_threads_configured { get; set; }
        public long System_uptime_seconds { get; set; }
        public long Beanstalk_queue_size { get; set; }
        public string Agent_version { get; set; }
        public string Openalpr_version { get; set; }
        public long Memory_swapused_bytes { get; set; }
    }

}