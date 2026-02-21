using UnityEngine;

public class WordleGameplayState : BaseGameplayState
{
    protected override void OnEnter()
    {
        menu.Show(new GameplayMenuDM
        {
            Title = "Wordle",
            Language = gameStateSO.CurrentLanguageConfig.Language,
            OnCloseClick = OnCloseClick
        });

        base.OnEnter();
    }

    protected override void OnExit()
    {
        eventChannelSO.OnGameplayClear?.Invoke();

        base.OnExit();
    }
}
