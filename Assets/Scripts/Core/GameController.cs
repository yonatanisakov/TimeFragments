
using EventBusScripts;
using System;
using UnityEngine.SceneManagement;
using Zenject;

public class GameController : IGameController, ITickable
{
    public  IGameStateMachine GameStateMachine { get; private set; }

    private readonly IUIService _uIService;

    [Inject]
    public GameController(IPlayerSpawner playerSpawner, IUIService uIService)
    {
        GameStateMachine = new GameStateMachine(playerSpawner, this);
        _uIService = uIService;
    }
    public void HandleGameOver()
    {
        _uIService.ShowGameOverUI(()=>HandleRestartGame());
    }

    public void HandleRestartGame()
    {
        _uIService.HideGameOverUI();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Tick()
    {
        GameStateMachine.Tick();
    }
}
