using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    bool isMoving;
    Vector2 lastMoveDirection;

    private Rigidbody2D rigidbody;

    public GameObject item1;

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
    }

    public void InputActionMove(InputAction.CallbackContext context)
    {
        Vector2 moveDirection = context.ReadValue<Vector2>().normalized;

        isMoving = (moveDirection != Vector2.zero);

        if (isMoving)
        {
            lastMoveDirection = moveDirection;
        }
    }

    public void InputActionThrow(InputAction.CallbackContext context)
    {
        GameObject item = Instantiate(item1, transform);

        Throwable throwable = item.GetComponent<Throwable>();

        if (throwable != null)
        {
            throwable.ThrowItem(500f, lastMoveDirection);
        }
    }
}
