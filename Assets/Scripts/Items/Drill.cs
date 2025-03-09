using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Drill : Throwable
{
    //Map the drill needs to edit
    private Tilemap tilemap;
    private TilemapCollider2D tilemapCollider;

    [SerializeField] private GameObject soundPrefab;

    //Tracking forward and right values before rotation is changed to improve visuals 
    private Vector3 initialForward;
    private Vector3 initialRight;

    public void Start()
    {
        initialForward = transform.forward;
        initialRight = transform.right;

        Vector3 angles = transform.rotation.eulerAngles;
        angles.z += 45;
        transform.rotation = Quaternion.Euler(angles);
    }

    private void Update()
    {
        //If we have found tilemap, start destroying it
        if (tilemap != null)
        {
            Vector3Int cellPosition = tilemap.WorldToCell(transform.position + initialForward);

            //Assume size of tilemap is 70x70, avoid destroying edges
            if (cellPosition.x == -10 || cellPosition.y == -10 || cellPosition.x == 59 || cellPosition.y == 59)
            {
                Destroy(gameObject);
                return;
            }

            if (tilemap.GetTile(cellPosition))
            {
                if (soundPrefab)
                {
                    Instantiate(
                        soundPrefab,
                        transform.position,
                        quaternion.identity
                    );
                }

                tilemap.SetTile(cellPosition, null);
            }

            Vector3Int cellSidePosition = tilemap.WorldToCell(transform.position + initialForward + initialRight);

            //Assume size of tilemap is 70x70, avoid destroying edges
            if (cellSidePosition.x == -10 || cellSidePosition.y == -10 || cellSidePosition.x == 59 || cellSidePosition.y == 59)
            {
                return;
            }

            if (tilemap.GetTile(cellSidePosition))
            {
                tilemap.SetTile(cellSidePosition, null);
            }


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