using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Network;

public class UI_Omok : MonoBehaviour
{
    [SerializeField]
    GameObject _omokPostionPrefab;
    [SerializeField]
    GameObject _positionGrid;
    [SerializeField]
    GameObject _myStoneText;
    [SerializeField]
    GameObject _curTurnText;

    List<List<UI_OmokPosition>> _positions;

    protected virtual void Start()
    {
        _positions = new List<List<UI_OmokPosition>>(15);

        for (int x = 0; x < 15; x++)
        {
            int localX = x;

            var positionsRow = new List<UI_OmokPosition>(15);
            _positions.Add(positionsRow);

            for (int y = 0; y < 15; y++)
            {
                int localY = y;

                GameObject omokPositionGo = Instantiate(_omokPostionPrefab, _positionGrid.transform);
                UI_OmokPosition omokPosition = omokPositionGo.GetComponent<UI_OmokPosition>();
                omokPosition._positionButton.onClick.AddListener(() => OnClickPosition(localX, localY));
                positionsRow.Add(omokPosition);
            }
        }
    }

    protected virtual void Update()
    {
        _myStoneText.GetComponent<Text>().text = $"나의 돌 : {(Managers.Network.MyStone == StoneType.Black ? "흑돌" : "백돌")}";
        _curTurnText.GetComponent<Text>().text = $"현재 턴 : {(Managers.Network.CurTurn == StoneType.Black ? "흑돌" : "백돌")}";
    }

    public void OnMove(StoneType type, int x, int y)
    {
        UI_OmokPosition position = _positions[x][y];
        position._positionButton.enabled = false;
        position._positionImage.color = type == StoneType.Black ? new Color(0, 0, 0, 255) : new Color(255, 255, 255, 255);
    }

    protected void OnClickPosition(int x, int y)
    {
        // 내 턴이 아니라면
        if (Managers.Network.CurTurn != Managers.Network.MyStone)
            return;

        C_PlaceStonePacket c_MovePacket = new C_PlaceStonePacket();
        c_MovePacket.PosX = x;
        c_MovePacket.PosY = y;
        Managers.Network.Send(c_MovePacket);
    }
}
