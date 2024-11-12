using Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Timer;

public class Managers : MonoBehaviour
{
    const string ManagerObjectName = "@Managers";

    // Instance
    public static Managers Instance { get { Init(); return s_instance; } }
    static Managers s_instance;

    public static TimerManager Timer => Instance._timer;
    public static NetworkManager Network => Instance._network;

    TimerManager _timer = new TimerManager();
    NetworkManager _network = new NetworkManager();

    static void Init()
    {
        if (s_instance != null)
            return;

        GameObject go = GameObject.Find(ManagerObjectName);
        if(go == null)
        {
            go = new GameObject(ManagerObjectName);
            DontDestroyOnLoad(go);
        }

        s_instance = go.GetOrAddComponent<Managers>();
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
