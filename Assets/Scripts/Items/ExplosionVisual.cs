using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExplosionVisual : MonoBehaviour
{
    [SerializeField] private bool isLethal;

    private Collider2D _collider2D;
    
    private void Start()
    {
        _collider2D = GetComponent<Collider2D>();
        StartCoroutine(DestroyDelay());
    }

    private IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(0.05f);
        _collider2D.enabled = false;
        yield return new WaitForSeconds(0.45f);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (LevelManager.Instance)
        {
            LevelManager.Instance.NotifyPlayerDie();
        }
        else
        {
            Destroy(other.gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
