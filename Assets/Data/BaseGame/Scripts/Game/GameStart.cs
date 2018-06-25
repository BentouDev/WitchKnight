using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;

public class GameStart : GameState
{
    protected override void OnStart()
    {
        MainGame.Instance.GUI.HideAllUIs();
        MainGame.Instance.GUI.PlayUnfade();
        MainGame.Instance.Controllers.Init();
        
        MainGame.Instance.SwitchState<GamePlay>();
    }
}
