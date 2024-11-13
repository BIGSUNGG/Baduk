using Network;
using UnityEngine;

public class UI_OmokScene : UI_Scene
{
    [SerializeField]
    GameObject _gameWin;
    [SerializeField]
    GameObject _gameLose;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public void OnFinishGame(StoneType winner)
    {
        if (Managers.Network.MyStone == winner)
            Instantiate(_gameWin, transform);
        else
            Instantiate(_gameLose, transform);
    }
}
