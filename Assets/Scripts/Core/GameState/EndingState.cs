using UnityEngine;

public class EndingState : IGameState
{
    private readonly IGameController _gameController;


    public EndingState(IGameController gameController)
    {
        _gameController = gameController;
    }

    public void Enter()
    {
        Time.timeScale = 0f;
    }

    public void Exit()
    {
    }

    public void Update()
    {
    
    }
}
