using OpenALPRQueueConsumer.Chatter;
using OpenALPRQueueConsumer.Chatter.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;

namespace SystemTrayApp.Chat
{
    internal class Chatting
    {
        internal event EventHandler<ChatEventArgs> UserEnter;
        internal event EventHandler<ChatEventArgs> UserLeave;
        internal event EventHandler<ChatEventArgs> InfoReceived;
        internal event EventHandler<ChatEventArgs> InfoWhispreReceived;
        internal bool Initialized;

        private ProxySingleton proxySingleton;
        private IList<User> users;

        public void Initialize()
        {
            Program.Log.Info("Initializing chat");
            proxySingleton = ProxySingleton.Instance;
            users = new List<User>();
            proxySingleton.ProxyEvent += ProxyEvent;
            proxySingleton.ProxyCallBackEvent += ProxyCallBackEvent;
            Initialized = true;
            Program.Log.Info("Initializing chat completed");
        }

        public void Connect(User user)
        {
            //connect to proxy, and subscribe to its events
            proxySingleton.Connect(user);
            int limit = 0;
            while (!proxySingleton.IsConnected && limit++ < 30 && proxySingleton.ProxyState != CommunicationState.Opened)
            {
                Thread.Sleep(1000);
                System.Windows.Forms.Application.DoEvents();
            }
        }

        public bool IsConnected
        {
            get { return proxySingleton != null ? proxySingleton.IsConnected : false; }
        }

        public void ProxyCallBackEvent(object sender, ProxyCallBackEventArgs e)
        {
            switch (e.ProxyCallbackType)
            {
                case CallBackType.Receive:
                    Receive(e.ProxyUser, e.ProxyMessage);
                    break;

                case CallBackType.ReceiveWhisper:
                    ReceiveWhisper(e.ProxyUser, e.ProxyMessage);
                    break;

                case CallBackType.UserEnter:
                    AddNewUser(e.ProxyUser);
                    break;

                case CallBackType.UserLeave:
                    RemoveUser(e.ProxyUser);
                    break;
            }
        }

        private void Receive(User user, Message message)
        {
            InfoReceived?.Invoke(this, new ChatEventArgs { User = user, Message = message });
        }

        private void ReceiveWhisper(User user, Message message)
        {
            InfoWhispreReceived?.Invoke(this, new ChatEventArgs { User = user, Message = message });
        }

        internal void Whisper(string toUser, Message message)
        {
            SayMessage(toUser, message, true);
        }

        internal void Say(Message message)
        {
            SayMessage(null, message, false);
        }

        private void SayMessage(string toUser, Message message, bool pvt)
        {
            if (IsConnected)
            {
                try
                {
                    proxySingleton.SayAndClear(toUser, message, pvt);
                }
                catch (Exception ex)
                {
                    Program.Log.Error("SayMessage", ex);
                }
            }
        }

        private void ProxyEvent(object sender, ProxyEventArgs e)
        {
            foreach (User user in e.GetList())
            {
                users.Add(user);
                UserEnter?.Invoke(this, new ChatEventArgs { User = user });
            }
        }

        private void AddNewUser(User user)
        {
            var reciver = users.SingleOrDefault(u => u.Name.ToUpperInvariant() == user.Name.ToUpperInvariant());
            if (reciver == null)
                users.Add(user);

            UserEnter?.Invoke(this, new ChatEventArgs(user));
        }

        private void RemoveUser(User user)
        {
            var reciver = users.SingleOrDefault(u => u.Name.ToUpperInvariant() == user.Name.ToUpperInvariant());
            if (reciver != null)
                users.Remove(user);

            UserLeave?.Invoke(this, new ChatEventArgs(user));
        }

        internal void ExitChatSession()
        {
            if (proxySingleton != null)
                proxySingleton.ExitChatSession();
        }

        internal void Close()
        {
            if (proxySingleton != null)
            {
                proxySingleton.ProxyEvent -= ProxyEvent;
                proxySingleton.ProxyCallBackEvent -= ProxyCallBackEvent;
                proxySingleton.ExitChatSession();
                proxySingleton.Dispose();
            }
        }
    }
}