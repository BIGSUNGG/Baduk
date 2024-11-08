using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using static System.Collections.Specialized.BitVector32;

namespace Network
{
    public class Listener
    {
        object _lock = new object();
        Socket _listenSocket;
        Func<Session> _sessionFactory;

        public void Listen(Func<Session> sessionFactory)
        {
            _sessionFactory = sessionFactory;

            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddress = ipHost.AddressList[1];
            IPEndPoint endPoint = new IPEndPoint(ipAddress, 7777);
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            _listenSocket.Bind(endPoint);
            _listenSocket.Listen(100);

            // 클라이언트의 승인 요청을 받는 작업을 할 비동기소켓작업 객체를 생성하고,
            // 작업이 끝나면 OnAcceptCompleted를 호출하도록 이벤트핸들러에 등록
            for (int i = 0; i < 10; i++)
            {
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
                RegisterAccept(args);
            }
        }

        /// <summary>
        /// 클라이언트의 접속 요청을 처리
        /// </summary>
        /// <param name="args"></param>
        void RegisterAccept(SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null;

            bool pending = _listenSocket.AcceptAsync(args);
            if (pending == false)
            {
                OnAcceptCompleted(null, args);
            }
        }

        /// <summary>
        /// 클라이언트 접속 완료 시 호출
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                // Client Session 생성
                Session session = _sessionFactory();
                session.Connected(args.AcceptSocket);
            }

            // 클라이언트 접속 완료 시 다른 클라이언트의 연결 처리를 위해 RegisterAccept반복
            RegisterAccept(args);
        }
    }
}
