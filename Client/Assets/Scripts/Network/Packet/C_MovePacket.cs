namespace Network
{
    public class C_MovePacket : Packet
    {
        public override PacketType Type => PacketType.C_Move;

        public int PosX { get; set; }
        public int PosY { get; set; }
    }
}