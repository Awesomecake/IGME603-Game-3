using UnityEngine;

public class PlayerExitDetector : MonoBehaviour
{
    [SerializeField] private PlayerDetector playerDetector;
    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerCheck(other);
        BroCheck(other);
        
    }

    private void PlayerCheck(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerDetector.LostTarget(other.transform);
    }
    
    private void BroCheck(Collider2D other)
    {
        if (!other.CompareTag("LilBro")) return;
        playerDetector.LostTarget(other.transform);
    }
}