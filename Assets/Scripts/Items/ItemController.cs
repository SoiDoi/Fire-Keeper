#if UNITY_EDITOR
using UnityEditorInternal.Profiling.Memory.Experimental;
#endif
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [Header("Items")]
    public ItemSO itemHolder;
    private Animator ItemsAnimator;
    private bool facingLeft;
    [SerializeField] private CapsuleCollider2D attackRange;

    [Header("Another Object")]
    public ItemType ItemType;
    public GameObject PlayerManager;
    [SerializeField] private SpriteRenderer ItemRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ItemsAnimator = GetComponent<Animator>();
        PlayerManager = GameObject.FindWithTag("Player");
        ItemRenderer = GetComponentInChildren<SpriteRenderer>();
        attackRange = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isGameOver) { return; }
        CheckFacing();///error

    }

    public void CheckFacing()
    {
        facingLeft = PlayerManager.GetComponent<Player>().facingLeft;
        ItemsAnimator.SetBool("facingLeft", facingLeft);
    }

    public void LeftClick()
    {
        if (ItemRenderer.sprite == null) { return; }
        ItemAttack();
    }
    public void RightClick()
    {
        if(ItemRenderer.sprite == null) { return; }
        if (ItemType == ItemType.Item)
        {
            ItemUsing();
        }
    }

    void ItemAttack()
    {
        ItemsAnimator.SetTrigger("IsAttack");
        AudioManager.instance.PlaySFX(2);
        attackRange.enabled = true;
        if (facingLeft)
        {
            ItemRenderer.sortingOrder = 1;
        }
        else
        {
            ItemRenderer.sortingOrder = -1;
        }
        Invoke("OffAttackRange", 0.2f);
        Invoke("ItemGetBack", 3f);
    }

    public void OffAttackRange()
    {
        attackRange.enabled = false;
    }

    void ItemUsing()
    {
        ItemsAnimator.SetTrigger("isUsing");
        AudioManager.instance.PlaySFX(4);
        ItemRenderer.sortingOrder = 1;
        if (ItemType == ItemType.Item)
        {
            float heal = InventoryManager.instance.UsingItem();
            PlayerManager.GetComponent<Player>().Eating(heal, heal);
        }
        Invoke("ItemGetBack", 3f);
    }

    void ItemGetBack()
    {
        ItemsAnimator.SetTrigger("IsNotAttack");
        ItemRenderer.sortingOrder = -1;
    }

    public void ResetItem()
    {
        ItemRenderer.sprite = null;
        itemHolder = null;
    }
    public void SetItem(ItemType type,Sprite image, ItemSO item)
    {
        itemHolder = item;
        ItemRenderer.sprite = image;
        ItemType = type;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                float damage = InventoryManager.instance.ItemAattack();
                enemy.TakeDamage(PlayerManager.GetComponent<Player>().attackDamage + damage);
            }
        }
        if (collision.gameObject.CompareTag("Fire"))
        {
            if (ItemType == ItemType.Material)
            {
                float fuel = InventoryManager.instance.UsingItem();
                BonfireController fire = collision.gameObject.GetComponentInParent<BonfireController>();
                if (fire != null)
                {
                    fire.AddFuel(200);
                }
            }
            
        }
    }
}
