public class DictionarleGameplayState : BaseGameplayState
{
    protected override string CurrentStreakKey => "DictionarleGames";
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
