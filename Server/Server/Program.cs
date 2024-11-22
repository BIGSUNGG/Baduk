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
using ServerDB;
using System.Data.SqlClient;

namespace Server
{
    internal static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //CreateTempAccounts();

            Application.Run(new ServerForm());
        }

        static void CreateTempAccounts()
        {
            Random random = new Random();
            for (int i = 0; i < 1000; i++)
            {
                AccountGateWay account = new AccountGateWay();
                if (account.Select($"Temp{i}", "1111"))
                {
                    // 이미 있는 테스트 계정이 있다면 점수만 변경
                    account.Score = random.Next(500, 2001);
                    account.Update();
                }
                else
                {
                    // 테스트 계정이 없다면 계정 생성
                    account.Name = $"Temp{i}";
                    account.Password = "1111";
                    account.Score = random.Next(500, 2001);
                    account.Insert();
                }
            }
        }
    }
}
