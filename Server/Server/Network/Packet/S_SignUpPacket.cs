namespace Network
{
    public class S_SignUpPacket : Packet
    {
        public bool Success { get; set; }
        public string Name { get; set; }

        public S_SignUpPacket()
        {
            Type = PacketType.S_SignUp; 
        }
    }
}