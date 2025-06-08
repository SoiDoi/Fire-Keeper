using System;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D;
using UnityEngine.UI;
#if UNITY_EDITOR
using static UnityEditor.Progress;
#endif

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public ItemController itemController;

    [Header("Slot")]
    public ItemSlot[] inventorySlot;
    public ItemSlot[] itemBarSlot = new ItemSlot[9];
    public ItemSlot[] itemEquiptmenSlot = new ItemSlot[2];
    public int itemUISelected = -1;
    int toolBarSelectedSlot=0;

    [Header("InventorySlots")]
    [SerializeField] public GameObject inventory;
    [SerializeField] private ItemSlot itemSlotPrefab;
    [SerializeField] private int inventorySize;
    [Header("ToolBarSlots")]
    [SerializeField] private RectTransform inventoryPanel;
    [SerializeField] private RectTransform toolBarPanel;
    [SerializeField] private GameObject inventoryButton;
    [Header("EquiptmentSlots")]
    [SerializeField] private RectTransform equiptmenPanel;
    [Header("ItemUI Prefab")]
    [SerializeField] private GameObject itemPrefab;
    [Header("Item Description")]
    [SerializeField] private ItemDescription itemDescription;
    [Header("Item Drop")]
    [SerializeField] private GameObject ItemDrop;
    [SerializeField] private GameObject ItemDropParent;

    //Test
    [SerializeField] ItemSO i;


    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        InitializeInventorySlot(inventorySize);
        InitializeToolBarSlot();
        InitializeEquiptmentSlot();
        itemBarSlot[0].Select();

        //test
        DropItem(i,new Vector3(2,2,0),0,25);
    }

    public void ChangeToolBarSelectedSlot(int newValue)
    {
        if (toolBarSelectedSlot >= 0)
        {
            itemBarSlot[toolBarSelectedSlot].Deselect();
            itemController.ResetItem();
        }
        itemBarSlot[newValue].Select();
        toolBarSelectedSlot = newValue;
        if (itemBarSlot[newValue].GetComponentInChildren<ItemUI>() !=null)
        {
            ItemSO item = itemBarSlot[newValue].GetComponentInChildren<ItemUI>().item;
            Sprite sprite = itemBarSlot[newValue].GetComponentInChildren<ItemUI>().itemImage.sprite;
            ItemType type = itemBarSlot[newValue].GetComponentInChildren<ItemUI>().item.itemType;
            itemController.SetItem(type, sprite,item);
        }
    }
    public void ChangeInventorySelectedSlot(int newValue)
    {
        if (itemUISelected >= 0)
        {
            inventorySlot[itemUISelected].Deselect();
        }
        inventorySlot[newValue].Select();
        itemUISelected = newValue;
    }

    public void InitializeInventorySlot(int inventorySize)
    {
        inventorySlot = new ItemSlot[inventorySize];
        for (int i = 0; i < inventorySize; i++)
        {
            itemSlotPrefab.itemSlotType = ItemSlotType.Inventory;
            ItemSlot uIItem = Instantiate(itemSlotPrefab, Vector3.zero, Quaternion.identity);
            uIItem.transform.SetParent(inventoryPanel);
            inventorySlot[i] = uIItem;
        }
    }

    public void InitializeToolBarSlot()
    {
        itemSlotPrefab.itemSlotType = ItemSlotType.ToolBar;
        for (int i = 0; i < itemBarSlot.Length; i++)
        {
            ItemSlot uIItem = Instantiate(itemSlotPrefab, Vector3.zero, Quaternion.identity);
            uIItem.transform.SetParent(toolBarPanel);
            itemBarSlot[i] = uIItem;
        }
        inventoryButton.transform.SetAsLastSibling();
    }
    
    public void InitializeEquiptmentSlot()
    {
        
        for (int i = 0; i < itemEquiptmenSlot.Length; i++)
        {
            if (i == 0) { itemSlotPrefab.itemSlotType = ItemSlotType.ArmorSlot; }
            else { itemSlotPrefab.itemSlotType = ItemSlotType.BackPackSlot; }
            ItemSlot uIItem = Instantiate(itemSlotPrefab, Vector3.zero, Quaternion.identity);
            uIItem.transform.SetParent(equiptmenPanel);
            itemEquiptmenSlot[i] = uIItem;
        }
    }

    public bool AddItem(ItemSO item, int itemDurability, int itemCount)
    {
        //check same item and add stack in inventory
        for (int i = 0; i < inventorySlot.Length; i++)
        {
            ItemSlot slot = inventorySlot[i];
            ItemUI itemInSlot = slot.GetComponentInChildren<ItemUI>();
            if(item.itemMaxStackSize ==0)
            {
                break;
            }
            if (itemInSlot != null && itemInSlot.item == item && 
                (itemInSlot.publicCurrentStack <itemInSlot.item.itemMaxStackSize))
            {
                if (itemInSlot.publicCurrentStack + itemCount < itemInSlot.item.itemMaxStackSize)
                {
                    int count = itemInSlot.publicCurrentStack + itemCount;
                    itemInSlot.RefreshCount(count);
                }
                else
                {
                    itemInSlot.RefreshCount(itemInSlot.item.itemMaxStackSize);
                    int count = (itemInSlot.publicCurrentStack + itemCount) - itemInSlot.item.itemMaxStackSize;
                    AddItem(item, itemDurability, count);
                }   
                return true;
            }
        }
        //check same item and add stack in toolbar
        for (int i = 0; i < itemBarSlot.Length; i++)
        {
            ItemSlot slot = itemBarSlot[i];
            ItemUI itemInSlot = slot.GetComponentInChildren<ItemUI>();
            if (item.itemMaxStackSize == 0)
            {
                break;
            }
            if (itemInSlot != null && itemInSlot.item == item &&
                ((itemInSlot.publicCurrentStack + itemCount) < itemInSlot.item.itemMaxStackSize))
            {
                int count = itemInSlot.publicCurrentStack + itemCount;
                itemInSlot.RefreshCount(count);
                return true;
            }
        }
        // check null slot
        for (int i = 0; i < inventorySlot.Length; i++)
        {
            ItemSlot slot = inventorySlot[i];
            ItemUI itemInSlot = slot.GetComponentInChildren<ItemUI>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, itemDurability, itemCount, slot);
                return true;
            }
        }
        for (int i = 0; i < itemBarSlot.Length; i++)
        {
            ItemSlot slot = itemBarSlot[i];
            ItemUI itemInSlot = slot.GetComponentInChildren<ItemUI>();
            if (itemInSlot == null)
            {
                SpawnNewItem(item, itemDurability,itemCount, slot);
                return true;
            }
        }
        return false;
    }
    void SpawnNewItem(ItemSO item,int itemDurability,int itemCount,ItemSlot slot)
    {
        GameObject newItemObj = Instantiate(itemPrefab, slot.transform.GetChild(0).transform);
        ItemUI inventoryItem = newItemObj.GetComponent<ItemUI>();
        inventoryItem.InitialiseItem(item, itemDurability,itemCount);
    }

    public void SetDescription(ItemSO itemSO)
    {
        itemDescription.SetDescription(itemSO.itemImage, itemSO.itemName, itemSO.itemDescription);
    }

    public void DropItem(ItemSO item,Vector3 spawnLocation, int quantity, int durability)
    {
        GameObject Item = Instantiate(ItemDrop, spawnLocation, Quaternion.identity);
        Item.GetComponent<ItemDropManager>().DropItem(item, quantity, durability);
        Item.transform.SetParent(ItemDropParent.transform);
    }

    public float UsingItem()
    {
        ItemSO item = itemBarSlot[toolBarSelectedSlot].GetComponentInChildren<ItemUI>().item;
        int stack = itemBarSlot[toolBarSelectedSlot].GetComponentInChildren<ItemUI>().currentStack;
        float heal = item.consumableHealingPoint;
        if (stack - 1 > 0)
        {
            stack--;
            itemBarSlot[toolBarSelectedSlot].GetComponentInChildren<ItemUI>().SetStack(item, stack);
        }
        else
        {
            Destroy(itemBarSlot[toolBarSelectedSlot].GetComponentInChildren<ItemUI>().gameObject,1f);
            itemController.ResetItem();
        }
        return heal;
    }

    public float ItemAattack()
    {
        ItemSO item = itemBarSlot[toolBarSelectedSlot].GetComponentInChildren<ItemUI>().item;
        int durability = itemBarSlot[toolBarSelectedSlot].GetComponentInChildren<ItemUI>().currentDurability;
        float damage = durability > 0 ? item.weaponAttackDamage : 1;
        if (durability > 0)
        {
            durability--;
            if (durability <= 0)
            {
                //play sound;
            }
        }
        itemBarSlot[toolBarSelectedSlot].GetComponentInChildren<ItemUI>().SetDurability(item, durability);
        return damage;
    }

    public void ShowInventory()
    {
        inventory.SetActive(true);
        //itemDescription.ResetDescription();
    }
    public void HideInventory()
    {
        inventory.SetActive(false);
    }
}
