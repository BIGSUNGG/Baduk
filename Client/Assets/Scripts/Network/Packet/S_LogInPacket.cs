namespace Network
{
    public class S_LogInPacket : Packet
    {
        public override PacketType Type => PacketType.S_LogIn;

        public bool Success { get; set; }
        public string Name { get; set; }
    }
}