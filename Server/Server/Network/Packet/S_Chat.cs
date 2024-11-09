namespace Network
{
    public class S_Chat : Packet
    {
        public string Sender { get; set; }
        public string Message { get; set; }

        public S_Chat()
        {
            Type = PacketType.S_Chat; 
        }
    }
}