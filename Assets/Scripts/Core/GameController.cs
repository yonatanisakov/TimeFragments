
using EventBusScripts;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameController : IGameController, ITickable, IDisposable
{
    public  IGameStateMachine GameStateMachine { get; private set; }
    private readonly IUIService _uIService;

    [Inject]
    public GameController(IPlayerSpawner playerSpawner, IUIService uIService)
    {
        EventBus.Get<LevelFailEvent>().Subscribe(OnLevelFail);
        EventBus.Get<LevelCompletedEvent>().Subscribe(OnLevelCompleted);
        EventBus.Get<RestartGameEvent>().Subscribe(HandleRestartGame);

        _uIService = uIService;
        GameStateMachine = new GameStateMachine(playerSpawner, this);
    }
    private void OnLevelFail()
    {
        // Transition to ending state
        GameStateMachine.ChangeGameState(GameStateMachine.EndingState);

        // Fire event for UI systems
        EventBus.Get<GameEndEvent>().Invoke(new GameEndData { isVictory = false });
    }

    private void OnLevelCompleted(LevelCompletionData completionData)
    {
        // Transition to ending state  
        GameStateMachine.ChangeGameState(GameStateMachine.EndingState);

        // Fire event for UI systems
        var gameEndData = new GameEndData
        {
            isVictory = true,
            completionData = completionData
        };

        EventBus.Get<GameEndEvent>().Invoke(gameEndData);
    }
    public void HandleRestartGame()
    {
        _uIService.HideResultsUI();
        UnityEngine.Time.timeScale = 1f; // Ensure time is resumed before scene reload
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Tick()
    {
        GameStateMachine.Tick();
    }

    public void Dispose()
    {
        EventBus.Get<LevelFailEvent>().Unsubscribe(OnLevelFail);
        EventBus.Get<LevelCompletedEvent>().Unsubscribe(OnLevelCompleted);
        EventBus.Get<RestartGameEvent>().Unsubscribe(HandleRestartGame);

    }

}
