using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace Network
{
    public abstract class Session
    {
        private object _lock = new object();
        protected Socket _socket = null;

        SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();
        SocketAsyncEventArgs _recvArgs = new SocketAsyncEventArgs();

        #region Connect
        /// <summary>
        /// 연결 성공 시 호출
        /// </summary>
        /// <param name="args"></param>
        public void Connected(Socket socket)
        {
            _socket = socket;

            _sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);
            _recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecvCompleted);

            RegisterRecv();

            OnConnect();
        }

        protected abstract void OnConnect();
        #endregion

        #region Disconnect
        /// <summary>
        /// 연결 종료 시 호출
        /// </summary>
        public void Disconnect()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();

            OnDiscconect();
        }

        protected abstract void OnDiscconect();
        #endregion

        #region Send
        // Send 로직 : Send => RegisterSend => OnSendCompleted => pendingQueue에 메시지가 있다면 다시 RegisterSend 반복
        Queue<string> sendQueue = new Queue<string>(); // 전송 대기 중인 메시지 큐
        bool sending = false; // 현재 패킷을 전송 중인지

        protected void Send(string message)
        {
            lock(_lock)
            {
                sendQueue.Enqueue(message);
                if (sending == false)
                    RegisterSend();
            }
        }

        private void RegisterSend()
        {
            lock (_lock)
            {
                if (sendQueue.Count == 0)
                    return;

                string message = "";
                while(sendQueue.Count > 0)
                {
                    message += $"{sendQueue.Dequeue()}\n";
                }

                if (message.Equals(string.Empty) == false)
                {
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(message);
                    _sendArgs.SetBuffer(buffer, 0, buffer.Length);

                    sending = true;
                    bool pending = _socket.SendAsync(_sendArgs);
                    if (pending == false)
                        OnSendCompleted(null, _sendArgs);
                }
            }
        }

        private void OnSendCompleted(object sender, SocketAsyncEventArgs args)
        {
            lock (_lock)
            {
                if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
                {
                    _sendArgs.BufferList = null;
                    OnSendPacket(_sendArgs.BytesTransferred);
                }

                sending = false;
                if (sendQueue.Count > 0)
                    RegisterSend();
            }
        }

        protected abstract void OnSendPacket(int numOfBytes);
        #endregion

        #region Recv
        /// <summary>
        /// 데이터 수신 시작
        /// </summary>
        /// <param name="receiveArgs"></param>
        private void RegisterRecv()
        {
            _recvArgs.SetBuffer(new byte[1024], 0, 1024);

            try
            {
                bool pending = _socket.ReceiveAsync(_recvArgs);
                if (pending == false)
                    OnRecvCompleted(null, _recvArgs);
            }
            catch (Exception e)
            {
                Console.WriteLine($"RegisterRecv Failed {e}");
            }
        }

        /// <summary>
        /// 데이터 수신 완료 시 호출
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRecvCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success && e.BytesTransferred > 0)
            {
                ArraySegment<byte> buff = new ArraySegment<byte>(e.Buffer, 0, e.BytesTransferred);
                OnRecvPacket(buff);

                // 계속해서 데이터를 수신
                RegisterRecv();
            }
            else
            {
                Disconnect();
            }
        }

        protected abstract void OnRecvPacket(ArraySegment<byte> data);
        #endregion
    }
}
