using EventBusScripts;
using UnityEngine;

/// <summary>
/// Magnet power-up - pulls nearby power-ups toward player
/// </summary>
public class MagnetPowerUpEffect : BasePowerUpEffect
{
    private const float MAGNET_FORCE = 5f;
    private const float MAGNET_RANGE = 3f;

    public override PowerUpType PowerUpType => PowerUpType.Magnet;
    public override float Duration => 5f;
    public override bool CanStack => false;

    protected override void OnActivate()
    {
        // Notify systems that magnet is active
        EventBus.Get<MagnetActivatedEvent>().Invoke(new MagnetData
        {
            force = MAGNET_FORCE,
            range = MAGNET_RANGE
        });
    }

    protected override void OnDeactivate()
    {
        // Notify systems that magnet is inactive
        EventBus.Get<MagnetDeactivatedEvent>().Invoke();
    }

    protected override void OnUpdate(float deltaTime)
    {
        // Update magnet effect every frame
        EventBus.Get<MagnetUpdateEvent>().Invoke(new MagnetData
        {
            force = MAGNET_FORCE,
            range = MAGNET_RANGE
        });
    }
}