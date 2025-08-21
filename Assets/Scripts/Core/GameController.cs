
using EventBusScripts;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameController : IGameController, ITickable, IDisposable
{
    public IGameStateMachine GameStateMachine { get; private set; }
    private readonly IUIService _uIService;
    private readonly IGameNavigationService _navigation;
    [Inject]
    public GameController(IPlayerSpawner playerSpawner, IUIService uIService, IGameNavigationService navigation)
    {
        EventBus.Get<LevelFailEvent>().Subscribe(OnLevelFail);
        EventBus.Get<LevelCompletedEvent>().Subscribe(OnLevelCompleted);
        _uIService = uIService;
        _navigation = navigation;

        GameStateMachine = new GameStateMachine(playerSpawner, this);
    }
    public void RequestPause()
    {
        _uIService.ShowPauseWindow(
    onResume: ResumeFromPause,
    onRestart: RequestRestart,
    onSettings: null,
    onQuit: RequestMainMenu

        );
    }

    public void ResumeFromPause()
    {
        _uIService.HidePauseWindow();
        GameStateMachine.ChangeGameState(GameStateMachine.PlayingState);
    }

    public void RequestRestart()
    {
        _uIService.HidePauseWindow();
        _uIService.HideResultsUI();
        Time.timeScale = 1f;
        _navigation.ReloadCurrentLevel();
    }

    public void RequestMainMenu()
    {
        _uIService.HidePauseWindow();
        _uIService.HideResultsUI();
        Time.timeScale = 1f;
        _navigation.GoToMainMenu();
    }
    public void RequestNextLevel()
    {
        _uIService.HidePauseWindow();
        _uIService.HideResultsUI();
        Time.timeScale = 1f;
        _navigation.GoToNextLevel();
    }
    private void OnLevelFail()
    {
        // Transition to ending state
        GameStateMachine.ChangeGameState(GameStateMachine.EndingState);
        _uIService.SetResultsHandlers(
onRestart: RequestRestart,
onMainMenu: RequestMainMenu,
onNextLevel: RequestNextLevel
    );
        // Fire event for UI systems
        EventBus.Get<GameEndEvent>().Invoke(new GameEndData { isVictory = false });
    }

    private void OnLevelCompleted(LevelCompletionData completionData)
    {
        // Transition to ending state  
        GameStateMachine.ChangeGameState(GameStateMachine.EndingState);
        _uIService.SetResultsHandlers(
    onRestart: RequestRestart,
    onMainMenu: RequestMainMenu,
    onNextLevel: RequestNextLevel
            );
        // Fire event for UI systems
        var gameEndData = new GameEndData
        {
            isVictory = true,
            completionData = completionData
        };

        EventBus.Get<GameEndEvent>().Invoke(gameEndData);
    }
    public void Tick()
    {
        GameStateMachine.Tick();
    }

    public void Dispose()
    {
        EventBus.Get<LevelFailEvent>().Unsubscribe(OnLevelFail);
        EventBus.Get<LevelCompletedEvent>().Unsubscribe(OnLevelCompleted);
    }

}
