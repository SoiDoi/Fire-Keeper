#if UNITY_EDITOR
using UnityEditorInternal.Profiling.Memory.Experimental;
#endif
using UnityEngine;
using UnityEngine.UI;

public class ItemDropManager : MonoBehaviour
{
    public ItemSO itemSO;
    public int currentStack;
    public int currentDurbility;

    public SpriteRenderer spriteRenderer;
    public CircleCollider2D circleCollider;

    public Transform playerPosition;

    private void Awake()
    {
        playerPosition = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        if (playerPosition != null)
        {
            CheckLayer();
        }
        
    }

    public void DropItem(ItemSO item, int itemQuantity, int itemDurability)
    {
        this.itemSO = item;
        switch (item.itemType)
        {
            case ItemType.Weapon:
            case ItemType.Armor:
            case ItemType.Backpack:
                spriteRenderer.sprite = itemDurability == 0 ? item.itemBrokenImage : item.itemImage;
                this.currentDurbility = itemDurability; break;
            case ItemType.Item:
            case ItemType.Contruction:
            case ItemType.Material:
                this.currentStack = itemQuantity;
                spriteRenderer.sprite = item.itemImage;
                break;
        }
    }
    private void CheckLayer()
    {
        if (transform.position.y + 0.1f > playerPosition.position.y)
        {
            spriteRenderer.sortingLayerName = "ItemsBehindLayer";
        }
        else
        {
            spriteRenderer.sortingLayerName = "ItemsFrontLayer";
        }
    }

}
