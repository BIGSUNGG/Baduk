using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class RoomManager
    {
        private object _lock = new object();
        private List<Room> Rooms { get; set; } = new List<Room>();
        private HashSet<ClientSession> WaitingSessions = new HashSet<ClientSession>();
        int lastId = 0;
        int RoomSize = 2;

        private static RoomManager _instance;
        public static RoomManager Instance 
        {
            get 
            { 
                if (_instance == null)
                    _instance = new RoomManager();

                return _instance;
            }
        }

        private RoomManager() { }

        public void RegisterSession(ClientSession session)
        {
            lock (_lock)
            {
                WaitingSessions.Add(session);
                LogManager.Instance.PushMessage($"Regist user {session.Name}");

                // 매칭 중인 유저가 2명 이상이라면
                if(WaitingSessions.Count >= RoomSize)
                {
                    Room room = new Room(lastId++);
                    Rooms.Add(room);

                    // 2명의 유저를 하나의 룸으로 참가
                    for(int i = 0; i < RoomSize; i++)
                    {
                        ClientSession waitingSession = WaitingSessions.First();
                        WaitingSessions.Remove(waitingSession);
                        room.Enter(waitingSession);
                    }

                    room.Start();
                }
            }
        }

        public void UnregisterSession(ClientSession session)
        {
            lock (_lock)
            {
                LogManager.Instance.PushMessage($"Unregist user {session.Name}");
                WaitingSessions.Remove(session);
            }
        }
    }
}
