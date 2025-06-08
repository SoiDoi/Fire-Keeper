using UnityEngine;

public class DemoScript : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public ItemSO[] itemToPickUp;

    public void PickUpItem(int id)
    {
        int count = Random.Range(1, 10);
        int itemDurability = Random.Range(0, 100);
        bool result = inventoryManager.AddItem(itemToPickUp[id],itemDurability,count); 
        if (result == true)
        {
            Debug.Log("Item Add");
        }
        else
        {
            Debug.Log("Bag full");
        }
    }

    public void DebugItemLocation()
    {
        string[] Location = new string[30];
        int index = 0;
        for (int i = 0; i < inventoryManager.inventorySlot.Length; i++)
        {
            if (inventoryManager.inventorySlot[i].transform.GetChild(0).childCount > 0)
            {
                Location[index] = "InventorySlot: "+ i.ToString() +" "+ inventoryManager.inventorySlot[i].transform.GetChild(0).transform.GetComponentInChildren<ItemUI>().item.name;
                index++;
            }
        }
        for (int i = 0; i < inventoryManager.itemBarSlot.Length; i++)
        {
            if (inventoryManager.itemBarSlot[i].transform.GetChild(0).childCount > 0)
            {
                Location[index] = "ToolBarSlot: " +i.ToString() + " " + inventoryManager.itemBarSlot[i].transform.GetChild(0).transform.GetComponentInChildren<ItemUI>().item.name;
                index++;
            }
        }
        for (int i = 0; i < inventoryManager.itemEquiptmenSlot.Length; i++)
        {
            if (inventoryManager.itemEquiptmenSlot[i].transform.GetChild(0).childCount > 0)
            {
                Location[index] = "itemEquiptmenSlot: " + i.ToString() + " " + inventoryManager.itemEquiptmenSlot[i].transform.GetChild(0).transform.GetComponentInChildren<ItemUI>().item.name;
                index++;
            }
        }
        for (int i = 0; i < Location.Length; i++)
        {
            Debug.Log(Location[i]);
        }
    }

}
