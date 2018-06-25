using System.Collections.Generic;
using UnityEngine;

public class InventoryRow : MonoBehaviour
{
    private List<ItemSlot> _slots;
    public List<ItemSlot> ItemSlots => _slots ?? (_slots = new List<ItemSlot>(GetComponentsInChildren<ItemSlot>()));
}