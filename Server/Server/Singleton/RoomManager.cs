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
        int lastId = 0;

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

        public Room GetEnterableRoom(ClientSession session)
        {
            lock(_lock)
            {
	            foreach (var room in Rooms)
	            {
                    if (room.Sessions.Count < 2)
                    {
                        room.Enter(session);
                        return room;
                    }
	            }

                Room newRoom = new Room();
                newRoom.Id = lastId++;
                newRoom.Enter(session);
                Rooms.Add(newRoom);
                return newRoom;
            }
        }
    }
}
