namespace Network
{
    public enum RequestTopRankType
    {
        TopRank,
        NearRank,
    }

    public class C_RequestTopRankPacket : Packet
    {
        public override PacketType Type => PacketType.C_RequestTopRank;

        public RequestTopRankType RequestType { get; set; }
    }
}  