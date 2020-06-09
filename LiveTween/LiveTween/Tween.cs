using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

/* TODO:
 * There should be a way to receive data asynchronously from the server, so that the player can continue
 * playing the game while the server is running. I'll need to BeginReceive when the tween is created
 * or something like that.
 */

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
        public bool IsPlaying { get; set; }

        private float start;
        private float destination;
        private float t = 0;
        private float uhhh = 0;

        private string tweenData = string.Empty;

        private BackgroundWorker bgw;

        /// <summary>
        /// Sets up a default constructor with a duration of 1 and easing type of Linear. Does not enable LiveTween.
        /// </summary>
        public Tween()
        {
            EasingType = EasingType.Linear;
            Duration = 1;
        }

        /// <summary>
        /// Create a new tween.
        /// </summary>
        /// <param name="easingType">Which easing algorithm the current tween will use.</param>
        /// <param name="duration">How long the tween will play for in seconds.</param>
        /// <param name="enableLiveTween">Whether or not to start accepting tween data from the LiveTween editor.</param>
        public Tween(EasingType easingType, float duration, bool enableLiveTween)
        {
            EasingType = easingType;
            Duration = duration;

            if (enableLiveTween)
            {
                if(Connect())
                    StartListener();
            }
        }

        #region Listener
        /// <summary>
        /// Waits for tween data to be sent from the editor using another thread.
        /// </summary>
        private void StartListener()
        {
            bgw = new BackgroundWorker();
            bgw.DoWork += bgwDoWork;
            bgw.RunWorkerCompleted += bgwRunWorkerCompleted;

            bgw.RunWorkerAsync();
        }

        private void bgwDoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                byte[] bytes = new byte[1024];
                int bytesRec = Socket.Receive(bytes);
                tweenData = Encoding.ASCII.GetString(bytes, 0, bytesRec);
            }
            catch
            {
                return;
            }
        }

        private void bgwRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (tweenData.Contains("LiveTween"))
            {
                JMessage message = JMessage.Deserialize(tweenData);
                if (message.Type == typeof(Tween))
                {
                    Tween temp = message.Value.ToObject<Tween>();
                    Duration = temp.Duration;
                    EasingType = temp.EasingType;

                    Console.WriteLine("New Duration: " + Duration + ", New Easing Type: " + EasingType.ToString());
                }
                else
                {
                    throw new Exception();
                }
            }

            bgw.RunWorkerAsync();
        }
        #endregion

        private float GetEasingFormula(float t)
        {
            switch (EasingType)
            {
                case EasingType.Linear:
                    return t;
                case EasingType.Quadratic:
                    return t < 0.5 ? 2 * t * t : 1 - (float)Math.Pow(-2 * t + 2, 2) / 2;
                default:
                    return t;
            }
        }

        /// <summary>
        /// Play this tween using its current properties.
        /// </summary>
        public void Play(float fromValue, float toValue)
        {
            if (!IsPlaying)
            {
                IsPlaying = true;
                start = fromValue;
                destination = toValue;
            }
        }

        /// <summary>
        /// Checks if the tween is playing, and if true, updates the properties.
        /// </summary>
        public float Update(float deltaTime)
        {
            if (IsPlaying)
            {
                t += (1 / Duration) * deltaTime;
                uhhh = GetEasingFormula(t);
            }

            if (t >= 1)
            {
                IsPlaying = false;
                t = 0;
                start = destination;
            }

            return start * (1 - uhhh) + destination * uhhh;
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
        /// Copy the properties from another tween.
        /// </summary>
        /// <param name="tween">The tween to copy properties from.</param>
        public void CopyFrom(Tween tween)
        {
            Duration = tween.Duration;
            EasingType = tween.EasingType;
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
