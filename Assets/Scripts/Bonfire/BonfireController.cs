using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Rendering.Universal;

public class BonfireController : MonoBehaviour
{
    [Header("Component")]
    public GameObject bonfire;
    public BonfireSO bonfireSO;
    public CircleCollider2D fireRadiusCircle;
    public BoxCollider2D bonfireHitbox;
    public Transform playerPosition;
    public Animator bonfireAnimator;
    public Light2D bonfireLight;

    [Header("Data")]
    public float currentFirePower;
    public float currentHP;
    public float fireRadius;

    //test
    public BonfireSO BonfireSO1;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateComponent();
    }
    private void Update()
    {
        CheckLayer(); 
        BurnDown();
    }

    private void UpdateComponent()
    {
        bonfire = transform.GetChild(0).gameObject;
        fireRadiusCircle = GetComponent<CircleCollider2D>();
        bonfireHitbox = GetComponentInChildren<BoxCollider2D>();
        playerPosition = GameObject.FindWithTag("Player").GetComponent<Transform>();
        currentFirePower = bonfireSO.bonfirePower;
        bonfireAnimator = bonfire.GetComponent<Animator>();
        bonfireLight = bonfire.GetComponentInChildren<Light2D>();
        currentHP = bonfireSO.bonfireHP;
    }
    //public void UpdateBonfire(BonfireSO newBonfire) //for Update 
    //{
    //    bonfireSO = newBonfire;
    //    currentFirePower = newBonfire.bonfirePower;
    //    currentHP = newBonfire.bonfireHP;
    //    fireRadius = newBonfire.bonfireRadius;
    //    DestroyImmediate(bonfire);
    //    Instantiate(newBonfire.bonfirePrefab, Vector3.zero, Quaternion.identity).transform.SetParent(transform);
    //    bonfire = transform.GetChild(0).gameObject;
    //    bonfireHitbox = GetComponentInChildren<BoxCollider2D>();
    //}

    private void CheckLayer()
    {
        if (transform.position.y +0.1f > playerPosition.position.y)
        {
            bonfire.GetComponent<SpriteRenderer>().sortingLayerName = "BuildBehindLayer";
        }
        else
        {
            bonfire.GetComponent<SpriteRenderer>().sortingLayerName = "BuildFrontLayer";
        }
    }

    private void BurnDown()
    {
        currentFirePower -= bonfireSO.decayRate * Time.deltaTime;
        currentFirePower = Mathf.Clamp(currentFirePower, 0, bonfireSO.bonfirePower);
        AnimationUpdate();
        UpdateFireRadius();
    }
    
    private void UpdateFireRadius()
    {
        if (fireRadiusCircle != null)
        {
            fireRadiusCircle.radius = Mathf.Lerp(1f, bonfireSO.bonfireRadius, currentFirePower / bonfireSO.bonfirePower);
            if (currentFirePower ==0 ) { fireRadiusCircle.radius = 0; }
            bonfireLight.pointLightOuterRadius = fireRadiusCircle.radius;
        }
    }
    private void AnimationUpdate()
    {
        if (currentFirePower > 3 * (bonfireSO.bonfirePower / 4))
        {
            bonfireAnimator.SetTrigger("isMax");
        }
        else if (currentFirePower > 2 * (bonfireSO.bonfirePower / 4))
        {
            bonfireAnimator.SetTrigger("isMid");
        }
        else if (currentFirePower > (bonfireSO.bonfirePower / 4))
        {
            bonfireAnimator.SetTrigger("isLow");
        }
        else if (currentFirePower > 0)
        {
            bonfireAnimator.SetTrigger("isMin");
        }
        else
        {
            bonfireAnimator.SetTrigger("isOff");
        }
    }

    public void AddFuel(float fuel)
    {
        currentFirePower += fuel;
        currentFirePower = Mathf.Clamp(currentFirePower, 0, bonfireSO.bonfirePower);
    }

    public void TakeDamage(float damage)
    {
        if (currentHP > damage)
        {
            currentHP -= damage;
        }
        else
        {
            currentHP = bonfireSO.bonfireHP;
            currentFirePower -= damage *5;
        }
    }
}
