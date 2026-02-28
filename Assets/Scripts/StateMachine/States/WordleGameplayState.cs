using UnityEngine;

public class WordleGameplayState : BaseGameplayState
{
    [SerializeField]
    private KeyboardController keyboardController;

    protected override string CurrentStreakKey => "WordleCurrentStreak";
    protected override string GamesCounterKey => "WordleGames";
    protected override string WinsKey => "WordleWins";

    protected override void OnEnter()
    {
        menu.Show(new GameplayMenuDM
        {
            Title = "Wordle",
            Language = gameStateSO.CurrentLanguageConfig.Language,
            OnCloseClick = OnCloseClick
        });

        keyboardController.ShowKeyboard(gameStateSO.CurrentLanguageConfig.Language);

        base.OnEnter();
    }

    protected override void OnExit()
    {
        keyboardController.HideKeyboard();

        base.OnExit();
    }
}
