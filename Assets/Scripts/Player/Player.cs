using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
#if UNITY_EDITOR
using static UnityEditor.Progress;
#endif

public class Player : MonoBehaviour
{
    [Header("PlayerManager")]
    [SerializeField] private CharacterSO characterData;
    public CharacterSO character => characterData;
    [SerializeField] private Rigidbody2D PlayerRB;
    [SerializeField] private Transform PlayerTransform;
    [SerializeField] private PlayerInput PlayerInput;

    [Header("PlayerController")]
    [SerializeField] private SpriteRenderer playerRenderer;
    [SerializeField] private Animator playerAnimator;
    

    [Header("Movement")]
    public Vector2 _moveDirection;
    public InputActionReference move;
    public bool facingLeft;
    public Vector3 _lastPosition;

    [Header("OpenInventory")]
    public GameObject inventory;
    public InputActionReference openInventory;
    public InputActionReference select1;
    public InputActionReference select2;
    public InputActionReference select3;
    public InputActionReference select4;
    public InputActionReference select5;
    public InputActionReference select6;
    public InputActionReference select7;
    public InputActionReference select8;
    public InputActionReference select9;

    [Header("Pause")]
    public InputActionReference gamePause;

    [Header("ItemsController")]
    public GameObject Item;
    public InputActionReference leftClick;
    public InputActionReference rightClick;

    [Header("Player Status")]
    public float currentplayerHP;
    private float currentplayerSP;
    public float currentplayerFood;
    public float playerSPD = 5f;
    public float attackDamage;

    [Header("Player Satatus UI")]
    public RectTransform HPBar;
    public RectTransform FoodBar;


    [Header("Bonfire")]
    public Transform fireTransform;


