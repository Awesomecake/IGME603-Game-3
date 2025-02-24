using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    bool isMoving;
    Vector2 lastMoveDirection;

    //Determining Look Direction from both Keyboard & Controller
    Vector2 mousePosition;
    Vector2 controllerLookDirection;

    Vector2 lookDirection;

    private Rigidbody2D rigidbody;
    public Rigidbody2D Rigidbody { get { return rigidbody; } }

    public GameObject item1;
    public GameObject item2;
    public GameObject item3;

    [SerializeField] private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            rigidbody.AddForce(lastMoveDirection * (Time.deltaTime * 500f));
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
    }

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
        mousePosition = Camera.main.ScreenToWorldPoint(context.ReadValue<Vector2>());
    }

    //Tracks last look direction on controller, clears mouse position
    public void InputActionLookController(InputAction.CallbackContext context)
    {
        controllerLookDirection = context.ReadValue<Vector2>().normalized;
        mousePosition = Vector2.zero;
    }

    //Trigger throw effect, spawn thrown object
    public void InputActionUseItemOne(InputAction.CallbackContext context)
    {
        if (context.started && item1 != null)
        {
            GameObject item = Instantiate(item1, transform);

            Throwable throwable = item.GetComponent<Throwable>();

            if (throwable != null)
            {
                throwable.ThrowItem(500f, lookDirection);
            }
        }
    }

    //Trigger throw effect, spawn thrown object
    public void InputActionUseItemTwo(InputAction.CallbackContext context)
    {
        if (context.started && item2 != null)
        {
            GameObject item = Instantiate(item2, transform);

            Throwable throwable = item.GetComponent<Throwable>();

            if (throwable != null)
            {
                throwable.ThrowItem(500f, lookDirection);
            }
        }
    }

    //Trigger throw effect, spawn thrown object
    public void InputActionUseItemThree(InputAction.CallbackContext context)
    {
        if (context.started && item3 != null)
        {
            GameObject item = Instantiate(item3, transform);

            Throwable throwable = item.GetComponent<Throwable>();

            if (throwable != null)
            {
                throwable.ThrowItem(500f, lookDirection);
            }
        }
    }
}
