
using EventBusScripts;
using UnityEngine;
using Zenject;
/// <summary>
/// Slow Motion power-up - slows down all fragments
/// </summary>
public class SlowMotionPowerUpEffect : BasePowerUpEffect
{
    private const float SLOW_FACTOR = 0.5f; // 50% speed

    public override PowerUpType PowerUpType => PowerUpType.SlowMotion;
    public override float Duration => 5f;
    public override bool CanStack => false;

    protected override void OnActivate()
    {
        // Apply slow motion to all fragments
        EventBus.Get<TimeScaleChangedEvent>().Invoke(new TimeScaleData
        {
            fragmentTimeScale = SLOW_FACTOR,
            isApply = true
        });
    }

    protected override void OnDeactivate()
    {
        // Remove slow motion effect
        EventBus.Get<TimeScaleChangedEvent>().Invoke(new TimeScaleData
        {
            fragmentTimeScale = 1f,
            isApply = false
        });
    }
}