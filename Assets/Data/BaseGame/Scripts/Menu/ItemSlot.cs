using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image           Icon;
    public CustomButton    Slot;
    public Item            Item;
    public TextMeshProUGUI Count;

    public struct Cell
    {
        public int X;
        public int Y;

        public void Set(int x, int y)
        {
            X = x;
            Y = y;
        }
    }

    [HideInInspector]
    public Cell Pos;
    
    [HideInInspector]
    public UnityAction SelectAction;
    
    [HideInInspector]
    public UnityAction<AxisEventData> NavigateAction;
}