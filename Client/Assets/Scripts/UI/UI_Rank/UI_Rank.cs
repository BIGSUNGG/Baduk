using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Rank : MonoBehaviour
{
    [SerializeField]
    GameObject _rankTextGo;
    Text _rankText;

    [SerializeField]
    GameObject _nameTextGo;    
    Text _nameText;

    [SerializeField]
    GameObject _scoreTextGo;
    Text _scoreText;

    protected virtual void Awake()
    {
        _rankText = _rankTextGo.GetComponent<Text>();
        _nameText = _nameTextGo.GetComponent<Text>();
        _scoreText = _scoreTextGo.GetComponent<Text>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        
    }

    public void Init(int rank, string name, int score)
    {
        _rankText.text = $"Rank {rank}";
        _nameText.text = $"{name}";
        _scoreText.text = $"Score {score}";
    }
}
