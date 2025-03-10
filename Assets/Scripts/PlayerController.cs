using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Movement Logic
    bool isMoving;
    Vector2 lastMoveDirection;

    //Tracks Game Status
    float health = 100f;
    public bool hasDiamond;

    //Determining Look Direction from both Keyboard & Controller
    Vector2 mousePosition;
    Vector2 controllerLookDirection;

    Vector2 lookDirection;

    private Rigidbody2D rb;

    [SerializeField] private SettingsData settingsData;

    public Rigidbody2D Rigidbody
    {
        get { return rb; }
    }

    public ItemPool itemPool;

    public GameObject item1;
    public GameObject item2;
    public GameObject item3;

    private Throwable item1ThrowableScript;
    private Throwable item2ThrowableScript;
    private Throwable item3ThrowableScript;

    //cooldown timers for items
    private float item1Cooldown = 0.1f;
    private float item2Cooldown = 0.1f;
    private float item3Cooldown = 0.1f;

    public int selectedSlot = 1;

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer heldSpriteRenderer;

    //HUD Display to show item selection
    HUD_ItemSelection HUD;

    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();

        List<GameObject> randomItems = new List<GameObject>(itemPool.itemPrefabs);
        item1 = randomItems[Random.Range(0, randomItems.Count)];
        randomItems.Remove(item1);

        item2 = randomItems[Random.Range(0, randomItems.Count)];
        randomItems.Remove(item2);

        item3 = randomItems[Random.Range(0, randomItems.Count)];
        randomItems.Remove(item3);

        rb = GetComponent<Rigidbody2D>();

        //Finds and updates HUD at runtime
        HUD = FindObjectOfType<HUD_ItemSelection>();
        HUD.StartToolSetup(item1, item2, item3);
        HUD.UpdateHUD(item1, item2, item3);
        HUD.player = this;

        LevelManager.Instance?.RegisterPlayer(gameObject);

        item1ThrowableScript = item1.GetComponent<Throwable>();
        item2ThrowableScript = item2.GetComponent<Throwable>();
        item3ThrowableScript = item3.GetComponent<Throwable>();
    }

    // Update is called once per frame
    private void Update()
    {
        HandleMovement();

        heldSpriteRenderer.transform.up = lookDirection;

        CooldownItems();
        UpdateHUD();

        //Debug.Log(item1ThrowableScript.itemCooldown + " - " + item2ThrowableScript.itemCooldown + " - " + item3ThrowableScript.itemCooldown);
    }

    private void HandleMovement()
    {
        if (isMoving)
        {
            rb.AddForce(lastMoveDirection * (Time.deltaTime * 500f));
        }

        //If we have a mouse position, calculate look-at
        if (mousePosition != Vector2.zero)
        {
            lookDirection = (mousePosition - (Vector2)transform.position).normalized;
        }
        //If we don't already have a lookdirection yet, get look direction from movement
        else if (controllerLookDirection == Vector2.zero)
        {
            lookDirection = lastMoveDirection.normalized;
        }
        else
        {
            lookDirection = controllerLookDirection;
        }

        _animator.SetBool("moving", isMoving);
    }

    private void CooldownItems()
    {
        if (item1Cooldown > 0) item1Cooldown -= Time.deltaTime;
        if (item2Cooldown > 0) item2Cooldown -= Time.deltaTime;
        if (item3Cooldown > 0) item3Cooldown -= Time.deltaTime;
    }

    private void UpdateHUD()
    {
        UpdateHUDSlot(1);
        UpdateHUDSlot(2);
        UpdateHUDSlot(3);

        HUD.UpdateCooldownUI(
            item1Cooldown / item1ThrowableScript.itemCooldown,
            item2Cooldown / item2ThrowableScript.itemCooldown,
            item3Cooldown / item3ThrowableScript.itemCooldown
        );
    }

    private void UpdateHUDSlot(int slotIndex)
    {
        var itemCooldown = slotIndex switch
        {
            1 => item1Cooldown,
            2 => item2Cooldown,
            3 => item3Cooldown,
            _ => item1Cooldown
        };
        if (selectedSlot == slotIndex && itemCooldown > 0)
        {
            heldSpriteRenderer.sprite = null;
            HUD.DeselectItems(slotIndex);
        }
        else if (itemCooldown < 0)
        {
            if (selectedSlot != slotIndex) return;
            switch (slotIndex)
            {
                case 1:
                    heldSpriteRenderer.sprite = item1.GetComponent<SpriteRenderer>().sprite;
                    HUD.SelectOne();
                    break;
                case 2:
                    heldSpriteRenderer.sprite = item2.GetComponent<SpriteRenderer>().sprite;
                    HUD.SelectTwo();
                    break;
                case 3:
                    heldSpriteRenderer.sprite = item3.GetComponent<SpriteRenderer>().sprite;
                    HUD.SelectThree();
                    break;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        _animator.Play("Noir_Dead");
    }

    private void ScrollHUDUI(float input)
    {
        if (input < 0)
        {
            selectedSlot -= 1;
            if (selectedSlot < 1) selectedSlot = 3;
        }
        else if (input > 0)
        {
            selectedSlot += 1;
            if (selectedSlot > 3) selectedSlot = 1;
        }

        switch (selectedSlot)
        {
            case 1:
                heldSpriteRenderer.sprite = item1.GetComponent<SpriteRenderer>().sprite;
                HUD.SelectOne();
                break;
            case 2:
                heldSpriteRenderer.sprite = item2.GetComponent<SpriteRenderer>().sprite;
                HUD.SelectTwo();
                break;
            case 3:
                heldSpriteRenderer.sprite = item3.GetComponent<SpriteRenderer>().sprite;
                HUD.SelectThree();
                break;
        }
    }

    #region InputActions

    //Get movement input from InputActions, update movement logic
    public void InputActionMove(InputAction.CallbackContext context)
    {
        Vector2 moveDirection = context.ReadValue<Vector2>().normalized;

        isMoving = (moveDirection != Vector2.zero);

        if (isMoving)
        {
            lastMoveDirection = moveDirection;

            if (moveDirection.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    //Tracks Last Position of Mouse on Screen
    public void InputActionLookMouse(InputAction.CallbackContext context)
    {
        mousePosition = Camera.main?.ScreenToWorldPoint(context.ReadValue<Vector2>())
                        ?? Vector2.zero;
    }

    //Tracks last look direction on controller, clears mouse position
    public void InputActionLookController(InputAction.CallbackContext context)
    {
        controllerLookDirection = context.ReadValue<Vector2>().normalized;
        mousePosition = Vector2.zero;
    }

    public void InputActionSelectOne(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            selectedSlot = 1;
            heldSpriteRenderer.sprite = item1.GetComponent<SpriteRenderer>().sprite;
            HUD.SelectOne();
        }
    }

    public void InputActionSelectTwo(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            selectedSlot = 2;
            heldSpriteRenderer.sprite = item2.GetComponent<SpriteRenderer>().sprite;
            HUD.SelectTwo();
        }
    }

    public void InputActionSelectThree(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            selectedSlot = 3;
            heldSpriteRenderer.sprite = item3.GetComponent<SpriteRenderer>().sprite;
            HUD.SelectThree();
        }
    }

    //Gets input from scroll wheel and updates HUD
    public void InputActionScrollSelect(InputAction.CallbackContext context)
    {
        float input = context.ReadValue<Vector2>().y;
        ScrollHUDUI(input);
    }

    //Trigger throw effect, spawn thrown object
    public void InputActionUseItem(InputAction.CallbackContext context)
    {
        if (!context.started || Time.timeScale == 0f)
            return;

        GameObject newItem = null;
        switch (selectedSlot)
        {
            case 1:
                if (item1Cooldown <= 0)
                {
                    newItem = item1;
                    item1Cooldown = item1ThrowableScript.itemCooldown;
                }
                else if (settingsData.isAutoSwapEnabled)
                {
                    ScrollHUDUI(1);
                }

                break;
            case 2:
                if (item2Cooldown <= 0)
                {
                    newItem = item2;
                    item2Cooldown = item2ThrowableScript.itemCooldown;
                }
                else if (settingsData.isAutoSwapEnabled)
                {
                    ScrollHUDUI(1);
                }

                break;
            case 3:
                if (item3Cooldown <= 0)
                {
                    newItem = item3;
                    item3Cooldown = item3ThrowableScript.itemCooldown;
                }
                else if (settingsData.isAutoSwapEnabled)
                {
                    ScrollHUDUI(1);
                }

                break;
        }

        if (newItem != null)
        {
            GameObject item = Instantiate(newItem, transform);
            item.transform.position = new Vector3(heldSpriteRenderer.transform.position.x,
                heldSpriteRenderer.transform.position.y, heldSpriteRenderer.transform.position.z);

            Throwable throwable = item.GetComponent<Throwable>();

            if (throwable != null)
            {
                throwable.ThrowItem(500f, lookDirection, gameObject);
                item.transform.up = lookDirection;
            }
        }
    }

    #endregion

    /*
    public void SwapItem(GameObject itemA, GameObject itemB)
    {
        GameObject tempItem = itemA;
        float tempCooldown = itemA.GetComponent<Throwable>().itemCooldown;

        itemA = itemB;
        itemA.GetComponent<Throwable>().itemCooldown = itemB.GetComponent<Throwable>().itemCooldown;

        itemB = tempItem;
        itemB.GetComponent<Throwable>().itemCooldown = tempCooldown;


        // Update the HUD to reflect the changes
        HUD.UpdateHUD(item1, item2, item3);
    }
    */
}