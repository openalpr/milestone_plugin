using System;

namespace OpenALPRQueueConsumer.Chatter.Proxy
{
    public class ChatEventArgs : EventArgs
    {
        public CallBackType CallbackType;
        public User User;
        public Message Message;

        public ChatEventArgs()
        {

        }

        public ChatEventArgs(User user)
        {
            User = user;
        }

        public ChatEventArgs(User user, Message message, CallBackType callbackType)
            : this(user)
        {
            Message = message;
            CallbackType = callbackType;
        }
    }

    public class ProxyEventArgs : EventArgs
    {
        internal User[] list;
        public User[] GetList()
        {
            return list.Clone() as User[];
        }
    }
}
