using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Network
{
    public class NetworkManager
    {
        ServerSession _serverSession = new ServerSession();
        public bool IsConnect => _serverSession._connecting;

        public void Connect()
        {
            Connector connector = new Connector();
            connector.Connect(() => { return _serverSession; });
        }

        public void Disconnect()
        {
            _serverSession.Disconnect();
            _serverSession = null;
        }

        public void Send(Packet packet)
        {
            _serverSession.Send(packet);
        }

        public void Update()
        {
            var q = _serverSession.PopQueue();
            while (q.Count > 0)
            {
                Debug.Log(q.Dequeue());
            }
        }
    }
}
