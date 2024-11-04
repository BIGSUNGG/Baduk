using Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    public partial class ClientSession : Session
    {
        protected override void OnConnect()
        {
            LogManager.Instance.PushMessage($"Connect");

            //// Keep Alive
            //int size = Marshal.SizeOf(typeof(uint));
            //byte[] inOptionValues = new byte[size * 3];

            //// Keep Alive 활성화 여부 1 : 활성화, 0 : 비활성화
            //BitConverter.GetBytes((uint)1).CopyTo(inOptionValues, 0);
            //// 마지막 패킷을 받은 시간으로 부터 몇 초가 지나야 Keep Alive를 확인할 것인지
            //BitConverter.GetBytes((uint)5000).CopyTo(inOptionValues, size); 
            //// Keep Alive 패킷의 반응이 없을 때 재전송할 시간
            //BitConverter.GetBytes((uint)1000).CopyTo(inOptionValues, size * 2); 

            //// Keep Alive 설정
            //_socket.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);

            Send("Name?");
        }

        protected override void OnDiscconect()
        {
            LogManager.Instance.PushMessage($"Disconnect {Name}");

            RoomManager.Instance.UnregisterSession(this);

            Listener.clients.Remove(this);

            if (MyRoom != null)
                MyRoom.Leave(this);
        }

        protected override void OnRecvPacket(ArraySegment<byte> data)
        {
            string message = System.Text.Encoding.UTF8.GetString(data.Array, 0, data.Count);
            //LogManager.Instance.PushMessage($"Recv : {message}");

            if (Name == string.Empty)
            {
                Name = message;
                RoomManager.Instance.RegisterSession(this);
                Send($"You are {Name}");
            }
            else
            {
                if (MyRoom != null)
                    MyRoom.SendAll(this, message);
            }
        }

        protected override void OnSendPacket(int numOfBytes)
        {
            //LogManager.Instance.PushMessage($"Send : {numOfBytes}");
        }
    }
}
