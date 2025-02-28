using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Drill : Throwable
{
    //Map the drill needs to edit
    private Tilemap tilemap;
    private TilemapCollider2D tilemapCollider;

    private void Awake()
    {
        itemSpeedModifier = 0.5f;
    }

    private void Update()
    {
        //If we have found tilemap, start destroying it
        if (tilemap != null)
        {
            Vector3Int cellPosition = tilemap.WorldToCell(transform.position + transform.forward);

            //Assume size of tilemap is 70x70, avoid destroying edges
            if (cellPosition.x == -10 || cellPosition.y == -10 || cellPosition.x == 59 || cellPosition.y == 59)
            {
                Destroy(gameObject);
                return;
            }

            tilemap.SetTile(cellPosition, null);

            tilemapCollider.CreateMesh(true, true);
        }
    }

    //On Collision, gets map and collision to edit
    public override void ThrownItemCollided(Collider2D collision)
    {
        tilemap = collision.GetComponent<Tilemap>();
        tilemapCollider = collision.GetComponent<TilemapCollider2D>();
    }
}
