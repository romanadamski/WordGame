using UnityEngine;

public class MainMenuState : StateWithMenu<MainMenu>
{
    [SerializeField]
    protected GameStateSO gameStateSO;
    
    [SerializeField]
    protected GameSettingsSO gameSettingsSO;

    protected override void OnEnter()
    {
        menu.Show(new MainMenuDM
        {
            OnWordleButtonClick = OnWordleButtonClick,
            OnDictionarleButtonClick = OnDictionarleButtonClick,
            OnGameLanguageChange = OnGameLanguageChange
        });
    }

    private void OnWordleButtonClick()
    {
        StateMachine.SetState<WordleGameplayState>();
    }

    private void OnDictionarleButtonClick()
    {
        StateMachine.SetState<DictionarleGameplayState>();
    }

    private void OnGameLanguageChange(LanguageType language)
    {
        switch (language)
        {
            case LanguageType.PL:
                gameStateSO.CurrentLanguageConfig = gameSettingsSO.PolishLanguage;
                break;
            case LanguageType.EN:
                gameStateSO.CurrentLanguageConfig = gameSettingsSO.EnglishLanguage;
                break;
            default:
                break;
        }
    }
}
