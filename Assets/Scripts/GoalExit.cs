using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalExit : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            if(collision.GetComponent<PlayerController>().hasDiamond)
                FindObjectOfType<MenuManager>().ChangeSceneFromName("MainMenu");
        }
    }
}
