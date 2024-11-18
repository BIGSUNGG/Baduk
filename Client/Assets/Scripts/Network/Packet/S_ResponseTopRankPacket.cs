using System.Collections.Generic;

namespace Network
{
    public class S_ResponseTopRankPacket : Packet
    {
        public override PacketType Type => PacketType.S_ResponseTopRank;

        public List<UserInfo> Users { get; set; } = new List<UserInfo>();
    }
}