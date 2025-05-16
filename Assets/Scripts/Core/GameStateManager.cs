
using Assets.Scripts.Utils;
using NUnit.Framework.Constraints;
using System;
using Zenject;

public class GameStateManager : IInitializable,ITickable,IDisposable
{
    private readonly Player.Factory _playerFactory;
    public readonly PlayingState PlayingSate;
    public readonly GameOverState GameOverSate;
    public readonly PauseGameState PausedGameState;

    private IGameState _currentGameState;

    private GameOverUI _gameOverUI;

    [Inject]
    public GameStateManager(Player.Factory playerFactory,GameOverUI gameOverUI)
    {
        _playerFactory = playerFactory;
        _gameOverUI = gameOverUI;
        PlayingSate = new PlayingState(this);
        GameOverSate = new GameOverState(this, _gameOverUI);
        PausedGameState = new PauseGameState(this);
    }
    public void Initialize()
    {
        _playerFactory.Create();
        ChangeGameState(PlayingSate);
    }

    public void Tick()
    {
        _currentGameState?.Update();
    }
    public void Dispose()
    {
        _currentGameState?.Exit();
    }

    public void ChangeGameState(IGameState newState)
    {
        _currentGameState?.Exit();
        _currentGameState = newState;
        _currentGameState.Enter();

    }

}
