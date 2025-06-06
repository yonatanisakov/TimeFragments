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
        EventBus.Get<LevelFailEvent>().Subscribe(OnLevelFail);
    }
    public void Update()
    {
    }

    public void Exit()
    {
        EventBus.Get<LevelFailEvent>().Unsubscribe(OnLevelFail);
    }



    private void OnLevelFail()
    {
        _gameController.GameStateMachine.ChangeGameState(_gameController.GameStateMachine.GameOverState);
    }
}