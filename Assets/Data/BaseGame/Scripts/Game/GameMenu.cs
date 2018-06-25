using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;

public class GameMenu : GameState
{
    private MenuBase CurrentMenu;
    
    public float AnimTime = 1;

    public delegate void OnAnimEnded();

    public void CloseMenu()
    {
        MainGame.Instance.SwitchState(MainGame.Instance.PreviousState);
    }

    public void SwitchToMenu(MenuBase menu)
    {
        MainGame.Instance.SwitchState(this);
        CurrentMenu = menu;
    }

    protected override void OnStart()
    {
        MainGame.Instance.GUI.PlayFade();
        StartCoroutine(CoWorker(() => CurrentMenu.Controller.AnimShow(CurrentMenu)));
    }

    protected override void OnEnd()
    {
        CurrentMenu.Controller.AnimHide();
        StartCoroutine(CoWorker(() => MainGame.Instance.GUI.PlayUnfade()));
    }
    
    IEnumerator CoWorker(OnAnimEnded callback)
    {
        yield return new WaitForSecondsRealtime(AnimTime);
        callback.Invoke();
    }
}
