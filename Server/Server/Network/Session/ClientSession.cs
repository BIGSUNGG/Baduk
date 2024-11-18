using Newtonsoft.Json;
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
using ServerDB;

namespace Network
{
    public partial class ClientSession : Session
    {
        public readonly int Id;
        public AccountGateWay Account { get; set; }

        public ClientSession(int inId)
        {
            Id = inId;
        }
        
        public void Send(Packet packet)
        {
            string packetJson = JsonConvert.SerializeObject(packet);
            Send(packetJson);
        }

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
        }

        protected override void OnDiscconect()
        {
            LogManager.Instance.PushMessage($"Disconnect {Name}");

            ClientSessionManager.Instance.Remove(this);
            RoomManager.Instance.UnregisterSession(this);
        }

        protected override void OnRecvPacket(ArraySegment<byte> data)
        {
            string json = System.Text.Encoding.UTF8.GetString(data.Array, 0, data.Count);
            Packet packet = JsonConvert.DeserializeObject<Packet>(json);

            switch (packet.Type)
            {
                case PacketType.C_LogIn:
                    {
                        C_LogInPacket c_LoginPacket = JsonConvert.DeserializeObject<C_LogInPacket>(json);

                        Account = new AccountGateWay();
                        bool success = Account.Select(c_LoginPacket.Name, c_LoginPacket.Password);
                        if (success)
                        {
                            LogManager.Instance.PushMessage($"User {Account.Name} Log In success");

                            Name = Account.Name;
                            Score = Account.Score;

                            S_LogInPacket s_LogInPacket = new S_LogInPacket();
                            s_LogInPacket.Success = true;
                            s_LogInPacket.Name = Account.Name;
                            Send(s_LogInPacket);
                        }
                        else
                        {
                            LogManager.Instance.PushMessage($"User {Name} Log In fail");

                            S_LogInPacket s_LogInPacket = new S_LogInPacket();
                            s_LogInPacket.Success = false;
                            Send(s_LogInPacket);
                        }

                        break;
                    }
                case PacketType.C_SignUp:
                    {
                        C_SignUpPacket signupPacket = JsonConvert.DeserializeObject<C_SignUpPacket>(json);

                        Account = new AccountGateWay();
                        Account.Name = signupPacket.Name;
                        Account.Password = signupPacket.Password;
                        Account.Score = 1000;

                        bool successs = Account.Insert();
                        if (successs)
                        {
                            LogManager.Instance.PushMessage($"User {Name} Sign Up success");

                            Name = Account.Name;

                            S_SignUpPacket s_SignUpPacket = new S_SignUpPacket();
                            s_SignUpPacket.Success = true;
                            s_SignUpPacket.Name = Account.Name;
                            Send(s_SignUpPacket);
                        }
                        else
                        {
                            LogManager.Instance.PushMessage($"User {Name} Sign Up fail");

                            S_SignUpPacket s_SignUpPacket = new S_SignUpPacket();
                            s_SignUpPacket.Success = false;
                            Send(s_SignUpPacket);
                        }

                        break;
                    }
                case PacketType.C_StartMatch:
                    {
                        C_StartMatchPacket c_StartMatchPacket = JsonConvert.DeserializeObject<C_StartMatchPacket>(json);

                        RoomManager.Instance.RegisterSession(this);

                        break;
                    }
                case PacketType.C_Chat:
                    {
                        C_ChatPacket c_Chat = JsonConvert.DeserializeObject<C_ChatPacket>(json);

                        if(MyRoom != null)
                        {
                            S_ChatPacket s_Chat = new S_ChatPacket();
                            s_Chat.Sender = Name;
                            s_Chat.Message = c_Chat.Message;
                            MyRoom.SendAll(this, s_Chat);
                        }

                        break;
                    }
                case PacketType.C_PlaceStone:
                    {
                        C_PlaceStonePacket c_Chat = JsonConvert.DeserializeObject<C_PlaceStonePacket>(json);

                        if (MyRoom != null)
                        {
                            MyRoom.PlaceStone(this, c_Chat);
                        }

                        break;
                    }
                case PacketType.C_RequestTopRank:
                    {
                        C_RequestTopRankPacket c_RequestTopRankPacket = JsonConvert.DeserializeObject<C_RequestTopRankPacket>(json);

                        S_ResponseTopRankPacket s_ResponseTopRankPacket = new S_ResponseTopRankPacket();
                       
                        int leftRank;
                        int rightRank;

                        if (c_RequestTopRankPacket.RequestType == RequestTopRankType.NearRank)
                        {
                            int myRank = Account.GetRank();
                            // leftRank가 1보다 작을 수 없도록
                            leftRank = myRank - 4;
                            leftRank = leftRank < 1 ? 1 : leftRank;
                            rightRank = leftRank + 9;
                        }
                        else if (c_RequestTopRankPacket.RequestType == RequestTopRankType.TopRank)
                        {
                            leftRank = 1;
                            rightRank = 10;
                        }
                        else
                        {
                            throw new NotImplementedException();
                        }

                        List<AccountGateWay> accounts = AccountGateWay.GetTopRank(leftRank, rightRank);
                        for (int i = 0; i < accounts.Count; i++)
                        {
                            UserInfo info = new UserInfo();
                            info.Name = accounts[i].Name;
                            info.Score = accounts[i].Score;
                            info.Rank = i + leftRank;
                            s_ResponseTopRankPacket.Users.Add(info);
                        }

                        Send(s_ResponseTopRankPacket);
                        break;
                    }
                default:
                    break;
            }
        }

        protected override void OnSendPacket(int numOfBytes)
        {
            //LogManager.Instance.PushMessage($"Send : {numOfBytes}");
        }
    }
}
