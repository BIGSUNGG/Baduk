namespace Network
{
    public class C_LogInPacket : Packet
    {
        public string Name { get; set; }
        public string Password { get; set; }

        public C_LogInPacket()
        {
            Type = PacketType.C_LogIn; 
        }
    }
}