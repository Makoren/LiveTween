using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class ServerSocket
{ 
    public static string data = null;
    private static Socket editorSocket;
    private static Socket gameSocket;

    public static void StartListening()
    {  
        byte[] bytes = new Byte[1024];

        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

        Socket listener = new Socket(ipAddress.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);

        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(10);
 
            while (true)
            {
                Console.WriteLine("Waiting for connection with editor...");
                editorSocket = listener.Accept();
                data = null;

                int bytesRec = editorSocket.Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                if (data == "editor")
                {
                    Console.WriteLine("Editor connected.");
                    break;
                }
                else
                {
                    Console.WriteLine("Editor connection was not received. Disconnecting...");
                    editorSocket.Shutdown(SocketShutdown.Both);
                    editorSocket.Close();
                }
            }

            while (true)
            {
                Console.WriteLine("Waiting for connection with game...");

                gameSocket = listener.Accept();
                data = null;

                int bytesRec = gameSocket.Receive(bytes);
                data += Encoding.ASCII.GetString(bytes, 0, bytesRec);

                if (data == "game")
                {
                    Console.WriteLine("Game connected.");

                    byte[] msg = Encoding.ASCII.GetBytes("connect");
                    editorSocket.Send(msg);

                    break;
                }
                else
                {
                    Console.WriteLine("Game connection was not received. Disconnecting...");
                    editorSocket.Shutdown(SocketShutdown.Both);
                    editorSocket.Close();
                }
            }

            while (true)
            {
                Console.WriteLine("Waiting for tween data...");
                int bytesRec = gameSocket.Receive(bytes);
                data = Encoding.ASCII.GetString(bytes, 0, bytesRec);

                // Crappy data validation that works but doesn't require JSON parsing
                if (data.Contains("LiveTween")) break;
            }

            Console.WriteLine("Received valid data. Sending to editor...");
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            editorSocket.Send(buffer);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("\nPress ENTER to continue...");
        Console.Read();

    }

    public static int Main(String[] args)
    {
        StartListening();
        return 0;
    }
}