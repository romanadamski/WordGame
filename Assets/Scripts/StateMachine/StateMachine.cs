using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private State CurrentState => _states.Peek();
    private readonly Stack<State> _states = new();
    private bool IsEmpty => _states.Count == 0;

    private State[] availableStates;

    private void Awake()
    {
        availableStates = GetComponentsInChildren<State>(true);
        SetState<MainMenuState>();
    }

    private void PushState(State state)
    {
        if (IsEmpty || CurrentState != state)
        {
            _states.Push(state);
        }

        CurrentState.Enter();
    }

    private void PopState()
    {
        if (IsEmpty) return;
        
        CurrentState.Exit();
        _states.Pop();
    }

    public void SetState<T>() where T : State
    {
        var state = availableStates.FirstOrDefault((s) => s.GetType() == typeof(T) || s.GetType().IsSubclassOf(typeof(T)));
        if (state == null)
        {
            Debug.LogError($"State {typeof(T).Name} not found in {name} children!");
            return;
        }
        PopState();
        PushState(state);
    }

    public void SetState(State state)
    {
        if (state == null)
        {
            Debug.LogError($"State {state.GetType().Name} not found in {name} children!");
            return;
        }
        PopState();
        PushState(state);
    }

    public void Clear()
    {
        if (IsEmpty) return;

        CurrentState.Exit();
        _states.Clear();
    }
}
