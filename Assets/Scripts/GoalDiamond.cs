using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDiamond : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            collision.GetComponent<PlayerController>().hasDiamond = true;
            Destroy(gameObject);
        }
    }
}
