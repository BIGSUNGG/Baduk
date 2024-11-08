using Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Timer;

public class Managers : MonoBehaviour
{
    const string ManagerObjectName = "@Managers";

    // Instance
    public static Managers Instance { get { Init(); return _instance; } }
    static Managers _instance;

    public static TimerManager Timer => Instance._timer;
    public static NetworkManager Network => Instance._network;

    TimerManager _timer = new TimerManager();
    NetworkManager _network = new NetworkManager();

    static void Init()
    {
        if (_instance != null)
            return;

        GameObject go = GameObject.Find(ManagerObjectName);
        if(go == null)
        {
            go = new GameObject(ManagerObjectName);
            DontDestroyOnLoad(go);
        }

        _instance = go.GetOrAddComponent<Managers>();
    }

    protected void Start()
    {
        
    }

    protected void Update()
    {
        _timer.Update();
        _network.Update();
    }
}
