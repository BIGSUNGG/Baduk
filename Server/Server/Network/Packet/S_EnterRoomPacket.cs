using System.Collections.Generic;

namespace Network
{
	public class S_EnterRoomPacket : Packet
	{
        public override PacketType Type => PacketType.S_EnterRoom;

		public string WhitePlayerName { get; set; }
		public string BlackPlayerName { get; set; } 

		public StoneType CurStone { get; set; }
        public StoneType YourStone { get; set; }

		public List<PositionInfo> Positions { get; set; } = new List<PositionInfo>();
	}
}