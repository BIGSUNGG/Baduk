using Network;
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

namespace Client
{
    public partial class ServerForm : Form
    {
        private Timer timer;

        public ServerForm()
        {
            InitializeComponent();

            timer = new Timer();
            timer.Interval = 10; 
            timer.Tick += Timer_Tick; 
            timer.Start();

            textBox2.Multiline = true;
            textBox2.ReadOnly = true;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var q = LogManager.Instance.PopMessage();
            while(q.Count > 0)
            {
                textBox2.AppendText(q.Dequeue() + Environment.NewLine);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true; 
                e.SuppressKeyPress = true; 

                string inputText = textBox1.Text;
                textBox1.Text = "";
                Connector._serverSession.Send(inputText);
            }
        }

        private void Send_Click(object sender, EventArgs e)
        {
            string inputText = textBox1.Text;
            textBox1.Text = "";
            Connector._serverSession.Send(inputText);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
