using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ImpulseBomb : Throwable
{
    public float knockbackForce;
    [SerializeField] private GameObject explosionPrefab;
    public override void ThrownItemCollided(Collider2D collision)
    {
        //Cast for entities in range of explosion
        Collider2D[] hit2D = Physics2D.OverlapCircleAll(gameObject.transform.position, 2f);

        //for all entities in range, check if they have a rigidbody and apply force
        foreach (Collider2D col in hit2D)
        {
            if (col.gameObject == gameObject)
                continue;

            Rigidbody2D colRB = col.GetComponent<Rigidbody2D>();

            //check if they have rigidbody
            if (colRB != null)
            {
                //Calc distance between entity and explosion center
                float distance = Vector2.Distance(colRB.position, transform.position);
                //calc impulse direction
                Vector2 direction = (colRB.position - (Vector2)transform.position).normalized;

                //Apply Force
                //The greater the distance the weaker the explosion force
                colRB.AddForce(direction * knockbackForce * 2/(distance+1f));
            }
        }

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 2f);
    }
}
