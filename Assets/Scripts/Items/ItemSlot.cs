using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public Image borderImage;
    public ItemSlotType itemSlotType;


    public void Deselect()
    {
        borderImage.enabled = false;
    }
    public void Select()
    {
        borderImage.enabled = true;
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.GetChild(0).childCount == 0)
        {
            ItemUI itemUI = eventData.pointerDrag.GetComponent<ItemUI>();
            if (itemUI == null) { return; }
            itemUI.parentAfterDrag = transform.GetChild(0);
        }
        else
        {
            ItemUI itemUI = eventData.pointerDrag.GetComponent<ItemUI>();
            ItemUI targetItem = transform.GetChild(0).GetComponentInChildren<ItemUI>();

            
            if (itemUI == null ||targetItem == null) { return;  }
            // Check Item can stack
            if (itemUI.item == targetItem.item && itemUI.item.itemMaxStackSize > 0)
            {
                int maxStack = targetItem.item.itemMaxStackSize;
                int total = itemUI.publicCurrentStack + targetItem.publicCurrentStack;

                if (total <= maxStack)
                {
                    targetItem.RefreshCount(total);
                    Destroy(itemUI.gameObject);
                }
                else
                {
                    targetItem.RefreshCount(maxStack);
                    itemUI.RefreshCount(total - maxStack);
                }
            }
            else
            {
                // Swap Item
                Transform draggedParent = itemUI.parentAfterDrag;
                itemUI.parentAfterDrag = targetItem.transform.parent;
                targetItem.transform.SetParent(draggedParent);
            }

        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        switch (this.itemSlotType)
        {
            case ItemSlotType.Inventory:
                for (int i = 0; i < InventoryManager.instance.inventorySlot.Length; i++)
                {
                    if (InventoryManager.instance.inventorySlot[i] == this)
                    {
                        InventoryManager.instance.ChangeInventorySelectedSlot(i);
                        break;
                    }
                }
                break;

            case ItemSlotType.ToolBar:
                for (int i = 0; i < InventoryManager.instance.itemBarSlot.Length; i++)
                {
                    if (InventoryManager.instance.itemBarSlot[i] == this)
                    {
                        InventoryManager.instance.ChangeToolBarSelectedSlot(i);
                        break;
                    }
                }
                break;
        }
        if (transform.GetChild(0).childCount == 0) return;
        ItemUI itemUI = transform.GetChild(0).GetChild(0).GetComponent<ItemUI>();
        if (itemUI == null) return;
        
                InventoryManager.instance.SetDescription(itemUI.item);

    }
}
public enum ItemSlotType
{
    Inventory,
    ToolBar,
    ArmorSlot,
    BackPackSlot
}
