using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitState : MenuBase
{
    public float ExitDelay;

    private bool DidExit = false;

    public void Quit()
    {
        if (DidExit) return;
        
        DidExit = true;
        Controller.StartCoroutine(CoWorker());
    }
    
    IEnumerator CoWorker()
    {
        yield return new WaitForSeconds(ExitDelay);

        MainGame.GlobalQuitGame();
    }
}
