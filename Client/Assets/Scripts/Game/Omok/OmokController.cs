using Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// TODO : OmokController�� UI_OmokBoard�� ���յ� ���߱� (������ ������ ����Ͽ�)
public class OmokController : MonoBehaviour
{
    public StoneType MyStone { get; private set; } = StoneType.None;
    public StoneType CurTurn { get; private set; } = StoneType.Black;

    // TODO : �̷� �� ���ֱ�
    UI_OmokBoard _omokUI { get; set; }

    protected virtual void Start()
    {
        _omokUI = GameObject.Find("UI_OmokBoard").GetComponent<UI_OmokBoard>();
    }

    protected virtual void Update()
    {
        
    }

    public void Place(int posX, int posY)
    {
        // �� ���� �ƴ϶��
        if (CurTurn != MyStone)
            return;

        C_PlaceStonePacket c_MovePacket = new C_PlaceStonePacket();
        c_MovePacket.PosX = posX;
        c_MovePacket.PosY = posY;
        Managers.Network.Send(c_MovePacket);
    }

    public void OnStart(StoneType myStone, StoneType curStone, List<PositionInfo> positions)
    {
        MyStone = myStone;     
        CurTurn = curStone;

        foreach (var position in positions)
            _omokUI.OnPlace(position.Stone, position.PosX, position.PosY);
    }

    public void OnPlace(StoneType mover, int posX, int posY)
    {
        CurTurn = (mover == StoneType.Black ? StoneType.White : StoneType.Black);
        _omokUI.OnPlace(mover, posX, posY);
    }

    public void OnFinish(StoneType winner)
    {
        _omokUI.OnFinishGame(winner);
    }
}
