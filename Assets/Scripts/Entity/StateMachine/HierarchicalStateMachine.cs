using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

// Handles updating the active state
// A State Machine which can be a child of another HierarchicalStateMachine.
public class HierarchicalStateMachine : State
{
    [Serializable]
    private enum ResumeBehavior
    {
        Continue,
        Reset
    }
    
    [SerializeField] private State initialState;
    [SerializeField] private bool isRunning = false; // only applicable to root state machine
    [SerializeField] private ResumeBehavior resumeBehavior = ResumeBehavior.Continue;

    [HideInInspector] public float lastTransition = 0f;

    private State _currentState;
    private readonly Dictionary<State, List<StateTransition>> _transitions = new();
    
    // Manually start the state machine. Only call on the root state machine.
    public void Begin()
    {
        Enter();
        isRunning = true;
    }

    // Manually stop the state machine. Only call on the root state machine.
    public void End()
    {
        isRunning = false;
        Exit();
    }

    private void Start()
    {
        if (!initialState) FindInitialState();
        _currentState = initialState;

        SetupTransitions();
        
        // TODO add debug message warning about unreachable states

        if (isRunning) Begin();
    }

    private void Update()
    {
        if (isRunning) FrameUpdate();
    }

    private void FixedUpdate()
    {
        if (isRunning) PhysicsUpdate();
    }

    private void FindInitialState()
    {
        foreach (Transform child in transform)
        {
            var state = child.gameObject.GetComponent<State>();
            if (!state) continue;

            initialState = _currentState;
            break;
        }
    }

    private void SetupTransitions()
    {
        var transitions = GetComponents<StateTransition>();
        foreach (var transition in transitions)
        {
            var fromState = transition.fromState;
            if (!_transitions.ContainsKey(fromState))
            {
                _transitions.Add(fromState, new List<StateTransition>());
            }

            _transitions[fromState].Add(transition);
            Debug.Log("added transition");
        }
        
        Debug.Log(_transitions.Keys.ToSeparatedString(", "));
    }

    public override void Enter()
    {
        if (resumeBehavior == ResumeBehavior.Reset)
        {
            _currentState = initialState;
        }

        _currentState.Enter();
        lastTransition = Time.time;
    }

    public override void Exit()
    {
        _currentState.Exit();
    }

    public override void FrameUpdate()
    {
        _currentState.FrameUpdate();
        CheckChangeState();
    }

    public override void PhysicsUpdate()
    {
        _currentState.PhysicsUpdate();
        CheckChangeState();
    }

    private void CheckChangeState()
    {
        if (!_transitions.ContainsKey(_currentState)) return;
        var transitions = _transitions[_currentState];
        foreach (var transition in transitions)
        {
            if (!transition.NeedsTransition()) continue;
            ChangeState(transition.toState);
            break;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void ChangeState(State toState)
    {
        Debug.Log($"Changing state from {_currentState.gameObject.name} to {toState.gameObject.name}");
        _currentState.Exit();
        _currentState = toState;
        _currentState.Enter();
        lastTransition = Time.time;
    }

    public override State GetCurrentState()
    {
        return _currentState;
    }
}