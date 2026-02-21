using System.Collections;
using TMPro;
using UnityEngine;

public abstract class BaseGameController : MonoBehaviour
{
    [SerializeField]
    protected GameStateSO gameStateSO;

    [SerializeField]
    protected TextMeshProUGUI warningText;

    [SerializeField]
    protected GameObject warningTextParent;

    private Coroutine warningCoroutine;

    public abstract void LoadGame();
    public abstract void UnloadGame();

    protected void ShowWarningMessage(string message)
    {
        if (warningCoroutine != null)
        {
            StopCoroutine(warningCoroutine);
        }
        warningCoroutine = StartCoroutine(ShowWarningMessageCoroutine(message));
    }

    protected IEnumerator ShowWarningMessageCoroutine(string message)
    {
        warningTextParent.SetActive(true);
        warningText.text = message;
        yield return new WaitForSeconds(1.5f);
        warningTextParent.SetActive(false);
    }
}
