using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

/// <summary>
/// Service implementation for managing power-ups
/// Handles activation, deactivation, updates, and lifecycle management
/// </summary>
public class PowerUpService : IPowerUpService, ITickable
{
    private readonly List<IPowerUpEffect> _activePowerUps = new List<IPowerUpEffect>();

    public IReadOnlyList<IPowerUpEffect> ActivePowerUps => _activePowerUps.AsReadOnly();

    public event Action<IPowerUpEffect> OnPowerUpActivated;
    public event Action<IPowerUpEffect> OnPowerUpExpired;

    public void ActivatePowerUp(PowerUpType powerUpType)
    {

        var existing = _activePowerUps.FirstOrDefault(p => p.PowerUpType == powerUpType && p.IsActive);

        if (existing != null && !existing.CanStack)
        {
            existing.RefreshDuration();   // just restart its timer
            return;                       // nothing else to do
        }

        var powerUpEffect = CreatePowerUpEffect(powerUpType);
        if (powerUpEffect == null)
        {
            Debug.LogWarning($"Failed to create power-up effect for type: {powerUpType}");
            return;
        }
        // Activate the power-up and add it to active list
        powerUpEffect.Activate();
        _activePowerUps.Add(powerUpEffect);

        OnPowerUpActivated?.Invoke(powerUpEffect);
    }

    public bool HasActivePowerUp(PowerUpType powerUpType)
    {
        return _activePowerUps.Any(p => p.PowerUpType == powerUpType && p.IsActive);
    }

    public void ClearAllPowerUps()
    {
        var powerUpsToRemove = _activePowerUps.ToList();
        foreach (var powerUp in powerUpsToRemove)
        {
            RemovePowerUp(powerUp);
        }
    }

    public void Tick()
    {
        // Update all power-ups in reverse order to safely remove expired ones
        for (int i = _activePowerUps.Count - 1; i >= 0; i--)
        {
            var powerUp = _activePowerUps[i];
            powerUp.Update(Time.deltaTime);

            // Remove power-ups that are no longer active
            if (!powerUp.IsActive)
            {
                RemovePowerUp(powerUp);
            }
        }
    }

    /// <summary>
    /// Remove a specific power-up instance
    /// </summary>
    private void RemovePowerUp(IPowerUpEffect powerUp)
    {
        if (_activePowerUps.Remove(powerUp))
        {
            powerUp.Deactivate();
            OnPowerUpExpired?.Invoke(powerUp);
        }
    }

    /// <summary>
    /// Remove all power-ups of a specific type
    /// </summary>
    private void RemovePowerUpsOfType(PowerUpType powerUpType)
    {
        var powerUpsToRemove = _activePowerUps
            .Where(p => p.PowerUpType == powerUpType)
            .ToList();

        foreach (var powerUp in powerUpsToRemove)
        {
            RemovePowerUp(powerUp);
        }
    }

    /// <summary>
    /// Factory method for creating power-up effects
    /// </summary>
    private IPowerUpEffect CreatePowerUpEffect(PowerUpType powerUpType)
    {
        return powerUpType switch
        {
            PowerUpType.Shield => new ShieldPowerUpEffect(),
            PowerUpType.SlowMotion => new SlowMotionPowerUpEffect(),
            PowerUpType.Magnet => new MagnetPowerUpEffect(),
            PowerUpType.InvertedControls => new InvertedControlsPowerUpEffect(),
            _ => null
        };
    }
}