using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Serialization.Formatters;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityScript.Steps;

public class InventoryGUI : MonoBehaviour
{
    public Inventory Inventory;
    public string InventoryTag;
    public TextMeshProUGUI ItemName;
    public string EmptyItemName = "0xNull";
    public TextMeshProUGUI ItemDescription;
    public string EmptyDescription = "0xNull";

    public List<List<ItemSlot>> Grid;

    private int Width  { get; set; }
    private int Height { get; set; }
    private int LastY  { get; set; }
    private int LastX  { get; set; }
    private int MaxRow => Mathf.CeilToInt(Inventory.Items.Count / (float) Width);

    public struct ItemSelectionInfo
    {
        public ItemSlot Slot { get; internal set; }

        internal int Height;
        internal int Upper;
        internal int Lower => Upper + Height;
    }

    [HideInInspector]
    public ItemSelectionInfo Selected;

    void Start()
    {
        if (!Inventory)
        {
            var go = GameObject.FindGameObjectWithTag(InventoryTag);
            if (go)
                Inventory = go.GetComponent<Inventory>();
        }
    }
    
    public void BuildGrid()
    {
        if (Grid != null)
            Grid.Clear();
        else 
            Grid = new List<List<ItemSlot>>();

        foreach (var row in GetComponentsInChildren<InventoryRow>())
        {
            var row_items = new List<ItemSlot>(row.ItemSlots);
            Grid.Add(row_items);
        }

        BuildNavigation();
    }

    void OnSelectionChanged(ItemSlot slot)
    {
        Selected.Slot = slot;
        
        if (slot && slot.Item)
        {
            ItemName.text        = slot.Item.name;
            ItemDescription.text = slot.Item.Description;
        }
        else
        {
            ItemName.text        = EmptyItemName;
            ItemDescription.text = EmptyDescription;
        }
    }

    private void BuildNavigation()
    {
        LastY           = Grid.Count - 1;
        Height          = Grid.Count;
        Selected.Height = Height;

        for (int y = 0; y < Height; y++)
        {
            LastX = Grid[y].Count - 1;
            Width = Grid[y].Count;
            
            for (int x = 0; x < Width; x++)
            {
                var item = Grid[y][x];
                var slot = Grid[y][x].Slot;
                
                item.Pos.Set(x, y);
                
                slot.navigation = new Navigation()
                {
                    mode = Navigation.Mode.Explicit,
                    selectOnLeft  = GetNavigationTarget(x, y, MoveDirection.Left),
                    selectOnRight = GetNavigationTarget(x, y, MoveDirection.Right),
                    selectOnUp    = GetNavigationTarget(x, y, MoveDirection.Up),
                    selectOnDown  = GetNavigationTarget(x, y, MoveDirection.Down),
                };
                
                if (item.SelectAction != null)
                    slot.OnSelected.RemoveListener(item.SelectAction);
                
                item.SelectAction = () => OnSelectionChanged(item);
                slot.OnSelected.AddListener(Grid[y][x].SelectAction);

                // Override on edges
                if (y == 0 || y == LastY 
                ||  x == 0 || y == LastX)
                {
                    slot.OverrideNavigation = true;
                    
                    if (item.NavigateAction != null)
                        slot.OnNavigate.RemoveListener(item.NavigateAction);

                    item.NavigateAction = HandleMovementDirection;
                    slot.OnNavigate.AddListener(item.NavigateAction);
                }
            }
        }

        ReloadItems();
    }

//    protected UnityAction<AxisEventData> CreateSwapLambda(ItemSlot slot, int x, int y)
//    {
//        return (data) =>
//        {
//            // HandleMovementDirection(data, slot, x, y);
//            bool selectRow = true;
//            if (y == 0 && data.moveDir == MoveDirection.Up)
//            {
//                selectRow = PreviousRow(x);
//            }
//            else if (y == LastY && data.moveDir == MoveDirection.Down)
//            {
//                selectRow = NextRow(x);
//            }
//            
//            if (selectRow)
//                slot.Slot.DoMove(data);
//        };
//    }

    private bool NextRow()
    {
        bool result = false;
        if (Selected.Lower + 1 <= MaxRow)
        {
            // Go down
            Selected.Upper++;
        }
        else
        {
            // Change selection to uppermost row
            Selected.Upper = 0;
            result = true;
        }

        ReloadItems(Selected.Upper * Width);
        
        return result;
    }

    private bool PreviousRow()
    {
        bool result = false;
        if (Selected.Upper - 1 >= 0)
        {
            // Go up
            Selected.Upper--;
        }
        else
        {
            // Change selection to lowermost row
            Selected.Upper = Mathf.Max(0, MaxRow - Height);
            result = true;
        }

        ReloadItems(Selected.Upper * Width);
        
        return result;
    }

