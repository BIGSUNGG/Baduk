using Network;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Network
{
    public class ServerSession : Session
    {
        object _lock = new object();
        public bool _connecting { get; private set; } = false;
        Queue<string> _recvQueue = new Queue<string>();

        public Queue<string> PopQueue()
        {
            lock(_lock)
            {
                Queue<string> result = _recvQueue;
                _recvQueue = new Queue<string>();
                return result;
            }
        }

        public void Send(Packet packet)
        {
            string packetJson = JsonConvert.SerializeObject(packet);
            Send(packetJson);
        }

        protected override void OnConnect()
        {
            Console.WriteLine($"Connect");

            _connecting = true;
        }

        protected override void OnDiscconect()
        {
            Console.WriteLine($"Disconnect");

            _connecting = false;
        }

        protected override void OnRecvPacket(ArraySegment<byte> data)
        {
            string json = System.Text.Encoding.UTF8.GetString(data.Array, 0, data.Count);
            Packet packet = JsonConvert.DeserializeObject<Packet>(json);

            switch (packet.Type)
            {
                case PacketType.S_LogIn:
                    {
                        S_LogInPacket s_LoginPacket = JsonConvert.DeserializeObject<S_LogInPacket>(json);
                        if(s_LoginPacket.Success)
                        {
                            Managers.Network.Name = s_LoginPacket.Name;

                            UnityEvent sceneMove = new UnityEvent();
                            sceneMove.AddListener(() => SceneManager.LoadScene("Matching Scene", LoadSceneMode.Single));
                            Managers.Timer.SetTimerNextUpdate(sceneMove);                           
                        }

                        break;
                    }
                case PacketType.S_SignUp:
                    {
                        S_SignUpPacket s_SignUpPacket = JsonConvert.DeserializeObject<S_SignUpPacket>(json);
                        if (s_SignUpPacket.Success)
                        {
                            Managers.Network.Name = s_SignUpPacket.Name;

                            UnityEvent sceneMove = new UnityEvent();
                            sceneMove.AddListener(() => SceneManager.LoadScene("Matching Scene", LoadSceneMode.Single));
                            Managers.Timer.SetTimerNextUpdate(sceneMove);
                        }

                        break;
                    }
                case PacketType.S_EnterRoom:
                    {
                        S_EnterRoomPacket s_SignUpPacket = JsonConvert.DeserializeObject<S_EnterRoomPacket>(json);

                        Managers.Network.MyStone = s_SignUpPacket.YourStone;
                        Managers.Network.CurTurn = StoneType.Black;

                        UnityEvent sceneMove = new UnityEvent();
                        sceneMove.AddListener(() => SceneManager.LoadScene("Baduk Scene", LoadSceneMode.Single));
                        Managers.Timer.SetTimerNextUpdate(sceneMove);

                        break;
                    }
                case PacketType.S_Chat:
                    {
                        S_ChatPacket s_Chat = JsonConvert.DeserializeObject<S_ChatPacket>(json);

                        UnityEvent sceneMove = new UnityEvent();
                        sceneMove.AddListener(() =>
                        {
                            GameObject chatBoxGo = GameObject.Find("UI_ChatBox");
                            if (chatBoxGo)
                            {
                                UI_ChatBox chatBox = chatBoxGo.GetComponent<UI_ChatBox>();
                                chatBox.PushMessage(s_Chat.Sender, s_Chat.Message);
                            }
                        });
                        Managers.Timer.SetTimerNextUpdate(sceneMove);

                        break;
                    }
                case PacketType.S_PlaceStone:
                    {
                        S_PlaceStonePacket s_Move = JsonConvert.DeserializeObject<S_PlaceStonePacket>(json);

                        // 턴 전환
                        Managers.Network.CurTurn = s_Move.Mover == StoneType.Black ? StoneType.White : StoneType.Black;

                        UnityEvent sceneMove = new UnityEvent();
                        sceneMove.AddListener(() =>
                        {
                            GameObject omokGo = GameObject.Find("UI_Omok");
                            if (omokGo)
                            {
                                UI_Omok omok = omokGo.GetComponent<UI_Omok>();
                                omok.OnMove(s_Move.Mover, s_Move.PosX, s_Move.PosY);
                            }
                        });
                        Managers.Timer.SetTimerNextUpdate(sceneMove);

                        break;
                    }
                case PacketType.S_GameFinish:
                    {
                        S_GameFinishPacket s_GameFinishPacket = JsonConvert.DeserializeObject<S_GameFinishPacket>(json);

                        UnityEvent sceneMove = new UnityEvent();
                        sceneMove.AddListener(() =>
                        {
                            GameObject badukSceneGo = GameObject.Find("UI_BadukScene");
                            if (badukSceneGo)
                            {
                                UI_OmokScene omokScene = badukSceneGo.GetComponent<UI_OmokScene>();
                                omokScene.OnFinishGame(s_GameFinishPacket.Winner);
                            }
                        });
                        Managers.Timer.SetTimerNextUpdate(sceneMove);

                        break;
                    }
                default:
                    break;
            }
        }

        protected override void OnSendPacket(int numOfBytes)
        {
            Console.WriteLine($"Send : {numOfBytes}");
        }
    }
}
