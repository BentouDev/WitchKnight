using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeginMenu : MenuBase
{
	public float LoadDelay;
	public int LevelToLoad;
	
	protected override void OnBegin()
	{
		Controller.StartCoroutine(CoWorker());
	}

	IEnumerator CoWorker()
	{
		yield return new WaitForSeconds(LoadDelay);
		yield return SceneManager.LoadSceneAsync(LevelToLoad);
	}
}
