using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // private PatrolHandler _patrolHandler;

    #region Variables to be set on spawn
    
    // public Path patrolPath;
    
    #endregion
    
    private void Awake()
    {
        // _patrolHandler = GetComponentInChildren<PatrolHandler>();
        // if (!_patrolHandler) Debug.LogWarning("No patrol handler found");
        // if (patrolPath && _patrolHandler) _patrolHandler.SetPath(patrolPath);
    }

}