using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalExit : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            if(collision.GetComponent<PlayerController>().hasDiamond)
            {
                Destroy(collision.gameObject);
                SceneManager.LoadScene("MainMenu");
            }
        }
    }
}
