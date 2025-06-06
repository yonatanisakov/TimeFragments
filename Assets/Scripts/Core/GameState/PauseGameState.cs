using UnityEngine;

public class PauseGameState : IGameState
{
    private readonly IGameController _gameController;

    public PauseGameState(IGameController gameController)
    {
        _gameController = gameController;
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
