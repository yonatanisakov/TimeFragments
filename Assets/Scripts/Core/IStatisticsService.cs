public interface IStatisticsService
{
    // Current session stats
    int BulletsFired { get; }
    int BulletsHit { get; }
    int DamageTaken { get; }
    float AccuracyPercentage { get; }
    bool IsPerfectRun { get; }

    // Methods
    void Initialize();
    void ResetStatistics();
    void OnBulletFired();
    void OnBulletHit();
    void OnPlayerDamaged();

    // End-of-level calculations
    int CalculateAccuracyBonus();
    int CalculatePerfectBonus();
}