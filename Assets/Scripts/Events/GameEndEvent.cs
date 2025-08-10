using EventBusScripts;

public struct GameEndData
{
    public bool isVictory;
    public LevelCompletionData completionData; // Only filled for victory
}

public class GameEndEvent : Event<GameEndData>
{
}