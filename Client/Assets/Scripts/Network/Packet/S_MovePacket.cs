namespace Network
{
    public class S_MovePacket : Packet
    {
        public override PacketType Type => PacketType.S_Move;

        public StoneType Mover { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
    }
}