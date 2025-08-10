/// <summary>
/// Service interface for managing power-up collectible lifetimes
/// Prevents power-ups from staying on the ground forever
/// Follows SOLID principles - Single Responsibility: only handles lifetime tracking
/// Uses only EventBus for communication (no C# events)
/// </summary>
public interface IPowerUpLifetimeManager
{
    /// <summary>
    /// Default lifetime for power-ups in seconds (configurable)
    /// </summary>
    float DefaultLifetime { get; }

    /// <summary>
    /// Register a power-up collectible for lifetime tracking
    /// Called when a power-up is spawned in the world
    /// </summary>
    /// <param name="powerUp">The power-up collectible to track</param>
    /// <param name="customLifetime">Optional custom lifetime, uses default if null</param>
    void RegisterPowerUp(PowerUpCollectible powerUp, float? customLifetime = null);

    /// <summary>
    /// Unregister a power-up (when collected by player or destroyed)
    /// Called when power-up is no longer in the world
    /// </summary>
    /// <param name="powerUp">The power-up to stop tracking</param>
    void UnregisterPowerUp(PowerUpCollectible powerUp);

    /// <summary>
    /// Clear all tracked power-ups (for level restart/cleanup)
    /// Called on level transitions to prevent memory leaks
    /// </summary>
    void ClearAllTrackedPowerUps();

    // REMOVED: C# events - using only EventBus for consistency with your architecture
    // Communication happens via:
    // - PowerUpLifetimeWarningEvent
    // - PowerUpLifetimeBlinkEvent  
    // - PowerUpLifetimeExpiredEvent
}