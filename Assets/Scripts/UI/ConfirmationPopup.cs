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

    public void Show(string titleText, string messageText, UnityAction confirmAction, UnityAction declineAction = null)
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

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);

        title.text = string.Empty;
        message.text = string.Empty;
        confirmButton.onClick.RemoveAllListeners();
        declineButton.onClick.RemoveAllListeners();
    }
}
