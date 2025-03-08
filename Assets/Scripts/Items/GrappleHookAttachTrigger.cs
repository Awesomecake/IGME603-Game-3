using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHookAttachTrigger : MonoBehaviour
{
    public string ownerTag = "Player";

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var isOwner = !string.IsNullOrEmpty(ownerTag) && collision.tag.Equals(ownerTag);
        if (isOwner ||
            collision.tag.Equals("Projectile") ||
            collision.tag.Equals("Sound") ||
            collision.tag.Equals("Detector"))
            return;

        GetComponentInParent<GrappleHook>().ThrownItemCollided(collision);
    }
}
