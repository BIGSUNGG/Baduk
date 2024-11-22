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

        /// <summary>
        /// Key : Client Name
        /// Value : 이전에 들어가 있던 룸
        /// </summary>
        Dictionary<string, OmokRoom> _disconnectedSessions = new Dictionary<string, OmokRoom>();

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
                // 이미 등록된 유저라면
                if (_matchingSessions.Contains(inSession))
                    return;

                LogManager.Instance.PushMessage($"Register user {inSession.Name}, Score : {inSession.Score}");

                // TODO : 최적화 기존 n의 속도로 처리되던 로직을 _matchingSessions를 점수별로 정렬 후 이분탐색으로 log n의 속도로 처리
                foreach (var clientSession in _matchingSessions)
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

        public void MatchSession(ClientSession session)
        {
            lock (_lock)
            {
                if(_disconnectedSessions.TryGetValue(session.Name, out var room))
                {
                    _disconnectedSessions.Remove(session.Name);
                    room.ComeBack(session);
                    return;
                }
                else
                {
                    RegisterSession(session);
                }
            }
        }

        public void DisconnectSession(ClientSession session)
        {
            lock (_lock)
            {
                if(session.MyRoom != null)
                    _disconnectedSessions.Add(session.Name, session.MyRoom);
                else
                    UnregisterSession(session);
            }
        }
    }
}
