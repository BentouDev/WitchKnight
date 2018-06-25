using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;

public class MainGame : Game<MainGame>
{
	public GUIController GUI;
	
	public override bool IsPlaying()
	{
		return CurrentState is GamePlay;
	}
}
