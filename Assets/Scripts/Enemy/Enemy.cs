using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    public float damage = 10f;
    public float HP = 20f;
    public bool facingLeft;
    public SpriteRenderer enemyRenderer;
    public Animator animator;

    public ItemSO orange;
    public ItemSO ironSword;
    public ItemSO wood;

    private Transform fireTarget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fireTarget = GameObject.FindGameObjectWithTag("Fire").transform;
        enemyRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fireTarget == null) return;

        Vector3 direction = (fireTarget.position - transform.position).normalized;
        facingLeft = transform.position.x > direction.x ? true : false;
        transform.position += direction * speed * Time.deltaTime;
        if (facingLeft) { enemyRenderer.flipX = true; }
        else { enemyRenderer.flipX = false; }
    }

    public void TakeDamage(float damage)
    {
        if (HP > damage)
        {
            HP -= damage;
            animator.SetTrigger("isAttacked");
        }
        else
        {
            RandomDrop();
            GameManager.instance.enemyKilled++;
            Destroy(gameObject);
        }
    }

    void RandomDrop()
    {
        ItemSO itemDrop;
        int quantity = 0;
        int durability = 0;
        int random = Random.Range(0, 20);
        if (random == 0)
        {
            return;
        }
        else if (random == 20)
        {
            itemDrop = ironSword;
            durability = Random.Range(10, ironSword.itemMaximumDurability);
        }
        else if (random >= 12)
        {
            itemDrop = wood;
            quantity = Random.Range(1, 5);
        }
        else
        {
            itemDrop = orange;
            quantity = Random.Range(1, 2);
        }
        InventoryManager.instance.DropItem(itemDrop,transform.position, quantity, durability);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Fire"))
        {
            BonfireController fire = collision.gameObject.GetComponent<BonfireController>();
            if (fire != null)
            {
                fire.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(damage);
                AudioManager.instance.PlaySFX(3);
            }
            Destroy(gameObject);
        }
    }
}
