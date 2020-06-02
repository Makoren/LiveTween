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

                ClientSocket.GetTweenData(this);
            }
        }
        
        public void GetTweenDataCallback(IAsyncResult ar)
        {
            ClientSocket.state = (StateObject)ar.AsyncState;

            int bytesRead = ClientSocket.Socket.EndReceive(ar);
            string data = Encoding.ASCII.GetString(ClientSocket.state.buffer, 0, bytesRead);

            if (data.Contains("LiveTween"))
            {
                JMessage message = JMessage.Deserialize(data);
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

                ClientSocket.GetTweenData(this);
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
    }
}
