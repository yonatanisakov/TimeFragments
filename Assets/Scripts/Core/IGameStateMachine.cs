
public interface IGameStateMachine
{
    PlayingState PlayingState { get; }
    GameOverState GameOverState { get; }
    PauseGameState PauseGameState { get; }
    void ChangeGameState(IGameState gameState);
    void Tick();

}