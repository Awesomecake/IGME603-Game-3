using UnityEngine;

public class DespawnLifetime : MonoBehaviour
{
    [SerializeField] private float lifetimeSeconds = 10f;

    private void Start()
    {
        StartCoroutine(Util.AfterDelay(lifetimeSeconds, () => Destroy(gameObject)));
    }
}