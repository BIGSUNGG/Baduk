using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class Room
    {
        private object _lock = new object();
        public List<ClientSession> Sessions = new List<ClientSession>();
        public int Id;

        public Room(int inId)
        {
            Id = inId;           
        }

        public void Start()
        {
            lock(_lock)
            {
                S_EnterRoomPacket s_EnterRoomPacket = new S_EnterRoomPacket();
                foreach (var session in Sessions)
                    s_EnterRoomPacket.Players.Add(session.Name);

                SendAll(s_EnterRoomPacket);
                LogManager.Instance.PushMessage($"Room Start");
            }
        }

        public void Enter(ClientSession session)
        {
            lock (_lock)
            {
                Sessions.Add(session);
                session.OnEnterRoom(this);

                LogManager.Instance.PushMessage($"User {session.Name} Enter {Id} Room");
            }
        }

        public void Leave(ClientSession session)
        {
            lock (_lock)
            {
                if (Sessions.Remove(session))
                {
                    session.OnLeaveRoom();

                    LogManager.Instance.PushMessage($"User {session.Name} Leave {Id} Room");
                }
            }
        }

        public void SendAll(Packet packet)
        {
            SendAll(null, packet);
        }

        public void SendAll(Session sender, Packet packet)
        {
            lock (_lock)
            {
                foreach (var session in Sessions)
                {
                    if (session == sender)
                        continue;

                    session.Send(packet);
                }
            }
        }
    }
}
