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
    public partial class ClientSession
    {
        object _lock = new object();
        public string Name = string.Empty;
        public OmokRoom MyRoom { get; private set; }
        public int Score;

        public void OnEnterRoom(OmokRoom room)
        {
            MyRoom = room;  
        }

        public void OnLeaveRoom()
        {
            MyRoom = null;
        }
    }
}
