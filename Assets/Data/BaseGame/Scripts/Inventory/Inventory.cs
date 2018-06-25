using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [System.Serializable]
    public class ItemInfo
    {
        [SerializeField] public Item Item;
        
        [SerializeField] public int Count;
    }

    [SerializeField]
    public List<ItemInfo> Items = new List<ItemInfo>();

    public void AddItem(Item item)
    {
        if (item == null)
            return;
        
        var itr = Items.FirstOrDefault(x => x.Item == item);
        if (itr != null && itr.Item != null)
        {
            itr.Count++;
        }
        else
        {
            Items.Add(new ItemInfo {
                Item  = item, 
                Count = 1
            });
        }
    }

    public void RemoveItem(Item item)
    {
        var itr = Items.FirstOrDefault(x => x.Item == item);
        if (itr.Item != null)
        {
            if (itr.Count == 1)
            {
                Items.Remove(itr);
            }
            else
            {
                itr.Count--;
            }
        }
    }
}
