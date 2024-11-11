namespace Network
{
    public class S_SignUpPacket : Packet
    {
        public override PacketType Type => PacketType.S_SignUp;

        public bool Success { get; set; }
        public string Name { get; set; }
    }
}