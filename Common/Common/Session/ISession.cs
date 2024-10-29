using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ServerCore
{
    public interface ISession
    {
        public void Send(ArraySegment<byte> sendBuff);
    }
}
