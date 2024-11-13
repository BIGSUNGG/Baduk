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

        List<OmokRoom> _rooms { get; set; } = new List<OmokRoom>();
        int _lastRoomId = 0;

        // 매칭 중인 세션 목록
        HashSet<ClientSession> _matchingSessions = new HashSet<ClientSession>();
        // 매칭 가능한 최대 점수 차이
        int _maxMatchingRange = 200;

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

        public void RegisterSession(ClientSession inSession)
        {
            lock (_lock)
            {
                LogManager.Instance.PushMessage($"Register user {inSession.Name}, Score : {inSession.Score}");

                foreach(var clientSession in _matchingSessions)
                {
                    // 점수 차이가 200점 이하라면 매칭
                    if(Math.Abs(clientSession.Score - inSession.Score) <= _maxMatchingRange)
                    {
                        _matchingSessions.Remove(clientSession);

                        OmokRoom room = new OmokRoom(_lastRoomId++);
                        _rooms.Add(room);

                        room.Start(clientSession, inSession);

                        return;
                    }
                }

                // 매칭 실패 시 매칭 리스트에 추가
                _matchingSessions.Add(inSession);
            }
        }

        public void UnregisterSession(ClientSession session)
        {
            lock (_lock)
            {
                LogManager.Instance.PushMessage($"Unregister user {session.Name}");
                _matchingSessions.Remove(session);
            }
        }
    }
}
