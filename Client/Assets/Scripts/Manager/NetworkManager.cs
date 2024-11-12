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
        public string Name { get; set; }
        public bool IsConnect => _serverSession._connecting;
        public StoneType MyStone { get; set; }
        public StoneType CurTurn { get; set; }

        ServerSession _serverSession = new ServerSession();

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
