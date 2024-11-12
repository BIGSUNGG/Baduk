using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_OmokBase : MonoBehaviour
{
    public GameObject _omokPostionPrefab;
    public GameObject _positionGrid;

    private List<List<UI_OmokPosition>> _positions;

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
        
    }

    protected void OnClickPosition(int x, int y)
    {
        UI_OmokPosition position = _positions[x][y];
        position._positionButton.enabled = false;
        position._positionImage.color = new Color(255, 255, 255, 128);
    }
}
