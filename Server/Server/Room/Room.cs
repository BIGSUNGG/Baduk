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

        public void Enter(ClientSession sesesion)
        {
            lock (_lock)
            {
                Sessions.Add(sesesion);
                LogManager.Instance.PushMessage($"User {sesesion.Name} Enter {Id} Room");
                sesesion.Send($"You Enter {Id} Room");
            }
        }
    }
}
