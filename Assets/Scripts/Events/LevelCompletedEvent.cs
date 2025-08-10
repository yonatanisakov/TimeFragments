using EventBusScripts;

public struct LevelCompletionData
{
    public int finalScore;
    public int starRating;
    public int timeBonus;
    public int accuracyBonus;
    public int perfectBonus;
    public float accuracyPercentage;
    public bool isPerfectRun;
    public float timeRemaining;
    public int bulletsFired;
    public int bulletsHit;
}

public class LevelCompletedEvent : Event<LevelCompletionData>
{
}