using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletThrowable : Throwable
{
    public override void ThrownItemCollided(Collider2D collision)
    {
        Debug.Log($"bullet hit {collision}");

        DestroyTile();

        HandlePlayerHit(collision);
        Destroy(gameObject);
    }

    private void DestroyTile()
    {
        var currentPosition = transform.position;
        var direction = projectileRigidBody.velocity.normalized.ToVector3();
        WorldManager.Instance?.SetTile(currentPosition, null);
        WorldManager.Instance?.SetTile(currentPosition + direction, null);
    }

    private void HandlePlayerHit(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (LevelManager.Instance)
        {
            LevelManager.Instance.NotifyPlayerDie();
        }
        else
        {
            Destroy(collision.gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}