
public interface IGameController
{
    IGameStateMachine GameStateMachine { get; }
    void RequestPause();
    void ResumeFromPause();
    void RequestRestart();     
    void RequestMainMenu();
    void RequestNextLevel();



}