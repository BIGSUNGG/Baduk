using Server;
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
        public string Name = string.Empty;
        public Room MyRoom = null;

        protected override void OnConnect()
        {
            Trace.WriteLine($"Connect");

            Send("Name?");
        }

        protected override void OnDiscconect()
        {
            Trace.WriteLine($"Disconnect");
        }

        protected override void OnRecvPacket(ArraySegment<byte> data)
        {
            string message = System.Text.Encoding.UTF8.GetString(data.Array, 0, data.Count);
            Trace.WriteLine($"Recv : {message}");

            if (Name == string.Empty)
            {
                Name = message;
                MyRoom = RoomManager.Instance.GetEnterableRoom(this);
            }
            else
            {
                foreach (var session in MyRoom.Sessions)
                {
                    LogManager.Instance.PushMessage($"{MyRoom.Id} : {Name} : {message}");
                    session.Send($"{Name} : {message}");
                }
            }
        }

        protected override void OnSendPacket(int numOfBytes)
        {
            Trace.WriteLine($"Send : {numOfBytes}");
        }
    }
}
