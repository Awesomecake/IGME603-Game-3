using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Throwable : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D projectileRigidBody;

    private void Start()
    {
        StartCoroutine(DeathTimer());
    }

    //Apply force to thrown item
    public void ThrowItem(float strength, Vector2 direction)
    {
        projectileRigidBody.AddForce(direction*strength);
    }

    //Detecting when item overlaps a rigidbody
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player") || collision.tag.Equals("Projectile") || collision.tag.Equals("Sound") || collision.tag.Equals("Detector"))
            return;

        ThrownItemCollided(collision);

        //Logic to Destroy Terrain

        //Tilemap tilemap = collision.GetComponent<Tilemap>();
        //TilemapCollider2D tilemapCollider2D = collision.GetComponent<TilemapCollider2D>();

        //if (tilemap != null)
        //{
        //    Vector3Int cellPosition = tilemap.WorldToCell(transform.position);

        //    tilemap.SetTile(cellPosition + new Vector3Int(1, 1, 0), null);
        //    tilemap.SetTile(cellPosition + new Vector3Int(1, 0, 0), null);
        //    tilemap.SetTile(cellPosition + new Vector3Int(1, -1, 0), null);
        //    tilemap.SetTile(cellPosition + new Vector3Int(0, 1, 0), null);
        //    tilemap.SetTile(cellPosition + new Vector3Int(0, 0, 0), null);
        //    tilemap.SetTile(cellPosition + new Vector3Int(0, -1, 0), null);
        //    tilemap.SetTile(cellPosition + new Vector3Int(-1, 1, 0), null);
        //    tilemap.SetTile(cellPosition + new Vector3Int(-1, 0, 0), null);
        //    tilemap.SetTile(cellPosition + new Vector3Int(-1, -1, 0), null);

        //    tilemapCollider2D.enabled = false;
        //    tilemapCollider2D.enabled = true;
        //}
    }

    public virtual void ThrownItemCollided(Collider2D collision)
    {
        Debug.LogWarning(collision.name);
        Destroy(gameObject);
    }

    public IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
