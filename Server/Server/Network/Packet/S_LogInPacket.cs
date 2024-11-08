namespace Network
{
    public class S_LogInPacket : Packet
    {
        public bool Success { get; set; }
        public string Name { get; set; }

        public S_LogInPacket()
        {
            Type = PacketType.S_LogIn; 
        }
    }
}