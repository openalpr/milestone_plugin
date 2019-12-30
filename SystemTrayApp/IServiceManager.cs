using System.Collections.Generic;
using System.ServiceProcess;

namespace SystemTrayApp
{
    /*
     * A simple dummy device with some simple commands to control its state
     */
    public interface IServiceManager
    {
        string ServiceName { get; }
        ServiceControllerStatus Status { get; }
        List<KeyValuePair<string, string>> StatusFlags { get; }
        void Initialize();
        void Start();
        void Stop();
        void SetStatus(ServiceControllerStatus serviceControllerStatus);
        //void Terminate();
    }
}
