namespace Network
{
    public class C_SignUpPacket : Packet
    {
        public string Name { get; set; }
        public string Password { get; set; }

        public C_SignUpPacket()
        {
            Type = PacketType.C_SignUp;
        }
    }
}