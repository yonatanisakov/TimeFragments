using UnityEngine;

public class GameOverState : IGameState
{
    private readonly GameStateManager _gameStateManager;
    private readonly GameOverUI _gameOverUI;
    public GameOverState(GameStateManager gameStateManager, GameOverUI gameOverUI)
    {
        _gameStateManager = gameStateManager;
        _gameOverUI = gameOverUI;
    }

    public void Enter()
    {
        Time.timeScale = 0f;
        _gameOverUI.Show();
    }

    public void Exit()
    {
        Time.timeScale = 1f;
        _gameOverUI.Hide();
    }

    public void Update()
    {
    
    }
}
