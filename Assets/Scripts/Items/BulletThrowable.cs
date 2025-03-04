using UnityEngine;
using UnityEngine.SceneManagement;

public class BulletThrowable : Throwable
{
    public override void ThrownItemCollided(Collider2D collision)
    {
        Debug.Log($"bullet hit {collision}");

        HandlePlayerHit(collision);
        
        Destroy(gameObject);
    }

    private void HandlePlayerHit(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        Destroy(collision.gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}