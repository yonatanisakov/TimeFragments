using EventBusScripts;
using System;
using UnityEngine;

public class PlayingState : IGameState
{
    private readonly IGameController _gameController;

    public PlayingState(IGameController gameController)
    {
        _gameController = gameController;
    }
    public void Enter()
    {
        Time.timeScale = 1.0f;
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _gameController.GameStateMachine.ChangeGameState(_gameController.GameStateMachine.PauseGameState);
        }

    }

    public void Exit()
    {
    }
}