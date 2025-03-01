using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Swapper : Throwable
{
    private void Awake()
    {
        itemSpeedModifier = 2f;
    }
    public override void ThrownItemCollided(Collider2D collision)
    {
        if (collision.tag.Equals("Enemy"))
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            Vector3 playerLocation = player.transform.position;
            Vector3 guardLocation = collision.transform.position;

            collision.transform.position = playerLocation;
            player.transform.position = guardLocation;
        }

        Destroy(gameObject);
    }
}
