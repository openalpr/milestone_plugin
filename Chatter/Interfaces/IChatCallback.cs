
using System.CodeDom.Compiler;
using System.ServiceModel;

namespace OpenALPRQueueConsumer.Chatter.Interfaces
{
    // https://www.codeproject.com/Articles/19752/WCF-WPF-Chat-Application

    #region IChatCallback interface
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    public interface IChatCallback
    {
        [OperationContract(IsOneWay = true, Action = "http://tempuri.org/IChat/Receive")]
        void Receive(User sender, Message message);

        [OperationContract(IsOneWay = true, Action = "http://tempuri.org/IChat/ReceiveWhisper")]
        void ReceiveWhisper(User sender, Message message);

        [OperationContract(IsOneWay = true, Action = "http://tempuri.org/IChat/UserEnter")]
        void UserEnter(User user);

        [OperationContract(IsOneWay = true, Action = "http://tempuri.org/IChat/UserLeave")]
        void UserLeave(User user);
    }
    #endregion
}