using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalDiamond : MonoBehaviour
{
    private void Start()
    {
        var world = WorldManager.Instance;
        if (world) world.diamond = this;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            var hurtbox = collision.GetComponent<Hurtbox>();
            TriggerPlayerCollected(hurtbox ? hurtbox.owner : collision);
        }

        if (collision.tag.Equals("LilBro"))
        {
            TriggerBroCollected(collision);
        }
    }

    private void TriggerBroCollected(Collider2D collision)
    {
        DestroyDiamond();
        if (LevelManager.Instance)
        {
            LevelManager.Instance.NotifyBroCollectedDiamond();
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void TriggerPlayerCollected(Collider2D collision)
    {
        collision.GetComponent<PlayerController>().hasDiamond = true;
        DestroyDiamond();
        if (LevelManager.Instance)
        {
            LevelManager.Instance.NotifyPlayerCollectedDiamond();
        }
        else
        {
            Destroy(collision.gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void DestroyDiamond()
    {
        Destroy(gameObject);
    }
}