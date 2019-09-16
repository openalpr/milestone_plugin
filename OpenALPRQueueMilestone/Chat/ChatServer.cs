using System;
using System.ServiceModel;
using OpenALPRQueueConsumer.Chatter.Proxy;

namespace OpenALPRQueueConsumer.Chat
{
    internal static class ChatServer
    {
        private static ServiceHost host;

        public static void OpenHost()
        {
            try
            {
                host = new ServiceHost(typeof(ChatService), new Uri(ProxySingleton.Uri));
                host.Open();
            }
            catch (Exception ex)
            {
                Program.Log.Error("OpenHost", ex);
            }
        }

        public static bool IsOpened
        {
            get { return host == null ? false : host.State == CommunicationState.Opened; }
        }

        public static void CloseHost()
        {
            if (host != null)
            {
                host.Abort();
                host.Close();
            }
        }

    }
}
