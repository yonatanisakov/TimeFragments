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
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _gameController.GameStateMachine.ChangeGameState(_gameController.GameStateMachine.PlayingState);
        }
    }
}