    void Start()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        PlayerTransform = GetComponent<Transform>();
        PlayerInput = GetComponent<PlayerInput>();
        playerAnimator = transform.Find("Player").GetComponentInChildren<Animator>();
        playerRenderer = transform.Find("Player").GetComponentInChildren<SpriteRenderer>();
        _lastPosition = Vector3.zero;
        UpdateStatus();

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.isGameOver) { return; }
        _moveDirection = move.action.ReadValue<Vector2>();
        if (facingLeft) { playerRenderer.flipX = true; }
        else { playerRenderer.flipX = false; }
        
        FoodReduce();
    }
    private void FixedUpdate()
    {
        PlayerRB.linearVelocity = new Vector2(_moveDirection.x * playerSPD, _moveDirection.y * playerSPD);
        if (_moveDirection.x > 0 )
        {
            facingLeft = false;
        }
        else if (_moveDirection.x < 0)
        {
            facingLeft = true;
        }
        playerAnimator.SetFloat("speed",PlayerRB.linearVelocity.magnitude); // Check player velocity
        BonfireDistance();
    }

    private bool PlayerMovementChecking()
    {
        if (_lastPosition != transform.position)
        {
            _lastPosition=transform.position;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void UpdateStatus()
    {
        if (characterData != null)
        {
            currentplayerHP = characterData.HP;
            currentplayerSP = characterData.SP;
            currentplayerFood = characterData.Food;
            playerSPD = characterData.SPD;
            attackDamage = characterData.attackDamage;
        }
    }

    public void TakeDamage(float damage)
    {
        if (currentplayerHP - damage <= 0)
        {
            currentplayerHP = 0;
            playerAnimator.SetTrigger("isDie");
            Invoke("Die", 2f);
        }
        else
        {
            currentplayerHP -= damage;
            playerAnimator.SetTrigger("isAttacked");
        }
        HPBarUIUpdate();
    }
    private void Die()
    {
        GameManager.instance.GameOver();
    }
    public void FoodReduce()
    {
        if (PlayerMovementChecking())
        {
            currentplayerFood -= 0.5f *Time.deltaTime;
        }
        else
        {
            currentplayerFood -= 0.2f*Time.deltaTime;
        }
        if (currentplayerFood < 0)
        {
            currentplayerFood = 0;
        }
        FoodBarUIUpdate();
    }

    public void Healing(float heal)
    {
        currentplayerHP += heal;
        if (currentplayerHP > characterData.HP)
        {
            currentplayerHP = characterData.HP;
        }
        HPBarUIUpdate();

    }

    public void Eating(float food, float heal)
    {
        currentplayerFood += food;
        if (currentplayerFood > characterData.Food)
        {
            currentplayerFood = characterData.Food;
        }
        Healing(heal);
        FoodBarUIUpdate();
    }

    private void HPBarUIUpdate()
    {
        Vector2 percent = HPBar.sizeDelta;
        percent.x = 460 * (currentplayerHP/ characterData.HP);
        this.HPBar.sizeDelta = percent;
    }
    private void FoodBarUIUpdate()
    {
        Vector2 percent = FoodBar.sizeDelta;
        percent.x = 460 * (currentplayerFood / characterData.Food);
        this.FoodBar.sizeDelta = percent;
    }

    private void BonfireDistance()
    {
        float distance = Vector3.Distance(transform.position, fireTransform.position);
        if (distance > fireTransform.GetComponent<CircleCollider2D>().radius)
        {
            TakeDamage(5*Time.deltaTime);
        }
        else if(currentplayerFood >0 && currentplayerHP < characterData.HP)
        {
            Healing(0.5f * Time.deltaTime);
        }
    }
    


    private void OnEnable()
    {
        leftClick.action.started += LeftClick;
        rightClick.action.started += RightClick;
        openInventory.action.started += OpenInventory;
        select1.action.started += Select1;
        select2.action.started += Select2;
        select3.action.started += Select3;
        select4.action.started += Select4;
        select5.action.started += Select5;
        select6.action.started += Select6;
        select7.action.started += Select7;
        select8.action.started += Select8;
        select9.action.started += Select9;
        gamePause.action.started += GamePause;

    }

    private void OnDisable()
    {
        leftClick.action.started -= LeftClick;
        rightClick.action.started -= RightClick;
        openInventory.action.started -= OpenInventory;
        select1.action.started -= Select1;
        select2.action.started -= Select2;
        select3.action.started -= Select3;
        select4.action.started -= Select4;
        select5.action.started -= Select5;
        select6.action.started -= Select6;
        select7.action.started -= Select7;
        select8.action.started -= Select8;
        select9.action.started -= Select9;
        gamePause.action.started -= GamePause;

    }

    public void OpenInventory(InputAction.CallbackContext context)
    {
        if (GameManager.instance.isGameOver) { return; }
        if (!inventory.activeSelf)
        {
            InventoryManager.instance.ShowInventory();

        }
        else
        {
            InventoryManager.instance.HideInventory();
        }
    }

    private void LeftClick(InputAction.CallbackContext context)
    {
        if (GameManager.instance.isGameOver) { return; }
        Item.GetComponent<ItemController>().LeftClick();
    }
    private void RightClick(InputAction.CallbackContext context)
    {
        if (GameManager.instance.isGameOver) { return; }
        Item.GetComponent<ItemController>().RightClick();
    }

    private void Select1(InputAction.CallbackContext context)
    {
        SelectSlot(0);
    }
    private void Select2(InputAction.CallbackContext context)
    {
        SelectSlot(1);
    }
    private void Select3(InputAction.CallbackContext context)
    {
        SelectSlot(2);
    }
    private void Select4(InputAction.CallbackContext context)
    {
        SelectSlot(3);
    }
    private void Select5(InputAction.CallbackContext context)
    {
        SelectSlot(4);
    }
    private void Select6(InputAction.CallbackContext context)
    {
        SelectSlot(5);
    }
    private void Select7(InputAction.CallbackContext context)
    {
        SelectSlot(6);
    }
    private void Select8(InputAction.CallbackContext context)
    {
        SelectSlot(7);
    }
    private void Select9(InputAction.CallbackContext context)
    {
        SelectSlot(8);
    }

    private void GamePause(InputAction.CallbackContext context)
    {
        GameManager.instance.GamePause();
    }

    void SelectSlot(int index)
    {
        InventoryManager.instance.ChangeToolBarSelectedSlot(index);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ItemDrop"))
        {
            ItemSO item = collision.GetComponent<ItemDropManager>().itemSO;
            int quantity = collision.GetComponent<ItemDropManager>().currentStack;
            int durability = collision.GetComponent<ItemDropManager>().currentDurbility;
            InventoryManager.instance.AddItem(item,durability,quantity);
            Destroy(collision.gameObject);
        }
    }
}
