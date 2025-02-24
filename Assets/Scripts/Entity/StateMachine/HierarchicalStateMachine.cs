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
    [SerializeField] private bool autoStart = false; // only applicable to root state machine
    [SerializeField] private ResumeBehavior resumeBehavior = ResumeBehavior.Continue;
    private bool _isRunning = false; 

    [HideInInspector] public float lastTransition = 0f;

    private State _currentState;
    private readonly Dictionary<State, List<StateTransition>> _transitions = new();
    
    // Manually start the state machine. Only call on the root state machine.
    public void Begin()
    {
        EnterState();
        _isRunning = true;
    }

    // Manually stop the state machine. Only call on the root state machine.
    public void End()
    {
        _isRunning = false;
        ExitState();
    }

    private void Start()
    {
        if (!initialState) FindInitialState();
        _currentState = initialState;

        SetupTransitions();
        
        // TODO add debug message warning about unreachable states
        if (autoStart) StartCoroutine(Util.AfterDelay(0.1f, Begin));
    }

    private void Update()
    {
        if (_isRunning) StateUpdate();
    }

    private void FixedUpdate()
    {
        if (_isRunning) StateFixedUpdate();
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

    public override void EnterState()
    {
        if (resumeBehavior == ResumeBehavior.Reset)
        {
            _currentState = initialState;
        }

        _currentState.EnterState();
        lastTransition = Time.time;
    }

    public override void ExitState()
    {
        _currentState.ExitState();
    }

    public override void StateUpdate()
    {
        _currentState.StateUpdate();
        CheckChangeState();
    }

    public override void StateFixedUpdate()
    {
        _currentState.StateFixedUpdate();
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
            transition.onTransitionSideEffect?.Invoke();
            break;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void ChangeState(State toState)
    {
        Debug.Log($"Changing state from {_currentState.gameObject.name} to {toState.gameObject.name}");
        _currentState.ExitState();
        _currentState = toState;
        _currentState.EnterState();
        lastTransition = Time.time;
    }

    public State GetCurrentState()
    {
        return _currentState;
    }
    
    public override State GetRunningState()
    {
        return _currentState.GetRunningState();
    }
}