using System.Collections;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D projectileRigidBody;
    public float itemSpeedModifier = 1f;

    public float itemCooldown = 5f;
    public string ownerTag = "Player";

    private void Start()
    {
        StartCoroutine(DeathTimer());
    }

    //Apply force to thrown item
    public void ThrowItem(float strength, Vector2 direction)
    {
        projectileRigidBody.AddForce(direction * strength * itemSpeedModifier);
    }

    //Detecting when item overlaps a rigidbody
    public void OnTriggerEnter2D(Collider2D collision)
    {
        var isOwner = !string.IsNullOrEmpty(ownerTag) && collision.tag.Equals(ownerTag);
        if (isOwner ||
            collision.tag.Equals("Projectile") ||
            collision.tag.Equals("Sound") ||
            collision.tag.Equals("Detector"))
            return;

        ThrownItemCollided(collision);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals(ownerTag)) ownerTag = null;
    }

    public virtual void ThrownItemCollided(Collider2D collision)
    {
        Debug.LogWarning(collision.name);
        Destroy(gameObject);
    }

    public IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(6);
        Destroy(gameObject);
    }
}