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
            EventSystem.current.SetSelectedGameObject(_inputField.gameObject, null);
            _inputField.OnPointerClick(new PointerEventData(EventSystem.current)); // 포커스 설정
        }
    }

    public void OnPressSendButton(string input)
    {
        if ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && _inputField.text != "")
        {
            PushMessage(Managers.Network.Name, input);            
            SendMessageServer(input);
            _inputField.text = "";
        }
    }

    private void SendMessageServer(string message)
    {
        C_ChatPacket c_Chat = new C_ChatPacket();
        c_Chat.Message = message;
        Managers.Network.Send(c_Chat);
    }

    public void PushMessage(string sender, string message)
    {
        if(_texts.Count >= _maxTextCount)
        {
            GameObject remove = _texts.Dequeue();
            GameObject.Destroy(remove, 0.0f);
        }

        GameObject newText = Instantiate(_textPrefab, _chatPanel.transform);
        newText.GetComponent<Text>().text = $" {sender} : {message}";

        _texts.Enqueue(newText);
    }
}
