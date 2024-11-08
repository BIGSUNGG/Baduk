using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class ClientSessionManager
    {       
        private static ClientSessionManager _instance;
        public static ClientSessionManager Instance
        {
            get 
            {
                if (_instance == null)
                    _instance = new ClientSessionManager();

                return _instance;
            }
        }

        private ClientSessionManager() { }

        object _lock = new object();
        int _lastId = 0;
        Dictionary<int, ClientSession> _clients = new Dictionary<int, ClientSession>();

        public void SendAll(Packet packet)
        {
            List<ClientSession> sessions;
            lock (_lock)
            {
                 sessions = _clients.Values.ToList();
            }
            
            if(sessions != null)
            {
                foreach (var session in sessions)
                    session.Send(packet);
            }
        }

        public ClientSession Create()
        {
            ClientSession created =  new ClientSession(_lastId++);
            lock (_lock)
            {
                _clients.Add(created.Id, created);
                return created;
            }
        }

        public void Remove(ClientSession removeSession)
        {
            lock(_lock)
            {
                _clients.Remove(removeSession.Id);
            }
        }

        public ClientSession Find(int sessionId)
        {
            lock(_lock)
            {
                if(_clients.TryGetValue(sessionId, out var session))
                    return session;

                return null;
            }
        }
    }
}
