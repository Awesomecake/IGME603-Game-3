using UnityEngine;

public class BulletThrowable : Throwable
{
    public override void ThrownItemCollided(Collider2D collision)
    {
        Debug.Log($"bullet hit {collision}");
        Destroy(gameObject);
    }
}