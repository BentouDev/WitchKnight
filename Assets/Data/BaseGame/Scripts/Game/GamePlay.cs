using System.Collections;
using System.Collections.Generic;
using Framework;
using UnityEngine;
using UnityEngine.EventSystems;

public class GamePlay : GameStart
{
	public GameMenu Menu;
	public IGUIActivateable ActivatorGUI;
	
	[System.Serializable]
	public struct MenuInfo
	{
		[SerializeField] public string   Button;
		[SerializeField] public MenuBase Menu;
	}

	public List<MenuInfo> Menus = new List<MenuInfo>();

	protected override void OnStart()
	{
		MainGame.Instance.Controllers.Enable();
		MainGame.Instance.GUI.ShowAllUIs();
		
		if (EventSystem.current && ActivatorGUI)
			EventSystem.current.SetSelectedGameObject(ActivatorGUI.gameObject);
	}

	protected override void OnEnd()
	{
		MainGame.Instance.Controllers.Disable();
		
		if (EventSystem.current.currentSelectedGameObject == ActivatorGUI.gameObject)
			EventSystem.current.SetSelectedGameObject(null);
	}

	protected override void OnTick()
	{
		foreach (MenuInfo info in Menus)
		{
			if (Input.GetButtonDown(info.Button))
				Menu.SwitchToMenu(info.Menu);
		}
	}
}
