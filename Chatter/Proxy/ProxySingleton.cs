using OpenALPRQueueConsumer.Chatter.Interfaces;
using System;
using System.ServiceModel;
using System.Xml;

namespace OpenALPRQueueConsumer.Chatter.Proxy
{
    public enum CallBackType { Receive, ReceiveWhisper, UserEnter, UserLeave };

    #region Proxy_Singleton class
    public sealed class ProxySingleton : IChatCallback, IDisposable 
    {
        #region Instance Fields
        public static string Port = "22019";
        public static string HostName = "localhost";

        public event EventHandler<ProxyEventArgs> ProxyEvent;
        public event EventHandler<ProxyCallBackEventArgs> ProxyCallBackEvent;

        public bool IsConnected { private set; get; }
        private static ProxySingleton singleton;
        private static readonly object singletonLock = new object();

        private ChatProxy proxy;

        #endregion

        private ProxySingleton() 
        {
        }

        #region Public Methods

        #region IChatCallback implementation

        public void Receive(User sender, Message message)
        {
            Receive(sender, message, CallBackType.Receive);
        }

        public void ReceiveWhisper(User sender, Message message)
        {
            Receive(sender, message, CallBackType.ReceiveWhisper);
        }

        private void Receive(User sender, Message message, CallBackType callbackType)
        {
            ProxyCallBackEventArgs e = new ProxyCallBackEventArgs
            {
                ProxyMessage = message,
                ProxyCallbackType = callbackType,
                ProxyUser = sender
            };

            OnProxyCallBackEvent(e);
        }

        public void UserEnter(User user)
        {
            UserEnterLeave(user, CallBackType.UserEnter);
        }

        public void UserLeave(User user)
        {
            UserEnterLeave(user, CallBackType.UserLeave);
        }

        private void UserEnterLeave(User user, CallBackType callbackType)
        {
            if (user.Name == User.AutoExporterServiceName)
                IsConnected = false;

            ProxyCallBackEventArgs e = new ProxyCallBackEventArgs
            {
                ProxyUser = user,
                ProxyCallbackType = callbackType
            };

            OnProxyCallBackEvent(e);
        }

        #endregion

        public void Connect(User p)
        {
            OptionalReliableSession OptionalReliableSession = new OptionalReliableSession
            {
                Enabled = true,
                InactivityTimeout = TimeSpan.MaxValue,
                Ordered = true
            };

            NetTcpSecurity netTcpSecurity = new NetTcpSecurity
            {
                Mode = SecurityMode.None
            };

            XmlDictionaryReaderQuotas readerQuotas = new XmlDictionaryReaderQuotas
            {
                MaxDepth = 32 * 2,
                MaxStringContentLength = 8192 * 2,
                MaxArrayLength = 16384 * 2,
                MaxBytesPerRead = 4096 * 2,
                MaxNameTableCharCount = 16384 * 2
            };

            NetTcpBinding netTcpBinding = new NetTcpBinding
            {
                Name = "DuplexBinding",
                ReceiveTimeout = TimeSpan.FromMinutes (10 * 2),
                ReliableSession = OptionalReliableSession,
                Security = netTcpSecurity,
                CloseTimeout = TimeSpan.FromMinutes(2),
                SendTimeout = TimeSpan.FromMinutes(2),
                OpenTimeout = TimeSpan.FromMinutes(2),
                ReaderQuotas = readerQuotas,
                MaxBufferPoolSize = 524288 * 2,
                MaxReceivedMessageSize = 65536 * 2,
                HostNameComparisonMode = HostNameComparisonMode.StrongWildcard,
                ListenBacklog = 10 * 2,
                MaxBufferSize = 65536 * 2,
                MaxConnections = 48 * 2,
                TransactionFlow = false,
                TransferMode = TransferMode.Buffered,
                PortSharingEnabled = false,
                Namespace = "http://tempuri.org/"
            };

            InstanceContext site = new InstanceContext(this);

            proxy = new ChatProxy(site, netTcpBinding, new EndpointAddress(Uri));
            IAsyncResult iar = proxy.BeginJoin(p, new AsyncCallback(OnEndJoin), null);
        }

        public static string Uri
        {
            get { return $"net.tcp://{HostName}:{Port}/chatservice"; }
        }

        private void OnEndJoin(IAsyncResult iar)
        {
            try
            {
                if (proxy != null)
                {
                    IsConnected = proxy != null && proxy.State != CommunicationState.Faulted;
                    if (proxy.State != CommunicationState.Faulted)
                    {
                        User[] list = proxy.EndJoin(iar);
                        HandleEndJoin(list);
                    }
                }
            }
            catch
            {
                IsConnected = false;
            }
        }

        private void HandleEndJoin(User[] list)
        {
            if (list == null)
                ExitChatSession();
            else
            {
                ProxyEventArgs e = new ProxyEventArgs { list = list };
                OnProxyEvent(e);
            }
        }

        public void OnProxyCallBackEvent(ProxyCallBackEventArgs e)
        {
            ProxyCallBackEvent?.Invoke(this, e);
        }

        public void OnProxyEvent(ProxyEventArgs e)
        {
            ProxyEvent?.Invoke(this, e);
        }

        public static ProxySingleton Instance
        {
            get
            {
                //critical section, which ensures the singleton is thread safe
                lock (singletonLock)
                {
                    if (singleton == null)
                        singleton = new ProxySingleton();

                    return singleton;
                }
            }
        }

        public void SayAndClear(string sendTo, Message msg, bool pvt)
        {
            if (proxy != null)
            {
                if (pvt)
                    proxy.Whisper(sendTo, msg);
                else
                    proxy.Say(msg);
            }
        }

        public CommunicationState ProxyState
        {
            get { return proxy == null ? CommunicationState.Closed : proxy.State; }
        }

        public void ExitChatSession()
        {
            if (proxy == null)
                return;

            try
            {
                if (IsConnected && proxy.State != CommunicationState.Faulted && (proxy.State == CommunicationState.Opened || proxy.State == CommunicationState.Opening))
                    proxy.Leave();
                IsConnected = false;
            }
            catch
            {
            }
            finally
            {
                AbortProxy();
            }
        }

        public void AbortProxy()
        {
            if (proxy != null)
            {
                proxy.Abort();
                proxy.Close();
                proxy = null;
            }
        }
        #endregion

        public void Dispose(bool disposing)
        {
            if (disposing)
            {
                AbortProxy();
                // dispose managed resources
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }

    #endregion

}
