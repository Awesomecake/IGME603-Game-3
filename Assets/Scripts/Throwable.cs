using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    public void ThrowItem(float strength, Vector2 direction)
    {
        rb.AddForce(direction*strength);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.tag.Equals("Player"))
        {
            Debug.LogWarning(collision.name);
            Destroy(gameObject);
        }
    }
}
