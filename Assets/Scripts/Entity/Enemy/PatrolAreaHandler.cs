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
        var randomLocation = _area.GetRandomLocation();
        const int maxRetries = 20;
        var retryCount = 0;
        while (
            retryCount < maxRetries &&
            WorldManager.Instance &&
            WorldManager.Instance.GetTile(randomLocation)
        )
        {
            randomLocation = _area.GetRandomLocation();
            retryCount++;
        }

        patrolTarget.UpdatePosition(randomLocation);
    }
}