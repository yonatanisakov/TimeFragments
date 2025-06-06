


using System;
using UnityEngine;
using Zenject;

public class GameStateMachine : IGameStateMachine
{
    public PlayingState PlayingState { get; private set; }
    public GameOverState GameOverState { get; private set; }
    public PauseGameState PauseGameState { get; private set; }

    private IGameState _currentGameState;
    private IPlayerSpawner _playerSpawner;


    public GameStateMachine( IPlayerSpawner playerSpawner,IGameController gameController)
    {
        PlayingState = new PlayingState(gameController);
        GameOverState = new GameOverState(gameController);
        PauseGameState = new PauseGameState(gameController);
        _playerSpawner = playerSpawner;
        Initialize();
    }
    public void Initialize()
    {
        _playerSpawner.SpawnPlayer();
        ChangeGameState(PlayingState);
    }

    public void Tick()
    {
        _currentGameState?.Update();
    }

    public void ChangeGameState(IGameState newState)
    {
        _currentGameState?.Exit();
        _currentGameState = newState;
        _currentGameState.Enter();

    }

}
