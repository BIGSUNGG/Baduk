using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Network;
using System;

public class UI_OmokBoard : MonoBehaviour
{
    [SerializeField]
    GameObject _gameWin;
    [SerializeField]
    GameObject _gameLose;

    [SerializeField]
    GameObject _omokPostionPrefab;
    [SerializeField]
    GameObject _positionGrid;
    [SerializeField]
    GameObject _myStoneText;
    [SerializeField]
    GameObject _curTurnText;

    List<List<UI_OmokPosition>> _positions;

    OmokController _omok { get; set; }

    protected virtual void Start()
    {
        _omok = GameObject.Find("OmokController").GetComponent<OmokController>();
        if (_omok == null)
            Debug.LogWarning("OmokController is null");

        // 오목판 설정
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
        if (_omok)
        {
            _myStoneText.GetComponent<Text>().text = $"나의 돌 : {(_omok.MyStone == StoneType.Black ? "흑돌" : "백돌")}";
            _curTurnText.GetComponent<Text>().text = $"현재 턴 : {(_omok.CurTurn == StoneType.Black ? "흑돌" : "백돌")}";
        }
    }

    public void OnMove(StoneType type, int x, int y)
    {
        UI_OmokPosition position = _positions[x][y];
        position._positionButton.enabled = false;
        position._positionImage.color = type == StoneType.Black ? new Color(0, 0, 0, 255) : new Color(255, 255, 255, 255);
    }

    protected void OnClickPosition(int x, int y)
    {
        _omok.Move(x, y);
    }

    public void OnFinishGame(StoneType winner)
    {
        if (_omok.MyStone == winner)
            Instantiate(_gameWin, transform);
        else
            Instantiate(_gameLose, transform);
    }
}
