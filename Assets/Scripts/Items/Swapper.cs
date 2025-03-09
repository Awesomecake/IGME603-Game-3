using UnityEngine;

public class Swapper : Throwable
{
    public override void ThrownItemCollided(Collider2D collision)
    {
        if (collision.tag.Equals("Enemy") || collision.tag.Equals("LilBro"))
        {
            Vector3 playerLocation = Owner.transform.position;
            Vector3 guardLocation = collision.transform.position;

            collision.transform.position = playerLocation;
            Owner.transform.position = guardLocation;
        }

        Destroy(gameObject);
    }
}
