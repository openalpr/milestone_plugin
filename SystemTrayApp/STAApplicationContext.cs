using OpenALPR.SystemTrayApp.Utility;
using OpenALPRQueueConsumer.Chatter;
using OpenALPRQueueConsumer.Chatter.Proxy;
using System;
using System.Net;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemTrayApp.Chat;

namespace SystemTrayApp
{
    public class STAApplicationContext : ApplicationContext
    {
        private ViewManager viewManager;
        private ServiceManager serviceManager;
        private Chatting chatConnection;
        private readonly User CurrentUser;
        private bool closed;

        public STAApplicationContext()
        {
            serviceManager = new ServiceManager();
            viewManager = new ViewManager(serviceManager);

            serviceManager.OnStatusChange += viewManager.OnStatusChange;
            serviceManager.Initialize();

            CurrentUser = new User(User.SystemTrayIconName);
            ProxySingleton.Port = Common.ReadConfigKey("ServicePort");
            ProxySingleton.HostName = Dns.GetHostName();
            CreateChat();
        }

        private void CreateChat()
        {
            chatConnection = new Chatting();
            chatConnection.Initialize();
            chatConnection.InfoReceived += InfoReceived;
            chatConnection.InfoWhispreReceived += InfoWhispreReceived;
            chatConnection.UserEnter += UserEnter;
            chatConnection.UserLeave += UserLeft;

            Task.Run(() => CreateChatConnection());
        }

        #region Chatter Events

        private readonly object chatConnectionLock = new object();
        private void CreateChatConnection()
        {
            lock (chatConnectionLock)
            {
                Program.Log.Info("Enter CreateChatConnection");
                while (!closed && !chatConnection.IsConnected)
                {
#if !DEBUG
                    if (WindowsService.GetServiceStatus("AutoExporterSvc") == ServiceControllerStatus.Running)
#endif
                    chatConnection.Connect(CurrentUser);
                    Thread.Sleep(5000);
                }
                Program.Log.Info("Exit CreateChatConnection");

                if (chatConnection.IsConnected)
                {
                    serviceManager.SetStatus(ServiceControllerStatus.Running);
                    Program.Log.Warn("1");
                    viewManager.OnStatusChange(this, new EventArgs());
                }
            }
        }

        private void UserEnter(object sender, ChatEventArgs e)
        {
            if (e.User.Name == User.AutoExporterServiceName)
            {
                //viewManager.OnStatusChange(this, new EventArgs());
            }
        }

        private void UserLeft(object sender, ChatEventArgs e)
        {
            Program.Log.Info($"UserLeft: {e.User.Name}");

            if (e.User.Name == User.AutoExporterServiceName)
            {
                // service closed
                serviceManager.SetStatus(ServiceControllerStatus.Stopped);
                viewManager.OnStatusChange(this, new EventArgs());
                chatConnection.ExitChatSession();
                CloseConnection();

                Program.Log.Info($"{serviceManager.ServiceName} still installed.");

                CreateChat();
                Task.Run(() => CreateChatConnection());
            }
        }

        private void InfoWhispreReceived(object sender, ChatEventArgs e)
        {
            InfoReceived(e.Message.MessageInfo);
        }

        private void InfoReceived(object sender, ChatEventArgs e)
        {
            InfoReceived(e.Message.MessageInfo);
        }

        private void InfoReceived(Info info)
        {
            switch (info.MsgId)
            {
                case MessageId.Init:
                    break;

                default:
                    //Program.Log.Info($"MsgId: {info.MsgId.ToString ()}");
                    break;
            }
        }

        #endregion Chatter Events

        // Called from the Dispose method of the base class
        protected override void Dispose(bool disposing)
        {
            closed = true;
            if (serviceManager != null && viewManager != null)
                serviceManager.OnStatusChange -= viewManager.OnStatusChange;

            serviceManager = null;
            if (viewManager != null)
            {
                viewManager.Dispose();
                viewManager = null;
            }
        }

        private void CloseConnection()
        {
            if (chatConnection != null)
            {
                chatConnection.InfoReceived -= InfoReceived;
                chatConnection.InfoWhispreReceived -= InfoWhispreReceived;
                chatConnection.UserEnter -= UserEnter;
                chatConnection.UserLeave -= UserLeft;
                chatConnection.Close();
            }
        }
    }
}
