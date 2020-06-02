using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class ServerSocket
{
    public static string connectData = null;
    public static string editorData = null;
    public static string gameData = null;

    private static Socket editorSocket;
    private static Socket gameSocket;

    public static void StartListening()
    {
        IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11000);

        Socket listener = new Socket(ipAddress.AddressFamily,
            SocketType.Stream, ProtocolType.Tcp);

        try
        {
            listener.Bind(localEndPoint);
            listener.Listen(10);

            byte[] bytes = new Byte[1024];
            ConnectToEditor(bytes, listener);
            ConnectToGame(bytes, listener);
            WaitForTweenDataFromEditor();
            WaitForTweenDataFromGame();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.ReadKey(true);
    }

    private static void WaitForTweenDataFromGame()
    {
        Console.WriteLine("Waiting for tween data from game...");

        StateObject gameState = new StateObject();
        gameSocket.BeginReceive(gameState.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(GameDataCallback), gameState);
    }

    private static void WaitForTweenDataFromEditor()
    {
        Console.WriteLine("Waiting for tween data from editor...");

        StateObject editorState = new StateObject();
        editorSocket.BeginReceive(editorState.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(EditorDataCallback), editorState);
    }

    private static void EditorDataCallback(IAsyncResult ar)
    {
        StateObject editorState = (StateObject)ar.AsyncState;

        int bytesRec = editorSocket.EndReceive(ar);
        editorData = Encoding.ASCII.GetString(editorState.buffer, 0, bytesRec);

        // Crappy data validation that works but doesn't require JSON parsing
        if (editorData.Contains("LiveTween"))
        {
            Console.WriteLine("Received valid data from editor. Sending to game...");
            byte[] buffer = Encoding.ASCII.GetBytes(editorData);
            gameSocket.Send(buffer);
        }

        WaitForTweenDataFromEditor();
    }

    private static void GameDataCallback(IAsyncResult ar)
    {
        StateObject gameState = (StateObject)ar.AsyncState;

        int bytesRec = gameSocket.EndReceive(ar);
        gameData = Encoding.ASCII.GetString(gameState.buffer, 0, bytesRec);

        // Crappy data validation that works but doesn't require JSON parsing
        if (gameData.Contains("LiveTween"))
        {
            Console.WriteLine("Received valid data from game. Sending to editor...");
            byte[] buffer = Encoding.ASCII.GetBytes(gameData);
            editorSocket.Send(buffer);
        }

        WaitForTweenDataFromGame();
    }

    private static void ConnectToGame(byte[] bytes, Socket listener)
    {
        while (true)
        {
            Console.WriteLine("Waiting for connection with game...");

            gameSocket = listener.Accept();
            connectData = null;

            int bytesRec = gameSocket.Receive(bytes);
            connectData += Encoding.ASCII.GetString(bytes, 0, bytesRec);

            if (connectData == "game")
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
    }

    private static void ConnectToEditor(byte[] bytes, Socket listener)
    {
        while (true)
        {
            Console.WriteLine("Waiting for connection with editor...");
            editorSocket = listener.Accept();
            connectData = null;

            int bytesRec = editorSocket.Receive(bytes);
            connectData += Encoding.ASCII.GetString(bytes, 0, bytesRec);

            if (connectData == "editor")
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
    }

    public class StateObject
    {
        public Socket workSocket = null;
        public const int BufferSize = 1024;
        public byte[] buffer = new byte[BufferSize];
        public StringBuilder sb = new StringBuilder();
    }

    public static int Main(String[] args)
    {
        StartListening();
        return 0;
    }
}
