using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum State
    {
        Normal,
        Stunned
    }

    [HideInInspector] public Rigidbody2D body;

    private State _currentState = State.Normal;
    // private PatrolHandler _patrolHandler;

    #region Variables to be set on spawn

    // public Path patrolPath;

    #endregion

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        // _patrolHandler = GetComponentInChildren<PatrolHandler>();
        // if (!_patrolHandler) Debug.LogWarning("No patrol handler found");
        // if (patrolPath && _patrolHandler) _patrolHandler.SetPath(patrolPath);
    }

    public State GetState()
    {
        return _currentState;
    }

    public void SetState(State state)
    {
        _currentState = state;
    }
}