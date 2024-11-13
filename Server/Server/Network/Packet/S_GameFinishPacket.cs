namespace Network
{
    public class S_GameFinishPacket : Packet
    {
        public override PacketType Type => PacketType.S_GameFinish;

        public StoneType Winner { get; set; }
    }
}