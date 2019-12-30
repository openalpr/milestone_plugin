using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using OpenALPRQueueConsumer.Chatter;
using OpenALPRQueueConsumer.Chatter.Interfaces;
using OpenALPRQueueConsumer.Chatter.Proxy;

namespace OpenALPRQueueConsumer.Chat
{
    #region ChatService

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class ChatService : IChat
    {
        #region Instance fields

        public static event EventHandler<ChatEventArgs> ChatEvent;

        private static Dictionary<User, EventHandler<ChatEventArgs>> chatters = new Dictionary<User, EventHandler<ChatEventArgs>>();
        private static readonly object syncObj = new object();

        private IChatCallback callback ;
        private EventHandler<ChatEventArgs> eventHandler;
        private User user;

        #endregion

        #region Helpers
        private static bool CheckIfPersonExists(string name)
        {
            foreach (User p in chatters.Keys)
            {
                if (p.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        private static EventHandler<ChatEventArgs> GetPersonHandler(string name)
        {
            foreach (User p in chatters.Keys)
            {
                if (p.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    chatters.TryGetValue(p, out EventHandler<ChatEventArgs> chatTo);
                    return chatTo;
                }
            }

            return null;
        }

        #endregion

        #region IChat implementation

        public User[] Join(User name)
        {
            bool userAdded = false;
            //create a new ChatEventHandler delegate, pointing to the MyEventHandler() method
            eventHandler = new EventHandler<ChatEventArgs>(ChatEventHandler);

            //carry out a critical section that checks to see if the new chatter
            //name is already in use, if its not allow the new chatter to be
            //added to the list of chatters, using the user as the key, and the
            //ChatEventHandler delegate as the value, for later invocation
            lock (syncObj)
            {
                if (name != null && !CheckIfPersonExists(name.Name))
                {
                    user = name;
                    chatters.Add(name, ChatEventHandler);
                    userAdded = true;
                }
            }

            //if the new chatter could be successfully added, get a callback instance
            //create a new message, and broadcast it to all other chatters, and then 
            //return the list of al chatters such that connected clients may show a
            //list of all the chatters
            if (userAdded)
            {
                callback = OperationContext.Current.GetCallbackChannel<IChatCallback>();
                var e = new ChatEventArgs()
                {
                    CallbackType = CallBackType.UserEnter,
                    User = user
                };

                BroadcastMessage(e);
                //add this newly joined chatters ChatEventHandler delegate, to the global
                //multicast delegate for invocation
                ChatEvent += eventHandler;
                User[] list = new User[chatters.Count];
                //carry out a critical section that copy all chatters to a new list
                lock (syncObj)
                    chatters.Keys.CopyTo(list, 0);
                
                return list;
            }
            else
                return null;
        }

        public void Say(Message msg)
        {
            var e = new ChatEventArgs
            {
                CallbackType = CallBackType.Receive,
                User = user,
                Message = msg
            };

            BroadcastMessage(e);
        }

        public void Whisper(string to, Message msg)
        {
            var e = new ChatEventArgs
            {
                CallbackType = CallBackType.ReceiveWhisper,
                User = user,
                Message = msg
            };

            try
            {
                EventHandler<ChatEventArgs> chatterTo;
                //carry out a critical section, that attempts to find the 
                //correct User in the list of chatters.
                //if a user match is found, the matched chatters 
                //ChatEventHandler delegate is invoked asynchronously
                lock (syncObj)
                {
                    chatterTo = GetPersonHandler(to);
                    if (chatterTo == null)
                        return;
                }
                //do a async invoke on the chatter (call the MyEventHandler() method, and the
                //EndAsync() method at the end of the asynch call
                chatterTo.BeginInvoke(this, e, new AsyncCallback(EndAsync), null);
            }
            catch //(Exception ex)
            {
                //Program.Log.Error("Whisper", ex);
            }
        }

        public void Leave()
        {
            if (user == null)
                return;

            //get the chatters ChatEventHandler delegate
            EventHandler<ChatEventArgs> chatterToRemove = GetPersonHandler(user.Name);

            //carry out a critical section, that removes the chatter from the
            //internal list of chatters
            lock (syncObj)
                chatters.Remove(user);

            //unwire the chatters delegate from the multicast delegate, so that 
            //it no longer gets invokes by globally broadcasted methods

            if(chatterToRemove != null)
                ChatEvent -= chatterToRemove;

            var e = new ChatEventArgs
            {
                CallbackType = CallBackType.UserLeave,
                User = user
            };

            user = null;
            //broadcast this leave message to all other remaining connected
            //chatters
            BroadcastMessage(e);
        }

        #endregion

        #region private methods

        private void ChatEventHandler(object sender, ChatEventArgs e)
        {
            try
            {
                switch (e.CallbackType)
                {
                    case CallBackType.Receive:
                        callback.Receive(e.User, e.Message);
                        break;

                    case CallBackType.ReceiveWhisper:
                        callback.ReceiveWhisper(e.User, e.Message);
                        break;

                    case CallBackType.UserEnter:
                        callback.UserEnter(e.User);
                        break;

                    case CallBackType.UserLeave:
                        callback.UserLeave(e.User);
                        break;
                }
            }
            catch //(Exception ex)
            {
                //Program.Log.Error($"{nameof(ChatEventHandler)}: {e.CallbackType.ToString()} : {e.User.Name}", ex);
                if (user != null && user.Name != User.AutoExporterServiceName)
                    Leave();
            }
        }

        private void BroadcastMessage(ChatEventArgs e)
        {
            EventHandler<ChatEventArgs> temp = ChatEvent;

            //loop through all connected chatters and invoke their 
            //ChatEventHandler delegate asynchronously, which will firstly call
            //the MyEventHandler() method and will allow a asynch callback to call
            //the EndAsync() method on completion of the initial call
            if (temp != null)
            {
                foreach (EventHandler<ChatEventArgs> handler in temp.GetInvocationList())
                {
                    if(handler != null)
                        handler.BeginInvoke(this, e, new AsyncCallback(EndAsync), null);
                }
            }
        }

        private void EndAsync(IAsyncResult ar)
        {
            EventHandler<ChatEventArgs> d = null;

            try
            {
                //get the standard System.Runtime.Remoting.Messaging.AsyncResult,and then
                //cast it to the correct delegate type, and do an end invoke
                var asres = ar as AsyncResult;
                d = asres.AsyncDelegate as EventHandler<ChatEventArgs>;
                d.EndInvoke(ar);
                if(asres.CompletedSynchronously )
                {

                }
            }
            catch //(Exception ex)
            {
                //Program.Log.Error("EndAsync", ex);
                ChatEvent -= d;
            }
        }
        #endregion

        public IAsyncResult BeginJoin(User name, AsyncCallback callback, object asyncState)
        {
            throw new NotImplementedException(nameof(BeginJoin));
        }

        public User[] EndJoin(IAsyncResult result)
        {
            throw new NotImplementedException(nameof(EndJoin));
        }

    }
    #endregion
}

