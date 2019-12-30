using System;
using System.Linq;
using System.ServiceProcess;

namespace SystemTrayApp
{
    internal static class WindowsService
    {
        internal static ServiceControllerStatus StartServiceAsync(string serviceName)
        {
            //Program.SystemTrayIconLogger.Log.Info($"StartServiceAsync: {serviceName}");

            if (string.IsNullOrEmpty(serviceName))
                throw new ArgumentNullException(serviceName, $"{nameof(serviceName)} cannot be a null reference");

            var result = ServiceControllerStatus.Stopped;

            var service = ServiceController.GetServices().FirstOrDefault(p => p.ServiceName == serviceName);

            if (service != null)
            {
                if (service.Status == ServiceControllerStatus.Stopped)
                {
                    try
                    {
                        service.Start(new string[0]);
                        service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromMinutes(1));
                        
                    }
                    catch (Exception ex)
                    {
                        Program.Log.Error("StartServiceAsync", ex);
                    }

                    service.Refresh();
                    result = service.Status;
                }

                service.Close();
                service = null;
            }

            return result;
        }

        internal static ServiceControllerStatus StopServiceAsync(string serviceName)
        {
            //Program.SystemTrayIconLogger.Log.Info($"StopServiceAsync: {serviceName}");

            if (string.IsNullOrEmpty(serviceName))
                throw new ArgumentNullException(serviceName, $"{nameof(serviceName)} cannot be a null reference");

            var result = ServiceControllerStatus.Running;

            var service = ServiceController.GetServices().FirstOrDefault(p => p.ServiceName == serviceName);

            if (service != null)
            {
                try
                {
                    if (service.Status == ServiceControllerStatus.Running)
                    {
                        try
                        {
                            service.Stop();
                            service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMinutes (1));
                            service.Refresh();

                            result = service.Status;
                        }
                        catch (Exception ex)
                        {
                            Program.Log.Error("StopServiceAsync", ex);
                        }
                    }
                }
                finally
                {
                    service.Close();
                    service = null;
                }
            }

            return result;
        }

        internal static ServiceControllerStatus GetServiceStatus(string serviceName)
        {
            //Program.SystemTrayIconLogger.Log.Info($"{nameof(IsServiceRunning)}: {serviceName}");

            if (string.IsNullOrEmpty(serviceName))
                throw new ArgumentNullException(nameof(serviceName), $"{nameof(serviceName)} cannot be a null reference");

            var service = ServiceController.GetServices().FirstOrDefault(p => p.ServiceName == serviceName);

            try
            {
                if (service != null)
                {
                    //Program.Log.Info($"Service: {serviceName} status is {service.Status.ToString()}");
                    return service.Status;
                }
            }
            catch (Exception ex)
            {
                Program.Log.Error("GetServiceStatus", ex);
            }
            finally
            {
                if (service != null)
                {
                    service.Dispose();
                    service = null;
                }
            }

            return ServiceControllerStatus.Stopped;
        }

    }
}