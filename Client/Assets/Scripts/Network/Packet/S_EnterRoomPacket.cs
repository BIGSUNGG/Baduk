using System.Collections.Generic;

namespace Network
{
	public class S_EnterRoomPacket : Packet
	{
		public List<string> Players = new List<string>();

		public S_EnterRoomPacket()
		{
			Type = PacketType.S_EnterRoom;
		}
	}
}