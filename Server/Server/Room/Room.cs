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
        public HashSet<ClientSession> Sessions = new HashSet<ClientSession>();
        public int Id;

        public Room(int inId)
        {
            Id = inId;           
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

        public void SendAll(ClientSession sender, Packet packet)
        {
            lock (_lock)
            {
                foreach (var session in Sessions)
                {
                    session.Send(packet);
                }
            }
        }
    }
}
