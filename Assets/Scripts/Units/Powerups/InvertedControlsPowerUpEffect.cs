using EventBusScripts;
using UnityEngine;
/// <summary>
/// Inverted Controls power-up (bad) - inverts player movement
/// </summary>
public class InvertedControlsPowerUpEffect : BasePowerUpEffect
{
    public override PowerUpType PowerUpType => PowerUpType.InvertedControls;
    public override float Duration => 5f;
    public override bool CanStack => false;

    protected override void OnActivate()
    {
        // Invert player controls
        EventBus.Get<ControlsInvertedEvent>().Invoke(true);
    }

    protected override void OnDeactivate()
    {
        // Restore normal controls
        EventBus.Get<ControlsInvertedEvent>().Invoke(false);
    }
}