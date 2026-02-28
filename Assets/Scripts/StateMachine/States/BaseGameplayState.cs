using UnityEngine;

public class BaseGameplayState : StateWithMenu<GameplayMenu>
{
    [SerializeField]
    protected GameStateSO gameStateSO;
    
    [SerializeField]
    protected EventChannelSO eventChannelSO;

    [SerializeField]
    protected ConfirmationPopup confirmationPopup;
    
    [SerializeField]
    private BaseGameController gameController;
    
    [SerializeField]
    private GamesCounterUI gamesCounter;
    
    [SerializeField]
    private float gameEndPopupDelay = 0f;

    protected virtual string GamesCounterKey => "";
    protected virtual string CurrentStreakKey => "";
    protected virtual string WinsKey => "";

    private int currentStreak;
    private int gamesCount;

    protected override void OnEnter()
    {
        base.OnEnter();

        eventChannelSO.OnGameEnd.AddListener(OnGameEnd);

        gamesCount = PlayerPrefs.GetInt(GamesCounterKey, 0);
        var wins = PlayerPrefs.GetInt(WinsKey, 0);
        var winRatio = gamesCount > 0 ? (wins / (float)gamesCount) * 100f : 0;

        gamesCount++;
        PlayerPrefs.SetInt(GamesCounterKey, gamesCount);

        currentStreak = PlayerPrefs.GetInt(CurrentStreakKey, 0);
        gamesCounter.InitCounter(gamesCount, currentStreak, winRatio);
        gameController.LoadGame();
    }

    protected override void OnExit()
    {
        base.OnExit();
        eventChannelSO.OnGameEnd.RemoveListener(OnGameEnd);
        eventChannelSO.OnGameplayClear?.Invoke();
        gameController.UnloadGame();
    }

    private void OnGameEnd(bool isWin, string answer)
    {
        PlayerPrefs.SetInt(CurrentStreakKey, isWin ? currentStreak + 1 : 0);
        if (isWin)
        {
            var wins = PlayerPrefs.GetInt(WinsKey, 0);
            wins++;
            PlayerPrefs.SetInt(WinsKey, wins);
        }
        var title = isWin ? "You've won!" : "You've lost!";
        var message = $"The word was: {answer.ToUpper()}\nPlay again?";
        confirmationPopup.Show(title, message, ReinitState, GoToMainMenu, gameEndPopupDelay);
    }

    protected void OnCloseClick()
    {
        var messagePostfix = string.Empty;
        if (currentStreak > 0)
        {
            messagePostfix = $"\nYou'll lose {currentStreak} game{(currentStreak > 1 ? "s" : "")} streak";
        }
        confirmationPopup.Show("Close", $"Are you sure you want to leave?\n{messagePostfix}", LeaveGame);
    }

    protected void ReinitState()
    {
        StateMachine.SetState(this);
    }

    private void LeaveGame()
    {
        PlayerPrefs.SetInt(CurrentStreakKey, 0);

        GoToMainMenu();
    }

    protected void GoToMainMenu()
    {
        StateMachine.SetState<MainMenuState>();
    }
}
