using System;

namespace OpenALPRQueueConsumer.Chatter.Proxy
{
    // https://www.codeproject.com/Articles/19752/WCF-WPF-Chat-Application

    public class ProxyCallBackEventArgs : EventArgs
    {
        public CallBackType ProxyCallbackType { set; get; }
        public Message ProxyMessage { set; get; }
        public User ProxyUser { set; get; }

        public ProxyCallBackEventArgs()
        {

        }

    }
}
