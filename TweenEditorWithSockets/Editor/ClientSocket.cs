using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Editor
{
    public class ClientSocket
    {
        public static Socket Socket { get; private set; }
        public static string response = string.Empty;
        public static byte[] buffer = new byte[BufferSize];

        public static bool ConnectToServer()
        {
            try
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                Socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    Socket.Connect(remoteEP);

                    // Let the server know that the editor is connecting
                    byte[] msg = Encoding.ASCII.GetBytes("editor");
                    int bytesSent = Socket.Send(msg);

                    return true;
                }
                catch
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public static void WaitForGameConnection()
        {
            byte[] bytes = new Byte[1024];

            while (true)
            {
                int bytesRec = Socket.Receive(bytes);
                string data = Encoding.ASCII.GetString(bytes, 0, bytesRec);

                if (data == "connect") break;
            }
        }

        public static async Task<string> GetTweenData()
        {
            StateObject state = new StateObject();

            while (true)
            {
                Socket.BeginReceive(buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(GetTweenDataCallback), null);
            }
        }

        private static void GetTweenDataCallback(IAsyncResult ar)
        {
            StateObject state = (StateObject)ar.AsyncState;

            int bytesRead = Socket.EndReceive(ar);
            string data = Encoding.ASCII.GetString(buffer, 0, bytesRead);

            if (data.Contains("LiveTween"))
            {
                response = data;
            }
        }
    }

    public class StateObject
    {
        public const int BufferSize = 256;
        
    }
}
