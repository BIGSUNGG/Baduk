using System.Collections.Generic;

namespace Network
{
	public class S_EnterRoomPacket : Packet
	{
        public override PacketType Type => PacketType.S_EnterRoom;

		public List<string> Players = new List<string>();
	}
}