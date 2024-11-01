using Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    public class ServerSession : Session
    {
        protected override void OnConnect()
        {
            Console.WriteLine($"Connect");
        }

        protected override void OnDiscconect()
        {
            Console.WriteLine($"Disconnect");
        }

        protected override void OnRecvPacket(ArraySegment<byte> data)
        {
            string message = System.Text.Encoding.UTF8.GetString(data.Array, 0, data.Count);
            Console.WriteLine($"Recv : {message}");
            Program.form.RecvMessage(message);
        }

        protected override void OnSendPacket(int numOfBytes)
        {
            Console.WriteLine($"Send : {numOfBytes}");
        }
    }
}
