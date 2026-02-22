using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ConfirmationPopup : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI title;
    
    [SerializeField]
    private TextMeshProUGUI message;
    
    [SerializeField]
    private Button confirmButton;
    
    [SerializeField]
    private Button declineButton;
    
    [SerializeField]
    private GameObject confirmationPopupObject;

    public void Show(string titleText, string messageText, UnityAction confirmAction, UnityAction declineAction = null, float delay = 0f)
    {
        title.text = titleText;
        message.text = messageText;
        confirmButton.onClick.AddListener(confirmAction);
        if (declineAction != null)
        {
            declineButton.onClick.AddListener(declineAction);
        }
        confirmButton.onClick.AddListener(Hide);
        declineButton.onClick.AddListener(Hide);

        if (delay <= 0)
        {
            confirmationPopupObject.SetActive(true);
        }
        else
        {
            StartCoroutine(Show(delay));
        }
    }

    private IEnumerator Show(float delay)
    {
        yield return new WaitForSeconds(delay);
        confirmationPopupObject.SetActive(true);
    }

    public void Hide()
    {
        confirmationPopupObject.SetActive(false);

        title.text = string.Empty;
        message.text = string.Empty;
        confirmButton.onClick.RemoveAllListeners();
        declineButton.onClick.RemoveAllListeners();
    }
}
