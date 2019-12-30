using System;
using System.Collections.Generic;
using System.ServiceProcess;

namespace SystemTrayApp
{
    public class ServiceManager : IServiceManager
    {
        public event EventHandler OnStatusChange;
        public string ServiceName { get; private set; }
        public ServiceControllerStatus Status { get; private set; }

        private const string OpenALPRMilestoneService = "OpenALPRMilestone";

        public ServiceManager()
        {
            ServiceName = "OpenALPRMilestone";            
        }

        #region IServiceManager

        public List<KeyValuePair<string, string>> StatusFlags
        {
            get
            {
                var statusFlags = new List<KeyValuePair<string, string>>();
                statusFlags.Add(new KeyValuePair<string, string>("Milestone XProtect Recording Server ", WindowsService.GetServiceStatus("Milestone XProtect Recording Server").ToString()));
                statusFlags.Add(new KeyValuePair<string, string>("Milestone XProtect Management Server ", WindowsService.GetServiceStatus("Milestone XProtect Management Server").ToString()));
                statusFlags.Add(new KeyValuePair<string, string>("Milestone Event Server Service ", WindowsService.GetServiceStatus("MilestoneEventServerService").ToString()));
                statusFlags.Add(new KeyValuePair<string, string>("Milestone XProtect Data Collector Server ", WindowsService.GetServiceStatus("Milestone XProtect Data Collector Server").ToString()));
                statusFlags.Add(new KeyValuePair<string, string>("Milestone Mobile Service ", WindowsService.GetServiceStatus("Milestone Mobile Service").ToString()));
          
                return statusFlags;
            }
        }

        public void Initialize()
        {
            Status = WindowsService.GetServiceStatus(OpenALPRMilestoneService);
            OnStatusChange?.Invoke(this, new EventArgs());
        }

        public void Start()
        {
            if (Status == ServiceControllerStatus.Stopped)
            {
                WindowsService.StartServiceAsync(OpenALPRMilestoneService);
                Status = WindowsService.GetServiceStatus (OpenALPRMilestoneService);
                OnStatusChange?.Invoke(this, new EventArgs());
            }
        }

        public void Stop()
        {
            if (Status == ServiceControllerStatus.Running)
            {
                WindowsService.StopServiceAsync(OpenALPRMilestoneService);
                Status = WindowsService.GetServiceStatus(OpenALPRMilestoneService);

                OnStatusChange?.Invoke(this, new EventArgs());
            }
        }

        public void SetStatus(ServiceControllerStatus serviceControllerStatus)
        {
            Status = serviceControllerStatus;
        }

        #endregion
    }
}
