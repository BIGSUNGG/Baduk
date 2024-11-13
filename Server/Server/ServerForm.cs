using Network;
using ServerDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Server
{
    public partial class ServerForm : Form
    {
        private Timer timer;

        public ServerForm()
        {
            InitializeComponent();

            Listener listener = new Listener();
            listener.Listen(() => { return ClientSessionManager.Instance.Create(); });

            timer = new Timer();
            timer.Interval = 10; 
            timer.Tick += Timer_Event; 
            timer.Start();

            textBox2.Multiline = true;
            textBox2.ReadOnly = true;
        }

        private void Timer_Event(object sender, EventArgs e)
        {
            var q = LogManager.Instance.PopMessage();
            while(q.Count > 0)
            {
                textBox2.AppendText(q.Dequeue() + Environment.NewLine);
            }
        }

        private void SendMessage()
        {
            if (textBox1.Text == "")
                return;

            LogManager.Instance.PushMessage($"Server Send Message : {textBox1.Text}");

            S_ChatPacket s_Chat = new S_ChatPacket();
            s_Chat.Sender = "Server";
            s_Chat.Message = textBox1.Text;
            ClientSessionManager.Instance.SendAll(s_Chat);
            textBox1.Text = "";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void Send_Click(object sender, EventArgs e)
        {
            SendMessage();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                SendMessage();
            }
        }
    }
}
