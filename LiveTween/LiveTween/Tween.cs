using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace LiveTween
{
    /// <summary>
    /// The easing algorithm to be used by the Tween class.
    /// </summary>
    public enum EasingType
    {
        Linear,
        Quadratic
    }

    /// <summary>
    /// A Tween class that can connect to the LiveTween editor during runtime.
    /// </summary>
    public class Tween
    {
        public static Socket Socket { get; private set; }

        public EasingType EasingType { get; set; }
        public float Duration { get; set; }

        private bool isPlaying;

        /// <summary>
        /// Create a new tween.
        /// </summary>
        /// <param name="easingType">Which easing algorithm the current tween will use.</param>
        /// <param name="duration">How long the tween will play for in seconds.</param>
        public Tween(EasingType easingType, float duration)
        {
            EasingType = easingType;
            Duration = duration;
        }

        /// <summary>
        /// Play this tween using its current properties.
        /// </summary>
        public void Play()
        {
            Console.WriteLine("Playing!");
        }

        /// <summary>
        /// Send this tween to the LiveTween editor for editing.
        /// </summary>
        public void Link()
        {
            // Serialise to JSON data and send it through the socket
            string tweenData = JMessage.Serialize(JMessage.FromValue(this));
            Console.WriteLine(tweenData);

            byte[] msg = Encoding.ASCII.GetBytes(tweenData);
            int bytesSent = Socket.Send(msg);
        }

        /// <summary>
        /// Connect the running game to the LiveTween editor.
        /// </summary>
        /// <returns>Whether or not the connection is successful.</returns>
        public static bool Connect()
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

                    // Let the server know that the game is connecting
                    byte[] msg = Encoding.ASCII.GetBytes("game");
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
    }
}
