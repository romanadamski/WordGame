using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenuDM : BaseMenuDM
{
    public UnityAction OnWordleButtonClick;
    public UnityAction OnDictionarleButtonClick;
    public UnityAction<LanguageType> OnGameLanguageChange;
}

public class MainMenu : BaseMenu<MainMenuDM>
{
    [SerializeField]
    protected GameSettingsSO gameSettingsSO;

    [SerializeField]
    private Button wordleButton;

    [SerializeField]
    private Button dictionarleButton;
    
    [SerializeField]
    private Button polishLanguageButton;

    [SerializeField]
    private Button englishLanguageButton;
    
    [SerializeField]
    private Color activeLanguageColor;
    
    [SerializeField]
    private Color inactiveLanguageColor;

    private bool isLanguageSet;

    public override void Show(MainMenuDM menuDM)
    {
        base.Show(menuDM);

        wordleButton.onClick.AddListener(menuDM.OnWordleButtonClick);
        dictionarleButton.onClick.AddListener(menuDM.OnDictionarleButtonClick);

        polishLanguageButton.onClick.AddListener(OnPolishLanguageButtonClick);
        englishLanguageButton.onClick.AddListener(OnEnglishLanguageButtonClick);

        if (!isLanguageSet)
        {
            isLanguageSet = true;
            switch (gameSettingsSO.DefaultLanguage)
            {
                case LanguageType.PL:
                    OnPolishLanguageButtonClick();
                    break;
                case LanguageType.EN:
                    OnEnglishLanguageButtonClick();
                    break;
                default:
                    break;
            }
        }
    }

    public override void Hide()
    {
        base.Hide();

        wordleButton.onClick.RemoveAllListeners();
        dictionarleButton.onClick.RemoveAllListeners();

        polishLanguageButton.onClick.RemoveAllListeners();
        englishLanguageButton.onClick.RemoveAllListeners();
    }

    private void OnPolishLanguageButtonClick()
    {
        polishLanguageButton.image.color = activeLanguageColor;
        englishLanguageButton.image.color = inactiveLanguageColor;
        menuDM.OnGameLanguageChange(LanguageType.PL);
    }

    private void OnEnglishLanguageButtonClick()
    {
        polishLanguageButton.image.color = inactiveLanguageColor;
        englishLanguageButton.image.color = activeLanguageColor;
        menuDM.OnGameLanguageChange(LanguageType.EN);
    }
}
