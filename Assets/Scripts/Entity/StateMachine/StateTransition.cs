using UnityEngine;

// Handles the logic for transitioning from one state to another.
// Each transition should be added as a component to the same object as 
// a HierarchicalStateMachine.
public abstract class StateTransition: MonoBehaviour
{
    [SerializeField] public State fromState;
    [SerializeField] public State toState;

    public abstract bool NeedsTransition();
}