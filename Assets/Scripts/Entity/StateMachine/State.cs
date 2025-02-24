using UnityEngine;

// Handles the logic for an entity being in this current state.
// Each state should be added as a child of a HierarchicalStateMachine.
public abstract class State: MonoBehaviour
{
    // Triggered once when the state begins
    public virtual void EnterState()
    {
    }

    // Triggered once when the state ends
    public virtual void ExitState()
    {
        
    }

    // Triggered every frame while the state is active. Update()
    public virtual void StateUpdate()
    {
    }

    // Triggered every physics update while the state is active. FixedUpdate()
    public virtual void StateFixedUpdate()
    {
    }
    
    public virtual State GetRunningState()
    {
        return this;
    }
}