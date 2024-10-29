using Network;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    public class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(1000);

            Connector connector = new Connector();
            connector.Connect(() => { return new ServerSession(); });

            //string host = Dns.GetHostName();
            //IPHostEntry ipHost = Dns.GetHostEntry(host);
            //IPAddress address = ipHost.AddressList[0];
            //IPEndPoint ipEndPoint = new IPEndPoint(address, 7777); // 최종주소

            //Socket socket = new Socket(ipEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            //Console.WriteLine("H");

            //socket.Connect(ipEndPoint); // 해당 ip주소로 연결 요청

            //Console.WriteLine($"Connect To {socket.RemoteEndPoint.ToString()}");

            //byte[] sendBuff = Encoding.UTF8.GetBytes("Hello Server");
            //socket.Send(sendBuff);

            //byte[] recvBuff = new byte[1024];
            //int recv = socket.Receive(recvBuff);
            //string data = Encoding.UTF8.GetString(recvBuff, 0, recv);

            //Console.WriteLine($"[From Server] {data}");

            //socket.Shutdown(SocketShutdown.Both);
            //socket.Close();

            while (true)
            {
                string message = Console.ReadLine();
                connector._serverSession.Send(message);
            }
        }
    }
}