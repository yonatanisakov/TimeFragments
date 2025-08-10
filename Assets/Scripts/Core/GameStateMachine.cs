


using System;
using UnityEngine;
using Zenject;

public class GameStateMachine : IGameStateMachine
{
    public PlayingState PlayingState { get; private set; }
    public EndingState EndingState { get; private set; }
    public PauseGameState PauseGameState { get; private set; }
    public IGameState CurrentState { get; private set; }
    private IPlayerSpawner _playerSpawner;
    private readonly IGameController _gameController;


    public GameStateMachine( IPlayerSpawner playerSpawner,IGameController gameController)
    {
        _playerSpawner = playerSpawner;
        _gameController = gameController;

        PlayingState = new PlayingState(gameController);
        EndingState = new EndingState(gameController);
        PauseGameState = new PauseGameState(gameController);
        Initialize();
    }
    public void Initialize()
    {
        _playerSpawner.SpawnPlayer();
        ChangeGameState(PlayingState);
    }

    public void Tick()
    {
        CurrentState?.Update();
    }

    public void ChangeGameState(IGameState newState)
    {
        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState.Enter();

    }

}
