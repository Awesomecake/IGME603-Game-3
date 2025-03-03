using UnityEngine;

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
            collision.GetComponent<PlayerController>().hasDiamond = true;
            Destroy(gameObject);
        }
    }
}