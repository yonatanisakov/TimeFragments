using EventBusScripts;
using UnityEngine;

public class StatisticsService : IStatisticsService
{
    public int BulletsFired { get; private set; }
    public int BulletsHit { get; private set; }
    public int DamageTaken { get; private set; }
    
    public float AccuracyPercentage => BulletsFired > 0 ? (float)BulletsHit / BulletsFired * 100f : 0f;
    public bool IsPerfectRun => DamageTaken == 0;
    
    public void Initialize()
    {
        ResetStatistics();
        
        // Subscribe to relevant events
        EventBus.Get<BulletHitFragmentEvent>().Subscribe(OnBulletHit);
        EventBus.Get<PlayerGetHitEvent>().Subscribe(OnPlayerDamaged);
    }
    
    public void ResetStatistics()
    {
        BulletsFired = 0;
        BulletsHit = 0;
        DamageTaken = 0;
    }
    
    public void OnBulletFired()
    {
        BulletsFired++;
    }
    
    public void OnBulletHit()
    {
        BulletsHit++;
    }
    
    public void OnPlayerDamaged()
    {
        DamageTaken++;
    }
    
    public int CalculateAccuracyBonus()
    {
        float accuracy = AccuracyPercentage;
        
        if (accuracy >= 90f) return 500;
        if (accuracy >= 80f) return 300;
        if (accuracy >= 70f) return 100;
        
        return 0;
    }
    
    public int CalculatePerfectBonus()
    {
        return IsPerfectRun ? 500 : 0;
    }
}