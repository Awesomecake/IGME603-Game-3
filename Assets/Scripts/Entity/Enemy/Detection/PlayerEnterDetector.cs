using UnityEngine;

public class PlayerEnterDetector : MonoBehaviour
{
    [SerializeField] private PlayerDetector playerDetector;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") && !other.CompareTag("LilBro")) return;
        playerDetector.SpottedTarget(other.transform);
    }
}