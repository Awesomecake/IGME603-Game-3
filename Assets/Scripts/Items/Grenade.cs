using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Grenade : Throwable
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
            if (colRB != null && !col.gameObject.tag.Equals("Projectile"))
            {
                //Calc distance between entity and explosion center
                float distance = Vector2.Distance(colRB.position, transform.position);
                //calc impulse direction
                Vector2 direction = (colRB.position - (Vector2)transform.position).normalized;

                //Apply Force
                //The greater the distance the weaker the explosion force
                colRB.AddForce(direction * knockbackForce * 2 / (distance + 1f));
            }
        }

        //Logic to Destroy Terrain

        Tilemap tilemap = collision.GetComponent<Tilemap>();
        TilemapCollider2D tilemapCollider2D = collision.GetComponent<TilemapCollider2D>();

        if (tilemap != null)
        {
            Vector3Int cellPosition = tilemap.WorldToCell(transform.position);

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    //Assume size of tilemap is 70x70, avoid destroying edges
                    if (cellPosition.x <= -9 || cellPosition.y <= -9 || cellPosition.x >= 50 || cellPosition.y >= 50)
                        continue;

                    tilemap.SetTile(cellPosition + new Vector3Int(i, j, 0), null);
                }
            }

            tilemapCollider2D.CreateMesh(true, true);
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
