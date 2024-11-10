using Network;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UI_LobbyScene : UI_Scene
{
    public GameObject _logInBtn;
    public GameObject _signUpBtn;
                      
    public GameObject _nameField;
    public GameObject _passwordField;

    protected override void Start()
    {
        base.Start();

        _logInBtn.GetComponent<Button>().onClick.AddListener(OnClickLogInBtn);
        _signUpBtn.GetComponent<Button>().onClick.AddListener(OnClickSignInBtn);
    }

    protected override void Update()
    {
        base.Update();
    }

    private void OnClickLogInBtn()
    {
        Debug.Log("On Click LogInWay Button");  

        Managers.Network.Connect();
        StartCoroutine(WaitingConnectServer(LogInSend));       
    }

    private void LogInSend()
    {
        InputField name = _nameField.GetComponent<InputField>();
        InputField password = _passwordField.GetComponent<InputField>();

        C_LogInPacket loginPacket = new C_LogInPacket();
        loginPacket.Name = name.text;
        loginPacket.Password = password.text;
        Managers.Network.Send(loginPacket);        

        name.text = "";
        password.text = "";
    }

    private void OnClickSignInBtn()
    {
        Debug.Log("On Click SignInWay Button");

        Managers.Network.Connect();
        StartCoroutine(WaitingConnectServer(SignUpSend));
    }

    private void SignUpSend()
    {
        InputField name = _nameField.GetComponent<InputField>();
        InputField password = _passwordField.GetComponent<InputField>();

        C_SignUpPacket singupPacket = new C_SignUpPacket();
        singupPacket.Name = name.text;
        singupPacket.Password = password.text;
        Managers.Network.Send(singupPacket);

        name.text = "";
        password.text = "";
    }

    private IEnumerator WaitingConnectServer(Action action)
    {
        if (Managers.Network.IsConnect == false)
            yield return null;

        action.Invoke();
        yield break;
    }
}
