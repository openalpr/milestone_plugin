
using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using System.ServiceModel.Channels;
using OpenALPRQueueConsumer.Chatter.Interfaces;

namespace OpenALPRQueueConsumer.Chatter.Proxy
{
    // https://www.codeproject.com/Articles/19752/WCF-WPF-Chat-Application

    #region ChatProxy class

    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    public partial class ChatProxy : DuplexClientBase<IChat>, IChat
    {
        public ChatProxy(InstanceContext callbackInstance)
            : base(callbackInstance)
        {
        }

        public ChatProxy(InstanceContext callbackInstance, string endpointConfigurationName)
            : base(callbackInstance, endpointConfigurationName)
        {
        }

        public ChatProxy(InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress)
            : base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        public ChatProxy(InstanceContext callbackInstance, string endpointConfigurationName, EndpointAddress remoteAddress)
            : base(callbackInstance, endpointConfigurationName, remoteAddress)
        {
        }

        public ChatProxy(InstanceContext callbackInstance, Binding binding, EndpointAddress remoteAddress)
            : base(callbackInstance, binding, remoteAddress)
        {
        }

        public IAsyncResult BeginJoin(User name, AsyncCallback callback, object asyncState)
        {
            return Channel.BeginJoin(name, callback, asyncState);
        }

        public User[] EndJoin(IAsyncResult result)
        {
            return Channel.EndJoin(result);
        }

        public User[] Join(User name)
        {
            return Channel.Join(name);
        }

        public void Leave()
        {
            Channel.Leave();
        }

        public void Say(Message msg)
        {
            Channel.Say(msg);
        }

        public void Whisper(string sendTo, Message msg)
        {
            Channel.Whisper(sendTo, msg);
        }
    }

    #endregion
}