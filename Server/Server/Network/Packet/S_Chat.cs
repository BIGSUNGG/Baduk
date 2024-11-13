namespace Network
{
    public class S_Chat : Packet
    {
        public override PacketType Type => PacketType.S_Chat;

        public string Sender { get; set; }
        public string Message { get; set; }
    }
}