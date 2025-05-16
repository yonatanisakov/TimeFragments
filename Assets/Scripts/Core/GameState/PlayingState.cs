using EventBusScripts;
using System;
using UnityEngine;

public class PlayingState : IGameState
{
    private readonly GameStateManager _gameStateManager;

    public PlayingState(GameStateManager gameStateManager)
    {
        _gameStateManager = gameStateManager;
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
        _gameStateManager.ChangeGameState(_gameStateManager.GameOverSate);
    }
}