using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;

namespace BlueChatClientWindows.Classes
{
    class ClientServices
    {
        //Change this to whatever server you are trying to connect to
        private string ServerIP = "127.0.0.1";
        private string Port = "10000";
        private StreamSocket streamSocket;
        private Stream outputStream;
        private StreamWriter streamWriter;
        private Stream inputStream;
        private StreamReader streamReader;

        public Boolean Connected { get; set; }
        public string Username { get; set; }

        public ClientServices()
        {
            Username = "Default";
            streamSocket = new StreamSocket();
            outputStream = streamSocket.OutputStream.AsStreamForWrite();
            streamWriter = new StreamWriter(outputStream);
            inputStream = streamSocket.InputStream.AsStreamForRead();
            streamReader = new StreamReader(inputStream);
            Connected = false;
        }

        public async void StartClient()
        {
            try {

                var hostName = new Windows.Networking.HostName(ServerIP);

                Debug.WriteLine("client is trying to connect...");

                await streamSocket.ConnectAsync(hostName, Port);

                Debug.WriteLine("client connected");

                Connected = true;
            }
            catch (Exception ex)
            {
                Windows.Networking.Sockets.SocketErrorStatus webErrorStatus = Windows.Networking.Sockets.SocketError.GetStatus(ex.GetBaseException().HResult);
                Debug.WriteLine(webErrorStatus.ToString() != "Unknown" ? webErrorStatus.ToString() : ex.Message);
            }
        }

        public async Task<String> SendMessage(string request)
        {
            try
            {
                await streamWriter.WriteLineAsync(request);
                await streamWriter.FlushAsync();
    
                Debug.WriteLine(string.Format("client sent the request: \"{0}\"", request));

                string response;

                response = await streamReader.ReadLineAsync();

                Debug.WriteLine(string.Format("client received the response: \"{0}\" ", response));

                return response;
            }
            catch(Exception e)
            {
                Debug.WriteLine("Failed to send message: " + e.Message);
            }

            return String.Empty;
        }

        public void CloseClient()
        {
            streamSocket.Dispose();
            Connected = false;
            Debug.WriteLine("Client closed its connection.");
        }
    }


}
