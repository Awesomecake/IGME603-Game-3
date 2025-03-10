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
        WorldManager.Instance?.SetTile(currentPosition, null);
        WorldManager.Instance?.SetTile(currentPosition + Vector3.up * 0.1f, null);
        WorldManager.Instance?.SetTile(currentPosition + Vector3.down * 0.1f, null);
        WorldManager.Instance?.SetTile(currentPosition + Vector3.left * 0.1f, null);
        WorldManager.Instance?.SetTile(currentPosition + Vector3.right * 0.1f, null);
        WorldManager.Instance?.SetTile(currentPosition + Vector3.up * 0.05f + Vector3.right * 0.05f, null);
        WorldManager.Instance?.SetTile(currentPosition + Vector3.up * 0.05f + Vector3.left * 0.05f, null);
        WorldManager.Instance?.SetTile(currentPosition + Vector3.down * 0.05f + Vector3.right * 0.05f, null);
        WorldManager.Instance?.SetTile(currentPosition + Vector3.down * 0.05f + Vector3.left * 0.05f, null);
    }

    private void HandlePlayerHit(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (LevelManager.Instance)
        {
            collision.GetComponent<PlayerController>().Die();
            LevelManager.Instance.NotifyPlayerDie();
        }
        else
        {
            Destroy(collision.gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}