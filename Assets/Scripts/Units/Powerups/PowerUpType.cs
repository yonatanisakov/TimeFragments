/// <summary>
/// Enumeration of all power-up types based on Game Design Document
/// Includes both beneficial power-ups and cursed (bad) power-ups
/// </summary>
public enum PowerUpType
{
    // === GOOD POWER-UPS (Beneficial Effects) ===

    /// <summary>
    /// Blocks 1 hit from a fragment (Until hit)
    /// </summary>
    Shield,

    /// <summary>
    /// Slows down all fragments (5s duration)
    /// </summary>
    SlowMotion,

    /// <summary>
    /// Pulls nearby power-ups (5s duration)
    /// </summary>
    Magnet,

    /// <summary>
    /// Destroys all small fragments instantly (Instant effect)
    /// </summary>
    QuantumPulse,

    // === BAD POWER-UPS (Cursed/Negative Effects) ===

    /// <summary>
    /// Left = Right, Right = Left (5s duration)
    /// </summary>
    InvertedControls,

    /// <summary>
    /// Locks time-based ability button (6s duration)
    /// </summary>
    CooldownLock,

    /// <summary>
    /// Player slows down, fragments speed up (4s duration)
    /// </summary>
    TimeGlitch,

    /// <summary>
    /// Duplicates all visible fragments instantly (Instant effect)
    /// </summary>
    FragmentClone,

    /// <summary>
    /// Fragments turn invisible randomly (6s duration)
    /// </summary>
    InvisibleFragments,

    /// <summary>
    /// Fragments bounce faster + with more force (5s duration)
    /// </summary>
    BounceBoost,

    /// <summary>
    /// Random screen shakes and noise effects (3s duration)
    /// </summary>
    GlitchFlash
}

/// <summary>
/// Helper class for PowerUpType categorization and utilities
/// </summary>
public static class PowerUpTypeHelper
{
    /// <summary>
    /// Check if a power-up type is beneficial (good) or cursed (bad)
    /// </summary>
    /// <param name="powerUpType">The power-up type to check</param>
    /// <returns>True if beneficial, false if cursed</returns>
    public static bool IsGoodPowerUp(PowerUpType powerUpType)
    {
        return powerUpType switch
        {
            // Good power-ups
            PowerUpType.Shield => true,
            PowerUpType.SlowMotion => true,
            PowerUpType.Magnet => true,
            PowerUpType.QuantumPulse => true,

            // Bad power-ups
            PowerUpType.InvertedControls => false,
            PowerUpType.CooldownLock => false,
            PowerUpType.TimeGlitch => false,
            PowerUpType.FragmentClone => false,
            PowerUpType.InvisibleFragments => false,
            PowerUpType.BounceBoost => false,
            PowerUpType.GlitchFlash => false,

            _ => true // Default to good if unknown
        };
    }

    /// <summary>
    /// Get the duration of a power-up effect in seconds
    /// </summary>
    /// <param name="powerUpType">The power-up type</param>
    /// <returns>Duration in seconds (0 for instant effects)</returns>
    public static float GetDuration(PowerUpType powerUpType)
    {
        return powerUpType switch
        {
            // Good power-ups
            PowerUpType.Shield => 0f, // Until hit - handled specially
            PowerUpType.SlowMotion => 5f,
            PowerUpType.Magnet => 5f,
            PowerUpType.QuantumPulse => 0f, // Instant

            // Bad power-ups
            PowerUpType.InvertedControls => 5f,
            PowerUpType.CooldownLock => 6f,
            PowerUpType.TimeGlitch => 4f,
            PowerUpType.FragmentClone => 0f, // Instant
            PowerUpType.InvisibleFragments => 6f,
            PowerUpType.BounceBoost => 5f,
            PowerUpType.GlitchFlash => 3f,

            _ => 0f
        };
    }

    /// <summary>
    /// Get display name for UI
    /// </summary>
    /// <param name="powerUpType">The power-up type</param>
    /// <returns>Human-readable name</returns>
    public static string GetDisplayName(PowerUpType powerUpType)
    {
        return powerUpType switch
        {
            PowerUpType.Shield => "Shield",
            PowerUpType.SlowMotion => "Slow Motion",
            PowerUpType.Magnet => "Magnet",
            PowerUpType.QuantumPulse => "Quantum Pulse",
            PowerUpType.InvertedControls => "Inverted Controls",
            PowerUpType.CooldownLock => "Cooldown Lock",
            PowerUpType.TimeGlitch => "Time Glitch",
            PowerUpType.FragmentClone => "Fragment Clone",
            PowerUpType.InvisibleFragments => "Invisible Fragments",
            PowerUpType.BounceBoost => "Bounce Boost",
            PowerUpType.GlitchFlash => "Glitch Flash",
            _ => powerUpType.ToString()
        };
    }
}