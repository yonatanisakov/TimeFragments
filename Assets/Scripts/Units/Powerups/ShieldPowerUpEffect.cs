using EventBusScripts;
using UnityEngine;

/// <summary>
/// Shield power-up - blocks one hit from fragments
/// </summary>
public class ShieldPowerUpEffect : BasePowerUpEffect
{
    public override PowerUpType PowerUpType => PowerUpType.Shield;
    public override float Duration => 0f; // Until hit - managed by hit detection
    public override bool CanStack => false;

    protected override void OnActivate()
    {
        // Subscribe to player hit events to consume shield
        EventBus.Get<ShieldConsumedEvent>().Subscribe(OnShieldBlocked);

        // Fire event to notify UI/player systems
        EventBus.Get<ShieldActivatedEvent>().Invoke();
    }

    protected override void OnDeactivate()
    {
        // Unsubscribe from events
        EventBus.Get<ShieldConsumedEvent>().Unsubscribe(OnShieldBlocked);

        // Fire event to notify UI/player systems
        EventBus.Get<ShieldDeactivatedEvent>().Invoke();
    }

    private void OnShieldBlocked()
    {
        // Shield blocks the hit - deactivate shield
        Deactivate();
        Debug.Log("Shield consumed after blocking hit!");
    }
}