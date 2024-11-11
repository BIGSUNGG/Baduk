namespace Network
{
    public class C_LogInPacket : Packet
    {
        public override PacketType Type => PacketType.C_LogIn;

        public string Name { get; set; }
        public string Password { get; set; }
    }
}