using System;
using System.Collections.Generic;

/// <summary>
/// Service interface for managing active power-ups and their effects
/// </summary>
public interface IPowerUpService
{
    /// <summary>
    /// All currently active power-ups
    /// </summary>
    IReadOnlyList<IPowerUpEffect> ActivePowerUps { get; }

    /// <summary>
    /// Activate a power-up effect
    /// </summary>
    /// <param name="powerUpType">Type of power-up to activate</param>
    void ActivatePowerUp(PowerUpType powerUpType);

    /// <summary>
    /// Check if a specific power-up type is currently active
    /// </summary>
    /// <param name="powerUpType">Type to check</param>
    /// <returns>True if at least one power-up of this type is active</returns>
    bool HasActivePowerUp(PowerUpType powerUpType);

    /// <summary>
    /// Remove all active power-ups (useful for level restart/game over)
    /// </summary>
    void ClearAllPowerUps();

    /// <summary>
    /// Event fired when a power-up effect is activated
    /// </summary>
    event Action<IPowerUpEffect> OnPowerUpActivated;

    /// <summary>
    /// Event fired when a power-up effect expires
    /// </summary>
    event Action<IPowerUpEffect> OnPowerUpExpired;
}