using UnityEngine;

public class State : MonoBehaviour
{
    protected StateMachine StateMachine { get; private set; }

    protected virtual void OnEnter() { }
    protected virtual void OnExit() { }

    private void Awake()
    {
        StateMachine = GetComponentInParent<StateMachine>();
    }

    public void Enter()
    {
        OnEnter();
    }

    public void Exit()
    {
        OnExit();
    }
}
