using UnityEngine;

/// <summary>
/// Abstract base class for power-up effects
/// Handles common functionality like timing and state management
/// </summary>
public abstract class BasePowerUpEffect : IPowerUpEffect
{
    protected float _remainingTime;
    protected bool _isActive;

    public abstract PowerUpType PowerUpType { get; }
    public abstract float Duration { get; }
    public abstract bool CanStack { get; }

    public float RemainingTime => _remainingTime;
    public bool IsActive => _isActive;

    public virtual void Activate()
    {
        _isActive = true;
        _remainingTime = Duration;
        OnActivate();

        Debug.Log($"Power-up activated: {PowerUpTypeHelper.GetDisplayName(PowerUpType)} for {Duration}s");
    }

    public virtual void Deactivate()
    {
        if (_isActive)
        {
            OnDeactivate();
            _isActive = false;
            _remainingTime = 0f;

            Debug.Log($"Power-up deactivated: {PowerUpTypeHelper.GetDisplayName(PowerUpType)}");
        }
    }

    public virtual void Update(float deltaTime)
    {
        if (!_isActive) return;

        // Handle timed effects
        if (Duration > 0f)
        {
            _remainingTime -= deltaTime;

            if (_remainingTime <= 0f)
            {
                Deactivate();
                return;
            }
        }

        OnUpdate(deltaTime);
    }
    public virtual void RefreshDuration()   // keeps the effect but resets its timer
    {
        _remainingTime = Duration;
    }
    /// <summary>
    /// Override this to implement power-up activation logic
    /// </summary>
    protected abstract void OnActivate();

    /// <summary>
    /// Override this to implement power-up deactivation logic
    /// </summary>
    protected abstract void OnDeactivate();

    /// <summary>
    /// Override this to implement per-frame power-up logic
    /// </summary>
    protected virtual void OnUpdate(float deltaTime) { }
}