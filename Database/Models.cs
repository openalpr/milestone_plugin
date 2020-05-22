using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class Settings
	{
		public int Id { get; set; }
		public DateTime Created { get; set; } = DateTime.Now;
        public string OpenALPRServerUrl { get; set; }
		public string MilestoneServerName { get; set; }
		public string MilestoneUserName { get; set; }
		public string MilestonePassword { get; set; }
		public int EventExpireAfterDays { get; set; }
		public int EpochStartSecondsBefore { get; set; }
	    public int EpochEndSecondsAfter { get; set; }
	    public bool AddBookmarks { get; set; }
	    public bool AutoMapping { get; set; }
		public int ServicePort { get; set; }
		public string ClientSettingsProviderServiceUri { get; set; }
	}
}
