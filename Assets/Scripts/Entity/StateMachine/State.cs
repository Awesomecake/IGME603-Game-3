using UnityEngine;

// Handles the logic for an entity being in this current state.
// Each state should be added as a child of a HierarchicalStateMachine.
public abstract class State: MonoBehaviour
{
    // Triggered once when the state begins
    public virtual void Enter()
    {
    }

    // Triggered once when the state ends
    public virtual void Exit()
    {
        
    }

    // Triggered every frame while the state is active. Update()
    public virtual void FrameUpdate()
    {
    }

    // Triggered every physics update while the state is active. FixedUpdate()
    public virtual void PhysicsUpdate()
    {
    }

    public virtual State GetCurrentState()
    {
        return this;
    }
}