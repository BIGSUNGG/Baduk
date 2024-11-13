namespace Network
{
    public class S_PlaceStonePacket : Packet
    {
        public override PacketType Type => PacketType.S_PlaceStone;

        public StoneType Mover { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
    }
}