namespace Network
{
    public class C_SignUpPacket : Packet
    {
        public override PacketType Type => PacketType.C_SignUp;
    
        public string Name { get; set; }
        public string Password { get; set; }
    }
}