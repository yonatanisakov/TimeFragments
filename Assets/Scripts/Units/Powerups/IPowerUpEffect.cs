/// <summary>
/// Interface for individual power-up effects
/// </summary>
public interface IPowerUpEffect
{
    /// <summary>
    /// Type of this power-up
    /// </summary>
    PowerUpType PowerUpType { get; }

    /// <summary>
    /// Duration in seconds (0 for instant or until-condition effects)
    /// </summary>
    float Duration { get; }

    /// <summary>
    /// Time remaining for this effect
    /// </summary>
    float RemainingTime { get; }

    /// <summary>
    /// Whether this effect is currently active
    /// </summary>
    bool IsActive { get; }

    /// <summary>
    /// Whether this power-up can stack with others of the same type
    /// </summary>
    bool CanStack { get; }

    /// <summary>
    /// Activate the power-up effect
    /// </summary>
    void Activate();

    /// <summary>
    /// Deactivate the power-up effect
    /// </summary>
    void Deactivate();

    /// <summary>
    /// Update the power-up effect (called every frame while active)
    /// </summary>
    /// <param name="deltaTime">Time since last update</param>
    void Update(float deltaTime);
    void RefreshDuration();
}