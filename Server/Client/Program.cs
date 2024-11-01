using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using Network;
using Client;

namespace Client
{
    internal static class Program
    {
        public static ServerForm form;

        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            form = new ServerForm();

            Thread.Sleep(1000);

            Connector connector = new Connector();
            connector.Connect(() => { return new ServerSession(); });

            Application.Run(form);
        }
    }
}
