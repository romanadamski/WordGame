using UnityEngine;

public abstract class BaseGameController : MonoBehaviour
{
    [SerializeField]
    protected GameStateSO gameStateSO;

    public abstract void LoadGame();
    public abstract void UnloadGame();
}
