using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_OmokPosition : MonoBehaviour
{
    [HideInInspector]
    public Button _positionButton;

    [HideInInspector]
    public Image _positionImage;

    protected virtual void Awake()
    {
        _positionButton = GetComponent<Button>();
        _positionImage = GetComponent<Image>();
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }
}
