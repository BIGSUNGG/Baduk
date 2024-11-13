namespace Network
{
    public class C_PlaceStonePacket : Packet
    {
        public override PacketType Type => PacketType.C_PlaceStone;

        public int PosX { get; set; }
        public int PosY { get; set; }
    }
}