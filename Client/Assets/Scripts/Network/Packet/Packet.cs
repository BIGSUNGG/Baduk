using Newtonsoft.Json;

namespace Network
{
    public class Packet
    {
        public virtual PacketType Type { get; set; } = PacketType.Unknown;
    }
}