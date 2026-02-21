using UnityEngine;

public class BaseGameplayState : StateWithMenu<GameplayMenu>
{
    [SerializeField]
    protected GameStateSO gameStateSO;
    
    [SerializeField]
    protected EventChannelSO eventChannelSO;

    [SerializeField]
    private ConfirmationPopup confirmationPopup;
    
    [SerializeField]
    private BaseGameController gameController;
    
    [SerializeField]
    private KeyboardController keyboardController;

    protected override void OnEnter()
    {
        base.OnEnter();

        keyboardController.ShowKeyboard(gameStateSO.CurrentLanguageConfig.Language);
        gameController.LoadGame();

        eventChannelSO.OnGameEnd.AddListener(OnGameEnd);
    }

    protected override void OnExit()
    {
        base.OnExit();

        keyboardController.HideKeyboard();
        gameController.UnloadGame();
        eventChannelSO.OnGameEnd.RemoveListener(OnGameEnd);
    }

    private void OnGameEnd(bool isWin, string answer)
    {
        var title = isWin ? "You've won!" : "You've lost!";
        var message = $"The word was: {answer}\nPlay again?";
        confirmationPopup.Show(title, message, ReinitState, GoToMainMenu);
    }

    protected void OnCloseClick()
    {
        confirmationPopup.Show("Close", "Are you sure you want to leave?", GoToMainMenu);
    }

    protected void ReinitState()
    {
        StateMachine.SetState(this);
    }

    protected void GoToMainMenu()
    {
        StateMachine.SetState<MainMenuState>();
    }
}
