using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.InputSystem;
#if UNITY_EDITOR
using UnityEditorInternal.Profiling.Memory.Experimental;
#endif
using TMPro;

public class ItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    public ItemSO item;
    
    private bool isEmpty => item==null;

    [Header("UI")]
    public Image itemImage;
    public int currentStack;
    public int currentDurability;
    public int publicCurrentStack => currentStack;
    public int publicCurrentDurability => currentDurability;

    [Header("Child gameObject")]
    [Header("Item Quantity")]
    [SerializeField] private Image textBackground;
    [SerializeField] private TMP_Text quantityTxt;
    [Header("Item Durability")]
    [SerializeField] private Image itemDurabilityBar;
    [SerializeField] private RectTransform itemDurabilityLineBar;

    [HideInInspector] public Transform parentAfterDrag;

    public void InitialiseItem(ItemSO newItem,int itemDurability, int itemCount)
    {
        this.item = newItem;
        switch (newItem.itemType)
        {
            case ItemType.Weapon:
            case ItemType.Armor:
            case ItemType.Backpack:
                SetDurability(newItem, itemDurability); break;
            case ItemType.Item:
            case ItemType.Contruction:
            case ItemType.Material:
                SetStack(newItem, itemCount);
                break;
        }
    }
    public void SetDurability(ItemSO item,int itemDurability)
    {
        
        this.itemDurabilityBar.gameObject.SetActive(true);
        this.currentDurability = itemDurability;
        Vector2 percent = itemDurabilityLineBar.sizeDelta;
        percent.x = 50 * ((float)this.currentDurability / item.itemMaximumDurability);
        this.itemDurabilityLineBar.sizeDelta = percent;
        if (currentDurability > 0)
        {
            itemImage.sprite = item.itemImage;
        }
        else
        {
            itemImage.sprite = item.itemBrokenImage;
        }
    }
    public void SetStack(ItemSO newitem, int itemCount)
    {
        itemImage.sprite = newitem.itemImage;
        this.textBackground.gameObject.SetActive(true);
        RefreshCount(itemCount);
    }

    public void RefreshCount(int itemCount)
    {
        this.quantityTxt.text = itemCount.ToString();
        this.currentStack = itemCount;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(isEmpty) return;
        itemImage.raycastTarget = false;
        parentAfterDrag = transform.parent;///
        transform.SetParent(transform.root);///
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Mouse.current.position.ReadValue();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemImage.raycastTarget = true;
        transform.SetParent(parentAfterDrag);//
    }
}

