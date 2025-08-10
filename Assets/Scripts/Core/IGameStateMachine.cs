
public interface IGameStateMachine
{
    PlayingState PlayingState { get; }
    EndingState EndingState { get; }
    PauseGameState PauseGameState { get; }

    IGameState CurrentState { get; }
    void ChangeGameState(IGameState gameState);
    void Tick();

}