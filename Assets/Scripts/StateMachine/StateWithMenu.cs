using UnityEngine;

public abstract class StateWithMenu<T> : State where T : BaseMenu
{
    //todo zenject
    [SerializeField]
    protected T menu; 

    protected override void OnEnter()
    {
        menu.Show();
    }

    protected override void OnExit()
    {
        menu.Hide();
    }
}
