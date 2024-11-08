using Newtonsoft.Json;

namespace Network
{
    public class Packet
    {
        public PacketType Type { get; set; } = PacketType.Unknown;
    }
}