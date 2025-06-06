using UnityEngine;

public class GameOverState : IGameState
{
    private readonly IGameController _gameController;


    public GameOverState(IGameController gameController)
    {
        _gameController = gameController;
    }

    public void Enter()
    {
        Time.timeScale = 0f;
        _gameController.HandleGameOver();
    }

    public void Exit()
    {
        Time.timeScale = 1f;
    }

    public void Update()
    {
    
    }
}
