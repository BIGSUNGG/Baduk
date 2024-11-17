using Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OmokController : MonoBehaviour
{
    public StoneType MyStone { get; private set; } = StoneType.None;
    public StoneType CurTurn { get; private set; } = StoneType.Black;

    UI_OmokBoard _omokUI { get; set; }

    protected virtual void Start()
    {
        _omokUI = GameObject.Find("UI_OmokBoard").GetComponent<UI_OmokBoard>();
    }

    protected virtual void Update()
    {
        
    }

    public void Move(int posX, int posY)
    {
        // 내 턴이 아니라면
        if (CurTurn != MyStone)
            return;

        C_PlaceStonePacket c_MovePacket = new C_PlaceStonePacket();
        c_MovePacket.PosX = posX;
        c_MovePacket.PosY = posY;
        Managers.Network.Send(c_MovePacket);
    }

    public void OnStart(StoneType myStone)
    {
        CurTurn = StoneType.Black;
        MyStone = myStone;
    }

    public void OnMove(StoneType mover, int posX, int posY)
    {
        CurTurn = (mover == StoneType.Black ? StoneType.White : StoneType.Black);
        _omokUI.OnMove(mover, posX, posY);
    }

    public void OnFinish(StoneType winner)
    {
        _omokUI.OnFinishGame(winner);
    }
}
