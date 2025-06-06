
public interface IGameController
{
    IGameStateMachine GameStateMachine { get; }
    void HandleRestartGame();
    void HandleGameOver();
}