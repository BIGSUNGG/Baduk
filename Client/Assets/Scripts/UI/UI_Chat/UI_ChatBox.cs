using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Network;
using UnityEngine.EventSystems;

public class UI_ChatBox : MonoBehaviour
{
    public GameObject _chatPanel;
    public GameObject _inputObject;
    private InputField _inputField;
    public GameObject _textPrefab;

    Queue<GameObject> _texts = new Queue<GameObject>();

    [SerializeField]
    private int _maxTextCount = 10;

    protected virtual void Start()
    {
        _inputField = _inputObject.GetComponent<InputField>();
        _inputField.onEndEdit.AddListener(OnPressSendButton);
    }

    protected virtual void Update()
    {
        if (!_inputField.isFocused && Input.GetKeyDown(KeyCode.Return))
        {
            // 엔터키 누를 시 포커스 설정
            EventSystem.current.SetSelectedGameObject(_inputField.gameObject, null);
            _inputField.OnPointerClick(new PointerEventData(EventSystem.current));
        }
    }

    public void OnPressSendButton(string input)
    {
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && _inputField.text != "")
        {
            // 엔터키 누를 시 채팅 전송
            PushMessage(Managers.Network.Name, input);            
            SendMessageServer(input);
            _inputField.text = "";
        }
    }

    /// <summary>
    /// 서버에게 메시지 전송
    /// </summary>
    /// <param name="message">전송할 메시지</param>
    private void SendMessageServer(string message)
    {
        C_ChatPacket c_Chat = new C_ChatPacket();
        c_Chat.Message = message;
        Managers.Network.Send(c_Chat);
    }

    /// <summary>
    /// 채팅창에 메시지 추가
    /// </summary>
    /// <param name="sender">보낸 사람 이름</param>
    /// <param name="message">추가할 메시지</param>
    public void PushMessage(string sender, string message)
    {
        if(_texts.Count >= _maxTextCount)
        {
            // 메시지의 개수가 많다면 가장 오래된 메시지 제거
            GameObject remove = _texts.Dequeue();
            GameObject.Destroy(remove, 0.0f);
        }

        GameObject newText = Instantiate(_textPrefab, _chatPanel.transform);
        newText.GetComponent<Text>().text = $" {sender} : {message}";

        _texts.Enqueue(newText);
    }
}
