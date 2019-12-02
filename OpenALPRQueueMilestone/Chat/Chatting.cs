
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using OpenALPRQueueConsumer.Chatter;
using OpenALPRQueueConsumer.Chatter.Proxy;

namespace OpenALPRQueueConsumer.Chat
{
    internal static class Chatting
    {
        internal static event EventHandler<ChatEventArgs> UserEnter;
        internal static event EventHandler<ChatEventArgs> UserLeave;
        internal static event EventHandler<ChatEventArgs> InfoArrived;

        internal static bool ServiceClosing { set; get; }
        private static ProxySingleton proxySingleton = ProxySingleton.Instance;
        private static List<User> users;
        private static Queue clientMessages;
        private static readonly object sayLocker = new object();

        public static void Initialize(User user)
        {
            users = new List<User>();
            clientMessages = Queue.Synchronized(new Queue());
            
            ChatServer.OpenHost();
            if (ChatServer.IsOpened)
            {
                proxySingleton.Connect(user);

                int limit = 0;
                while (!proxySingleton.IsConnected && limit++ < 30 && proxySingleton.ProxyState != CommunicationState.Opened)
                {
                    Thread.Sleep(1000);
                }

                proxySingleton.ProxyEvent += ProxyEvent;
                proxySingleton.ProxyCallBackEvent += ProxyCallBackEvent;
            }
        }

        public static void ProxyCallBackEvent(object sender, ProxyCallBackEventArgs e)
        {
            try
            {
                switch (e.ProxyCallbackType)
                {
                    case CallBackType.Receive:
                        Receive(e.ProxyUser, e.ProxyMessage, e.ProxyCallbackType);
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
            catch (Exception ex)
            {
                Program.Log.Error("ProxyCallBackEvent", ex);
            }
        }

        private static void Receive(User user, Message message, CallBackType callbackType)
        {
            lock (clientMessages.SyncRoot)
                clientMessages.Enqueue(new ChatEventArgs(user, message, callbackType));
        }

        private static void ReceiveWhisper(User user, Message message)
        {
            lock (clientMessages.SyncRoot)
                clientMessages.Enqueue(new ChatEventArgs(user, message, CallBackType.ReceiveWhisper));
        }

        internal static void Whisper(string name, Info info)
        {
            lock (sayLocker)
            {
                if (name == null)
                    return;

                if (users.Count != 0)
                {
                    var user = users.SingleOrDefault(u => u.Name.ToUpperInvariant() == name.ToUpperInvariant());
                    if (user != null)
                        SayMessage(user, new Message(info), true);
                }
            }
        }

        internal static void WhisperGui(Info info, string except = null)
        {
            lock (sayLocker)
            {
                if (users.Count != 0)
                {
                    var selected = users.Where(u => u.Name.ToUpperInvariant().EndsWith (User.ManagmentClientPluginName.ToUpperInvariant()));
                    foreach (var u in selected)
                    {
                        if (u.Name != except)
                            SayMessage(u, new Message(info), true);
                    }
                }
            }
        }

        internal static void Say(Info info)
        {
            lock (sayLocker)
            {
                for (int i = 0; i < users.Count; i++)
                {
                    if (users[i].Name != User.AutoExporterServiceName)
                        SayMessage(users[i], new Message(info), true);
                }
            }
        }

        private static void SayMessage(User user, Message message, bool pvt)
        {
            if (user == null)
                return;

            try
            {
                proxySingleton.SayAndClear(user.Name, message, pvt);
            }
            catch //(Exception ex)
            {
                //Program.Log.Error("SayMessage", ex);
            }
        }

        private static void ProxyEvent(object sender, ProxyEventArgs e)
        {
            foreach (User user in e.GetList())
            {
                if (user.Name != User.AutoExporterServiceName)
                {
                    users.Add(user);
                    UserEnter?.Invoke(null, new ChatEventArgs(user));
                }
            }
        }

        private static void AddNewUser(User user)
        {
            var reciver = users.SingleOrDefault(u => u.Name.ToUpperInvariant() == user.Name.ToUpperInvariant());
            if (reciver == null)
                users.Add(user);
            
            UserEnter?.Invoke(null, new ChatEventArgs(user));
        }

        private static void RemoveUser(User user)
        {
            var reciver = users.SingleOrDefault(u => u.Name.ToUpperInvariant() == user.Name.ToUpperInvariant());
            if (reciver != null)
                users.Remove(user);

            UserLeave?.Invoke(null, new ChatEventArgs(user));
        }

        internal static void Close()
        {
            proxySingleton.ProxyEvent -= ProxyEvent;
            proxySingleton.ProxyCallBackEvent -= ProxyCallBackEvent;
            proxySingleton.ExitChatSession();
            proxySingleton.Dispose();
            ChatServer.CloseHost();
        }

        internal static void MonitorClientToServerQueue()
        {
            ChatEventArgs commEventArgs = null;
            while (!ServiceClosing)
            {
                if (clientMessages.Count != 0)
                {
                    lock (clientMessages.SyncRoot)
                        commEventArgs = clientMessages.Dequeue() as ChatEventArgs;

                    InfoArrived?.Invoke(null, commEventArgs);
                }
                else
                    Thread.Sleep(150);
            }
        }
    }
}
