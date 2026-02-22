using System;

public class DictionarleGameplayState : BaseGameplayState
{
    protected override void OnEnter()
    {
        menu.Show(new GameplayMenuDM
        {
            Title = "Dictionarle",
            Language = gameStateSO.CurrentLanguageConfig.Language,
            OnCloseClick = OnCloseClick
        });

        base.OnEnter();
    }
}
