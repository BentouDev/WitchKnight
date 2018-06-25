using System.Collections;
using System.Collections.Generic;
using Framework;
using Framework.Base.Gameplay;
using TMPro;
using UnityEngine;

public class ActivatorUI : IGUIActivateable
{
	public TextMeshProUGUI Text;
	
	public override void OnActivatorSelected(ActionActivator activator)
	{
		Text.text = activator ? activator.Name : string.Empty;
	}
}
