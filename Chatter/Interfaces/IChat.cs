using System;
using System.CodeDom.Compiler;
using System.ServiceModel;

namespace OpenALPRQueueConsumer.Chatter.Interfaces
{
    // https://www.codeproject.com/Articles/19752/WCF-WPF-Chat-Application

    #region IChat interface
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IChatCallback))]
    public interface IChat
    {
        [OperationContract(AsyncPattern = true, Action = "http://tempuri.org/IChat/Join", ReplyAction = "http://tempuri.org/IChat/JoinResponse")]
        IAsyncResult BeginJoin(User name, AsyncCallback callback, object asyncState);

        User[] EndJoin(IAsyncResult result);

        [OperationContract(IsOneWay = false, IsInitiating = true, IsTerminating = false, Action = "http://tempuri.org/IChat/Join")]
        User[] Join(User name);

        [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = false, Action = "http://tempuri.org/IChat/Leave")]
        void Leave();

        [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = false, Action = "http://tempuri.org/IChat/Say")]
        void Say(Message msg);

        [OperationContract(IsOneWay = true, IsInitiating = false, IsTerminating = false, Action = "http://tempuri.org/IChat/Whisper")]
        void Whisper(string sendTo, Message msg);
    }
    #endregion
}
