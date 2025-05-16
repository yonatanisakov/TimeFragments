using UnityEngine;

public class PauseGameState : IGameState
{
    private readonly GameStateManager _gameStateManager;

    public PauseGameState(GameStateManager gameStateManager)
    {
        _gameStateManager = gameStateManager;
    }
    public void Enter()
    {
        Time.timeScale = 0f;
    }

    public void Exit()
    {
        Time.timeScale = 1f;
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {


        }
    }
}
