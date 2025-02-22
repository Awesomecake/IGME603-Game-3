using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D projectileRigidBody;

    //Apply force to thrown item
    public void ThrowItem(float strength, Vector2 direction)
    {
        projectileRigidBody.AddForce(direction*strength);
    }

    //Detecting when item overlaps a rigidbody
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.tag.Equals("Player"))
        {
            ThrownItemCollided(collision);
        }
    }

    public virtual void ThrownItemCollided(Collider2D collision)
    {
        Debug.LogWarning(collision.name);
        Destroy(gameObject);
    }
}
