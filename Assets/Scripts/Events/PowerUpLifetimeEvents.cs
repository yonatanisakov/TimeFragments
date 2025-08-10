using EventBusScripts;

/// <summary>
/// Event data structure for power-up lifetime warnings
/// Contains all information needed for visual warning effects
/// </summary>
public struct PowerUpLifetimeWarningData
{
    public PowerUpCollectible powerUp;    // The power-up entering warning state
    public float remainingTime;           // Seconds left before expiry
    public float progress;                // 0.0 to 1.0, where 1.0 is fully expired
}

/// <summary>
/// Event fired when a power-up enters warning state (about to expire)
/// UI systems can subscribe to show red tint, particles, etc.
/// Audio systems can play warning sounds
/// </summary>
public class PowerUpLifetimeWarningEvent : Event<PowerUpLifetimeWarningData>
{
}

/// <summary>
/// Event fired for blinking visual effect during warning period
/// Triggers at regular intervals (every 0.5 seconds by default)
/// PowerUpCollectible can subscribe to toggle visibility/alpha
/// </summary>
public class PowerUpLifetimeBlinkEvent : Event<PowerUpCollectible>
{
}

/// <summary>
/// Event fired when a power-up expires and is about to be destroyed
/// Statistics systems can track missed power-ups
/// Audio systems can play expiry sound effects
/// Particle systems can show "poof" effects
/// </summary>
public class PowerUpLifetimeExpiredEvent : Event<PowerUpCollectible>
{
}