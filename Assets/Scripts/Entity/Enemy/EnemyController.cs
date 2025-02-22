using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D body;
    
    private void Start()
    {
        if (!body) body = GetComponent<Rigidbody2D>();
        if (!body) Debug.LogWarning("No rigidbody found");
    }

    private void Update()
    {
    }
}