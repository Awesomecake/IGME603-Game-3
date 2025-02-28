using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Drill : Throwable
{
    //Map the drill needs to edit
    private Tilemap tilemap;
    private TilemapCollider2D tilemapCollider;

    private void Update()
    {
        if (tilemap != null)
        {
            Vector3Int cellPosition = tilemap.WorldToCell(transform.position + transform.forward);

            tilemap.SetTile(cellPosition + new Vector3Int(0, 0, 0), null);


            tilemapCollider.CreateMesh(true, true);
        }
    }

    //On Collision, gets map and collision to edit
    public override void ThrownItemCollided(Collider2D collision)
    {
        tilemap = collision.GetComponent<Tilemap>();
        tilemapCollider = collision.GetComponent<TilemapCollider2D>();

        Destroy(gameObject);
    }
}
