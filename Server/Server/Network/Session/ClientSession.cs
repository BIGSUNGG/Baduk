using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    public class ClientSession : Session
    {
        protected override void OnConnect()
        {
            Trace.WriteLine($"Connect");

            Send("Hello");
        }

        protected override void OnDiscconect()
        {
            Trace.WriteLine($"Disconnect");
        }

        protected override void OnRecvPacket(ArraySegment<byte> data)
        {
            string message = System.Text.Encoding.UTF8.GetString(data.Array, 0, data.Count);
            Trace.WriteLine($"Recv : {message}");

            foreach (var session in Listener.clients)
            {
                session.Send(message);
            }
        }

        protected override void OnSendPacket(ArraySegment<byte> data)
        {
            string message = System.Text.Encoding.UTF8.GetString(data.Array, 0, data.Count);
            Trace.WriteLine($"Send : {message}");
        }
    }
}