    public void ReloadItems(int startingIndex = 0)
    {
        int itemIndex = startingIndex;
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                if (itemIndex < Inventory.Items.Count)
                {
                    SetupSlot(Grid[y][x], Inventory.Items[itemIndex]);
                    itemIndex++;
                }
                else
                {
                    ClearSlot(Grid[y][x]);
                }
            }
        }
    }

    private void SetupSlot(ItemSlot slot, Inventory.ItemInfo item)
    {
        slot.Count.text   = item.Count.ToString();
        slot.Item         = item.Item;
        slot.Icon.sprite  = item.Item.Icon;
        slot.Icon.color   = Color.white;
        slot.Slot.enabled = true;
    }

    private void ClearSlot(ItemSlot slot)
    {
        slot.Count.text   = string.Empty;
        slot.Item         = null;
        slot.Icon.sprite  = null;
        slot.Icon.color   = Color.clear;
        slot.Slot.enabled = false;
    }

    public void SelectSlot(int x, int y)
    {
        SelectSlot(Grid[y][x]);
    }

    public void SelectSlot(ItemSlot slot)
    {
        slot.Slot.Select();
        slot.Slot.OnSelected.Invoke();
    }

    private void HandleMovementDirection(AxisEventData data)
    {
        CustomButton overrideSelection = null;
        bool         performNavigation = true;
        switch (data.moveDir)
        {
            case MoveDirection.Left:
            {
                if (Selected.Slot.Pos.Y == 0 && Selected.Slot.Pos.X == 0)
                {
                    if (PreviousRow())
                        overrideSelection = Grid[LastY][LastX].Slot;
                    else
                        overrideSelection = Grid[0][LastX].Slot;
                }
                break;
            }
            case MoveDirection.Right:
            {
                if (Selected.Slot.Pos.Y == LastY && Selected.Slot.Pos.X == LastX)
                {
                    if (NextRow())
                        overrideSelection = Grid[0][0].Slot;
                    else
                        overrideSelection = Grid[LastY][0].Slot;
                }
                break;
            }
            case MoveDirection.Up:
            {
                if (Selected.Slot.Pos.Y == 0 && data.moveDir == MoveDirection.Up)
                {
                    performNavigation = PreviousRow();
                }
                break;
            }
            case MoveDirection.Down:
            {
                if (Selected.Slot.Pos.Y == LastY && data.moveDir == MoveDirection.Down)
                {
                    performNavigation = NextRow();
                }
                break;
            }
        }
        
        // Change selection, we moved from edge
        if (performNavigation)
        {
            // Check if slot we want to move to is enabled
            if (overrideSelection && (UnityEngine.Object) overrideSelection != (UnityEngine.Object) null)
            {
                // If it's not enabled, recursively find first enabled slot on left (there is always at least one)
                if (!overrideSelection.IsActive())
                    data.selectedObject = overrideSelection.GetNavigationTarget(MoveDirection.Left);
                else
                    data.selectedObject = overrideSelection.gameObject;
            }
            else
                Selected.Slot.Slot.DoMove(data);
        }
        // No selection change, but slot can be disabled now
        else if (!Selected.Slot.Slot.enabled)
        {
            // recursively find first enabled slot on left (there is always at least one)
            data.selectedObject = Selected.Slot.Slot.GetNavigationTarget(MoveDirection.Left);
        }
    }
    
    private Selectable GetNavigationTarget(int x, int y, MoveDirection dir)
    {
        Selectable result = null;
        switch (dir)
        {
            case MoveDirection.Left:
            {
                // Change row on leftmost
                if (x == 0)
                {
                    int row    = y == 0 ? LastY : y - 1;
                    result = Grid[row][LastX].Slot;
                }
                else
                {
                    result = Grid[y][x - 1].Slot;
                }
                break;
            }
            case MoveDirection.Right:
            {
                // Change row on rightmost
                if (x == LastX)
                {
                    int row    = y == LastY ? 0 : y + 1;
                    result = Grid[row][0].Slot;
                }
                else
                {
                    result = Grid[y][x + 1].Slot;
                }
                break;
            }
            case MoveDirection.Up:
            {
                // Change row on topmost
                int row    = y == 0 ? LastY : y - 1;
                result = Grid[row][x].Slot;
                
                break;
            }
            case MoveDirection.Down:
            {
                // Change row on last row
                int row    = y == LastY ? 0 : y + 1;
                result = Grid[row][x].Slot;
                break;
            }
        }

        return result;
    }
}