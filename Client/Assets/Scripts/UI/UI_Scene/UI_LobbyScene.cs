using Network;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_LobbyScene : UI_Scene
{
    [SerializeField]
    GameObject _matchButtonGo;
    Button _matchButton;
    [SerializeField]
    GameObject _topRankButtonGo;
    Button _topRankButton;
    [SerializeField]
    GameObject _nearRankButtonGo;
    Button _nearRankButton;

    [SerializeField]
    GameObject _rankGridGo;
    [SerializeField]
    GameObject _rankPrefab;

    protected override void Start()
    {
        base.Start();

        _matchButton = _matchButtonGo.GetComponent<Button>();
        _matchButton.onClick.AddListener(OnClickMatchStartButton);

        _topRankButton = _topRankButtonGo.GetComponent<Button>();
        _topRankButton.onClick.AddListener(OnClickTopRankButton);

        _nearRankButton = _nearRankButtonGo.GetComponent<Button>();
        _nearRankButton.onClick.AddListener(OnClickNearRankButton);
    }

    protected override void Update()
    {
        base.Update();
    }

    public void OnClickMatchStartButton()
    {
        C_StartMatchPacket c_StartMatchPacket = new C_StartMatchPacket();
        Managers.Network.Send(c_StartMatchPacket);

        SceneManager.LoadScene("Match Scene", LoadSceneMode.Single);
    }

    public void OnClickTopRankButton()
    {
        C_RequestTopRankPacket c_RequestTopRankPacket = new C_RequestTopRankPacket();
        c_RequestTopRankPacket.RequestType = RequestTopRankType.TopRank;
        Managers.Network.Send(c_RequestTopRankPacket);
    }

    public void OnClickNearRankButton()
    {
        C_RequestTopRankPacket c_RequestTopRankPacket = new C_RequestTopRankPacket();
        c_RequestTopRankPacket.RequestType = RequestTopRankType.NearRank;
        Managers.Network.Send(c_RequestTopRankPacket);
    }

    public void SetUserInfo(List<UserInfo> users)
    {
        foreach (Transform child in _rankGridGo.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < users.Count; i++)
        {
            GameObject newGo = Instantiate(_rankPrefab, _rankGridGo.transform);
            UI_Rank uiRank = newGo.GetComponent<UI_Rank>();
            uiRank.Init(users[i].Rank, users[i].Name, users[i].Score);
        }
    }
}
