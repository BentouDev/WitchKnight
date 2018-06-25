using System.Collections;
using System.Collections.Generic;
using Framework.Base.Gameplay;
using UnityEngine;

[RequireComponent(typeof(ActionActivator))]
public class ItemHolder : MonoBehaviour
{
    private ActionActivator Activator;
    public  Item            Item;
    public  GameObject      ToDestroy;

    void Start()
    {
        this.TryInit(ref Activator);
        
        Activator.Action.AddListener(OnGiveItem);
        if (string.IsNullOrWhiteSpace(Activator.Name))
            Activator.Name = Item.Name;
    }

    protected void OnGiveItem()
    {
        var inv = Activator.CurrentActivateable.GetComponent<Inventory>();
        if (inv) 
            GiveItem(inv);
        else 
            Activator.Enabled = true;
    }

    public void GiveItem(Inventory inventory)
    {
        inventory.AddItem(Item);
        
        Destroy(ToDestroy != null ? ToDestroy : gameObject);
    }
}
