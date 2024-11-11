namespace Network
{
    public class C_Chat : Packet
    {
        public override PacketType Type => PacketType.C_Chat;

        public string Message { get; set; }
    }
}