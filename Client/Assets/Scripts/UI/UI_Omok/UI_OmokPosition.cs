using Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UI_OmokPosition : MonoBehaviour
{
    public UnityEvent OnClick = new UnityEvent();

    [HideInInspector]
    Button _positionButton;

    [HideInInspector]
    Image _positionImage;

    protected virtual void Awake()
    {
        _positionButton = GetComponent<Button>();
        _positionImage = GetComponent<Image>();

        _positionButton.onClick.AddListener(OnClickButton);
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }

    private void OnClickButton()
    {
        OnClick.Invoke();
    }

    public void PlaceStone(StoneType stoneType)
    {
        _positionButton.enabled = false;
        _positionImage.color = (stoneType == StoneType.Black ? new Color(0, 0, 0, 255) : new Color(255, 255, 255, 255));
    }
}
