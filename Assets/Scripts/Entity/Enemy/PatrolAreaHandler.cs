using UnityEngine;

public class PatrolAreaHandler : MonoBehaviour
{
    [SerializeField] private LocationContainer patrolTarget;
    [SerializeField] private PatrolArea areaPrefab;

    private PatrolArea _area;
    
    private void Start()
    {
        _area = Instantiate(areaPrefab, transform.position, Quaternion.identity);
    }

    public void NextPoint()
    {
        patrolTarget.UpdatePosition(_area.GetRandomLocation());
    }
}