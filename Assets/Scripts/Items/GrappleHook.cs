using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : Throwable
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private CircleCollider2D circleCollider;
    private PlayerController playerController;

    //After Collision Visual Logic
    private bool hasCollidedToDynamic = false;
    private GameObject newAttachedTarget;

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    public override void ThrownItemCollided(Collider2D collision)
    {
        //Stops projectile from moving anymore
        projectileRigidBody.velocity= Vector3.zero;
        projectileRigidBody.isKinematic = true;

        Rigidbody2D colRB = collision.GetComponent<Rigidbody2D>();
        if (colRB != null) 
        {
            //If hit object is static, pull player
            if (colRB.isKinematic)
            {
                Vector2 direction = (projectileRigidBody.position - playerController.Rigidbody.position).normalized;
                playerController.Rigidbody.AddForce(direction * 500);
            }
            //if hit object is not static, pull object
            else
            {
                newAttachedTarget = collision.gameObject;
                hasCollidedToDynamic = true;

                Vector2 direction = (playerController.Rigidbody.position - colRB.position).normalized;
                colRB.AddForce(direction * 250);
            }
        }

        StartCoroutine(DestroyDelay());
    }

    private IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (hasCollidedToDynamic)
        {
            projectileRigidBody.gameObject.transform.position = newAttachedTarget.transform.position;
        }

        lineRenderer.SetPosition(0, playerController.transform.position);
        lineRenderer.SetPosition(1, projectileRigidBody.position);
        circleCollider.offset = (projectileRigidBody.position - (Vector2)transform.position);
    }
}
