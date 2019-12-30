﻿// Copyright OpenALPR Technology, Inc. 2018

using System;
using System.Security.Permissions;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace OpenALPRQueueConsumer
{
    public partial class OpenALPRQueueMilestone : ServiceBase
    {
        internal static ServiceStarter ServiceStartInstance;
        private static OpenALPRQueueMilestone serviceInstance;
        
        public OpenALPRQueueMilestone()
        {
            InitializeComponent();
            serviceInstance = this;
        }

        public static OpenALPRQueueMilestone ServiceInstance
        {
            get { return serviceInstance; }
        }

        internal void StartConsole(string[] args)
        {
            OnStart(args);
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        protected override void OnStart(string[] args)
        {
            ServiceStartInstance = new ServiceStarter();
            Task.Run(() => ServiceStartInstance.OnStartServiceAsync());
        }

        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            return base.OnPowerEvent(powerStatus);
        }

        protected override void OnShutdown()
        {
            Program.Log.Warn("OnShutdown");
            OnStop();
        }

        internal void StopConsole()
        {
            OnStop();
        }

        protected override void OnStop()
        {
            if (ServiceStartInstance != null)
            {
                try
                {
                    ServiceStartInstance.OnStop();
                }
                catch (Exception ex)
                {
                    Program.Log.Error(null, ex);
                }
            }

            Program.Log.Logger.Repository.Shutdown();
        }
    }
}
