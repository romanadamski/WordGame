using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoseMenu : BaseMenu
{
    [SerializeField]
    private Button goToMainMenuButton;
    [SerializeField]
    private Button restartButton;

    private void Awake()
    {
        goToMainMenuButton.onClick.AddListener(OnGoToMainMenuClick);
        restartButton.onClick.AddListener(OnRestartClick);
    }

    private void OnGoToMainMenuClick()
    {

    }

    private void OnRestartClick()
    {

    }
}
