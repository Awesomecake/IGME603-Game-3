using UnityEngine;

public class PlayerEnterDetector : MonoBehaviour
{
    [SerializeField] private PlayerDetector playerDetector;

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckPlayer(other);
        CheckBro(other);
    }

    private void CheckPlayer(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        SpottedTarget(other);
    }

    private void CheckBro(Collider2D other)
    {
        if (!other.CompareTag("LilBro")) return;
        var bro = GetComponent<LilBro>();
        if (!bro) return;
        if (bro.currentState == LilBro.State.Downed) return;
        SpottedTarget(other);
    }

    private void SpottedTarget(Collider2D other)
    {
        playerDetector.SpottedTarget(other.transform);
    }
}