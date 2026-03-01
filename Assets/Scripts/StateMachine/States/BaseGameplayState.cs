using UnityEngine;

public class BaseGameplayState : StateWithMenu<GameplayMenu>
{
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
    protected virtual string WinsKey => "";

    private int gamesCount;

    protected override void OnEnter()
    {
        base.OnEnter();

        eventChannelSO.OnGameEnd.AddListener(OnGameEnd);

        gamesCount = PlayerPrefs.GetInt(GamesCounterKey, 0);
        var wins = PlayerPrefs.GetInt(WinsKey, 0);
        var winRatio = gamesCount > 0 ? (wins / (float)gamesCount) * 100f : 0;

        gamesCounter.InitCounter(gamesCount, winRatio);
        gameController.LoadGame();
    }

    protected override void OnExit()
    {
        base.OnExit();
        eventChannelSO.OnGameEnd.RemoveListener(OnGameEnd);
        eventChannelSO.OnGameplayClear?.Invoke();
        gameController.UnloadGame();
    }

    private void OnGameUnload()
    {
        gamesCount++;
        PlayerPrefs.SetInt(GamesCounterKey, gamesCount);
    }

    private void OnGameEnd(bool isWin, string answer)
    {
        if (isWin)
        {
            var wins = PlayerPrefs.GetInt(WinsKey, 0);
            wins++;
            PlayerPrefs.SetInt(WinsKey, wins);
        }
        var title = isWin ? "You've won!" : "You've lost!";
        var message = $"The word was: {answer.ToUpper()}\nPlay again?";
        OnGameUnload();
        confirmationPopup.Show(title, message, ReinitState, GoToMainMenu, gameEndPopupDelay);
    }

    protected void OnCloseClick()
    {
        confirmationPopup.Show("Close", $"Are you sure you want to leave?\n", LeaveGame);
    }

    protected void ReinitState()
    {
        StateMachine.SetState(this);
    }

    private void LeaveGame()
    {
        OnGameUnload();
        GoToMainMenu();
    }

    protected void GoToMainMenu()
    {
        StateMachine.SetState<MainMenuState>();
    }
}
