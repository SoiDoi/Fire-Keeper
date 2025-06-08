using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewItem", menuName = "Scriptable Objects/ItemSO")]
public class ItemSO : ScriptableObject
{
    [Header("Both")]
    [SerializeField] string itemID;
    public string itemName;
    public Sprite itemImage;
    public Sprite itemBrokenImage;

    [Header("Only UI")]
    public ItemType itemType;
    public float weaponAttackDamage;
    public float consumableHealingPoint;
    public int itemMaxStackSize;
    public int itemMaximumDurability;
    public string itemDescription;

    [Header("Only Ingame")]
    public GameObject itemPrefab;
    public TileBase itemTile;

  
}
public enum ItemType
{
    Weapon,
    Armor,
    Backpack,
    Item,
    Contruction,
    Material
}
