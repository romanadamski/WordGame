using UnityEngine;

public class BaseMenu : MonoBehaviour
{
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}

public class BaseMenu<T> : BaseMenu where T : BaseMenuDM
{
    protected T menuDM;

    public virtual void Show(T menuDM)
    {
        this.menuDM = menuDM;
        Show();
    }
}
