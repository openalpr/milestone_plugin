using System.CodeDom.Compiler;
using System.ServiceModel;

namespace OpenALPRQueueConsumer.Chatter.Interfaces
{
    // https://www.codeproject.com/Articles/19752/WCF-WPF-Chat-Application

    #region IChatChannel interface
    [GeneratedCode("System.ServiceModel", "4.0.0.0")]
    public interface IChatChannel : IChat, IClientChannel
    {
    }
    #endregion
}