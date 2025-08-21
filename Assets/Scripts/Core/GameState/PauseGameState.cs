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
        _gameController.RequestPause();
    }

    public void Exit()
    {
        Time.timeScale = 1f;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _gameController?.ResumeFromPause();
        }
    }
}
