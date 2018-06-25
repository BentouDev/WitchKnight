using UnityEngine;
using Framework;

public class GameDialog : DialogGameState
{
    protected override void OnStart()
    {
        base.OnStart();

        if (!MainGame.Instance.GUI.IsCinematicShown())
        {
            MainGame.Instance.GUI.HideAllUIs();
            MainGame.Instance.GUI.ShowCinematicsWithAnim();
        }

        MainGame.Instance.Controllers.Disable();
    }

    protected override void ReturnToPreviousState()
    {
        MainGame.Instance.SwitchState(MainGame.Instance.PreviousState);
    }

    protected override void OnEnd()
    {
        base.OnEnd();

        MainGame.Instance.GUI.HideCinematicsWithAnim();
        MainGame.Instance.Controllers.Enable();
    }
}