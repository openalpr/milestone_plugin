using System;
using System.Net;
using System.Text;

namespace OpenALPRQueueConsumer.BeanstalkWorker
{
    internal class HttpAsyncServer
    {
        private readonly string[] listenedAddresses;
        private bool isWorked;
        private HttpListener listener;

        public HttpAsyncServer(string[] listenedAddresses)
        {
            this.listenedAddresses = listenedAddresses;
            isWorked = false;
        }

        private void HandleRequest(HttpListenerContext context)
        {
        }

        private void Work()
        {
            listener = new HttpListener();
            foreach (string prefix in listenedAddresses)
                listener.Prefixes.Add(prefix);

            listener.Start();

            while (isWorked)
            {
                try
                {
                    HttpListenerContext context = listener.GetContext();
                    string responseString = "<HTML><BODY> Hello world!</BODY></HTML>";
                    byte[] buffer = Encoding.UTF8.GetBytes(responseString);

                    context.Response.ContentLength64 = buffer.Length;
                    System.IO.Stream output = context.Response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);

                    // You must close the output stream.

                    output.Close();
                }
                catch (Exception)
                {
                }
            }
            Stop();
        }

        public void Stop()
        {
            isWorked = false;
            listener.Stop();
        }
    }
}