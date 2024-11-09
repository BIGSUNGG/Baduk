namespace Network
{
    public class C_Chat : Packet
    {
        public string Message { get; set; }

        public C_Chat()
        {
            Type = PacketType.C_Chat; 
        }
    }
}