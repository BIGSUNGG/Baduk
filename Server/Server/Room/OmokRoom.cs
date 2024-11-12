using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class OmokRoom
    {
        public int Id;

        private object _lock = new object();
        ClientSession _blackPlayer;
        ClientSession _whitePlayer;
        StoneType _curTurn = StoneType.Black;

        List<List<StoneType>> _stones;

        public OmokRoom(int inId)
        {
            Id = inId;

            _stones = new List<List<StoneType>>(15);
            for (int i = 0; i < 15; i++)
            {
                _stones.Add(new List<StoneType>(15));
                for (int j = 0; j < 15; j++)
                {
                    _stones[i].Add(StoneType.None);
                }
            }
        }

        public void Start(ClientSession white, ClientSession black)
        {
            lock (_lock)
            {
                _whitePlayer = white;
                _whitePlayer.OnEnterRoom(this);

                _blackPlayer = black;
                _blackPlayer.OnEnterRoom(this);

                // 흑돌 유저
                {
                    S_EnterRoomPacket s_EnterRoomPacket = new S_EnterRoomPacket();
                    s_EnterRoomPacket.BlackPlayerName = _blackPlayer.Name;
                    s_EnterRoomPacket.WhitePlayerName = _whitePlayer.Name;
                    s_EnterRoomPacket.YourStone = StoneType.Black;
                    _blackPlayer.Send(s_EnterRoomPacket);
                }

                // 백돌 유저
                {
                    S_EnterRoomPacket s_EnterRoomPacket = new S_EnterRoomPacket();
                    s_EnterRoomPacket.BlackPlayerName = _blackPlayer.Name;
                    s_EnterRoomPacket.WhitePlayerName = _whitePlayer.Name;
                    s_EnterRoomPacket.YourStone = StoneType.White;
                    _whitePlayer.Send(s_EnterRoomPacket);
                }

                LogManager.Instance.PushMessage($"Room Start");
            }
        }

        public void MoveAction(ClientSession mover, C_MovePacket c_MovePacket)
        {
            lock (_lock)
            {
                // 현재 흑돌의 턴이지만 흑돌이 아닌 플레이어가 수를 둔다면
                if (_curTurn == StoneType.Black && mover != _blackPlayer)
                    return;

                // 현재 백돌의 턴이지만 백돌이 아닌 플레이어가 수를 둔다면
                if (_curTurn == StoneType.White && mover != _whitePlayer)
                    return;

                // 이미 수가 두어진 곳에 수를 두려고 한다면
                if (_stones[c_MovePacket.PosX][c_MovePacket.PosY] != StoneType.None)
                    return;

                // 수 두고 알리기
                _stones[c_MovePacket.PosX][c_MovePacket.PosY] = _curTurn;

                S_MovePacket s_MovePacket = new S_MovePacket();
                s_MovePacket.Mover = _curTurn;
                s_MovePacket.PosX = c_MovePacket.PosX;
                s_MovePacket.PosY = c_MovePacket.PosY;
                SendAll(s_MovePacket);

                // 턴 전환
                _curTurn = _curTurn == StoneType.Black ? StoneType.White : StoneType.Black;
            }
        }

        public void SendAll(Packet packet)
        {
            SendAll(null, packet);
        }

        public void SendAll(Session sender, Packet packet)
        {
            lock (_lock)
            {
                _whitePlayer.Send(packet);
                _blackPlayer.Send(packet);
            }
        }
    }
}
