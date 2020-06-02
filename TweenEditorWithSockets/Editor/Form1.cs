using System;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using LiveTween;
using System.Threading.Tasks;
using System.Text;

namespace Editor
{
    public partial class Form1 : Form
    {
        private static string tweenData = string.Empty;
        private static Tween tween = null;

        public Form1()
        {
            InitializeComponent();

            try
            {
                //Process.Start("server.exe");    // doesn't exist yet!
            }
            catch (Exception e)
            {
                MessageBox.Show("Could not launch the server! Make sure server.exe is in the same directory as this program.");
            }

            // Populate the combo box with the enum values
            foreach (var item in Enum.GetValues(typeof(EasingType)))
            {
                easeTypeField.Items.Add(item);
            }
        }

        private void OnShown(object sender, EventArgs e)
        {
            bool didConnect = ClientSocket.ConnectToServer();
            if (didConnect)
            {
                ClientSocket.WaitForGameConnection();
                ShowControls();

                tweenDataWorker.RunWorkerAsync();
            }
        }

        private void ShowControls()
        {
            waitingLabel.Visible = false;
            durationLabel.Visible = true;
            durationField.Visible = true;
            easeTypeLabel.Visible = true;
            easeTypeField.Visible = true;
        }

        private void tdwDoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            byte[] bytes = new byte[1024];
            int bytesRec = ClientSocket.Socket.Receive(bytes);
            tweenData = Encoding.ASCII.GetString(bytes, 0, bytesRec);
        }

        private void tdwRunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (tweenData.Contains("LiveTween"))
            {
                JMessage message = JMessage.Deserialize(tweenData);
                if (message.Type == typeof(Tween))
                {
                    Tween tween = message.Value.ToObject<Tween>();
                    durationField.Text = tween.Duration.ToString();
                    easeTypeField.SelectedItem = tween.EasingType;
                }
                else
                {
                    throw new Exception();
                }
            }
        }

        private void UpdateTween(object sender, EventArgs e)
        {
            if (tween != null)
            {
                string tweenData = JMessage.Serialize(JMessage.FromValue(tween));

                byte[] msg = Encoding.ASCII.GetBytes(tweenData);
                ClientSocket.Socket.Send(msg);
            }
            else
            {
                Console.WriteLine("Tween data is empty. Cannot send.");
            }
        }
    }
}
