using UnityEngine;

public class DictionarleGameplayState : BaseGameplayState
{
    [SerializeField]
    private GameStateSO gameStateSO;

    protected override string GamesCounterKey => "DictionarleCurrentStreak";
    protected override string WinsKey => "DictionarleWins";

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
