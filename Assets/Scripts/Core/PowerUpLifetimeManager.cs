using System.Collections.Generic;
using UnityEngine;
using Zenject;
using EventBusScripts;
using System;

/// <summary>
/// Concrete implementation of IPowerUpLifetimeManager
/// Tracks power-up lifetimes with visual warnings and automatic cleanup
/// Uses Zenject interfaces for proper Unity integration
/// CORRECTED: Proper EventBus signatures, performance optimized, no C# events
/// </summary>
public class PowerUpLifetimeManager : IPowerUpLifetimeManager, ITickable, IInitializable, IDisposable
{
    [Header("Lifetime Configuration")]
    [SerializeField] private float _defaultLifetime = 5f; // 8 seconds default
    [SerializeField] private float _warningThreshold = 2f; // Warn when 2 seconds left
    [SerializeField] private float _warningBlinkInterval = 0.5f; // Blink every 0.5 seconds

    // Dictionary to track all power-ups and their lifetime data
    private readonly Dictionary<PowerUpCollectible, PowerUpLifetimeData> _trackedPowerUps =
        new Dictionary<PowerUpCollectible, PowerUpLifetimeData>();

    // PERFORMANCE FIX: Reuse list instead of allocating every frame
    private readonly List<PowerUpCollectible> _powerUpsToExpire = new List<PowerUpCollectible>();

    // Public properties
    public float DefaultLifetime => _defaultLifetime;

    /// <summary>
    /// Internal data structure for tracking individual power-up lifetimes
    /// Encapsulates all timing logic for a single power-up
    /// </summary>
    private class PowerUpLifetimeData
    {
        public float LifetimeRemaining;
        public float TotalLifetime;
        public bool WarningActive;
        public float WarningTimer;

        public PowerUpLifetimeData(float lifetime)
        {
            LifetimeRemaining = lifetime;
            TotalLifetime = lifetime;
            WarningActive = false;
            WarningTimer = 0f;
        }

        // Calculated properties for convenience
        public float LifetimeProgress => 1f - (LifetimeRemaining / TotalLifetime);
        public bool IsNearExpiry(float warningThreshold) => LifetimeRemaining <= warningThreshold;
        public bool IsExpired => LifetimeRemaining <= 0f;
    }

    /// <summary>
    /// Zenject IInitializable - called after dependency injection
    /// Subscribe to game events for automatic cleanup
    /// CORRECTED: Proper event handler signatures based on event types
    /// </summary>
    public void Initialize()
    {
        // Clean up when levels end to prevent memory leaks
        EventBus.Get<LevelCompletedEvent>().Subscribe(OnLevelCompleted);  // Event<LevelCompletionData>
        EventBus.Get<LevelFailEvent>().Subscribe(OnLevelFailed);          // Event (no data)
        EventBus.Get<RestartGameEvent>().Subscribe(OnGameRestart);        // Event (no data)

        Debug.Log($"PowerUpLifetimeManager initialized - Default lifetime: {_defaultLifetime}s");
    }

    /// <summary>
    /// Zenject IDisposable - called when service is destroyed
    /// Clean up all subscriptions and tracked power-ups
    /// </summary>
    public void Dispose()
    {
        // Unsubscribe from events to prevent memory leaks
        EventBus.Get<LevelCompletedEvent>().Unsubscribe(OnLevelCompleted);
        EventBus.Get<LevelFailEvent>().Unsubscribe(OnLevelFailed);
        EventBus.Get<RestartGameEvent>().Unsubscribe(OnGameRestart);

        // Clear all tracked power-ups
        ClearAllTrackedPowerUps();

        Debug.Log("PowerUpLifetimeManager disposed");
    }

    /// <summary>
    /// Zenject ITickable - called every frame
    /// Update all tracked power-ups and handle expiration
    /// PERFORMANCE FIX: Reuse list instead of allocating every frame
    /// </summary>
    public void Tick()
    {
        // PERFORMANCE FIX: Clear existing list instead of allocating new one
        _powerUpsToExpire.Clear();

        // Update all tracked power-ups
        foreach (var kvp in _trackedPowerUps)
        {
            var powerUp = kvp.Key;
            var data = kvp.Value;

            // Skip if power-up GameObject was destroyed externally
            if (powerUp == null || powerUp.gameObject == null)
            {
                _powerUpsToExpire.Add(powerUp);
                continue;
            }

            // Update lifetime countdown
            data.LifetimeRemaining -= Time.deltaTime;

            // Handle warning state entry
            if (data.IsNearExpiry(_warningThreshold) && !data.WarningActive)
            {
                EnterWarningState(powerUp, data);
            }

            // Handle warning visual effects (blinking)
            if (data.WarningActive)
            {
                UpdateWarningEffects(powerUp, data);
            }

            // Check if expired
            if (data.IsExpired)
            {
                _powerUpsToExpire.Add(powerUp);
            }
        }

        // Handle expired power-ups
        foreach (var powerUp in _powerUpsToExpire)
        {
            ExpirePowerUp(powerUp);
        }
    }

    #region Public Interface Implementation

    public void RegisterPowerUp(PowerUpCollectible powerUp, float? customLifetime = null)
    {
        if (powerUp == null)
        {
            Debug.LogWarning("Attempted to register null power-up for lifetime tracking");
            return;
        }

        var lifetime = customLifetime ?? _defaultLifetime;
        var lifetimeData = new PowerUpLifetimeData(lifetime);

        _trackedPowerUps[powerUp] = lifetimeData;

        Debug.Log($"Registered {powerUp.PowerUpType} for {lifetime}s lifetime tracking");
    }

    public void UnregisterPowerUp(PowerUpCollectible powerUp)
    {
        if (powerUp != null && _trackedPowerUps.ContainsKey(powerUp))
        {
            _trackedPowerUps.Remove(powerUp);
            Debug.Log($"Unregistered {powerUp.PowerUpType} from lifetime tracking");
        }
    }

    public void ClearAllTrackedPowerUps()
    {
        var count = _trackedPowerUps.Count;
        _trackedPowerUps.Clear();

        if (count > 0)
        {
            Debug.Log($"Cleared {count} tracked power-ups from lifetime manager");
        }
    }

    #endregion

    #region Private Helper Methods

    /// <summary>
    /// Enter warning state for a power-up (called when near expiry)
    /// Triggers visual warning effects and fires events
    /// CORRECTED: Only EventBus events, no duplicate C# events
    /// </summary>
    private void EnterWarningState(PowerUpCollectible powerUp, PowerUpLifetimeData data)
    {
        data.WarningActive = true;
        data.WarningTimer = 0f;

        // Fire EventBus event for visual warning effects
        EventBus.Get<PowerUpLifetimeWarningEvent>().Invoke(new PowerUpLifetimeWarningData
        {
            powerUp = powerUp,
            remainingTime = data.LifetimeRemaining,
            progress = data.LifetimeProgress
        });

        Debug.Log($"Power-up {powerUp.PowerUpType} entering warning state: {data.LifetimeRemaining:F1}s remaining");
    }

    /// <summary>
    /// Update warning visual effects (blinking animation)
    /// Called every frame while power-up is in warning state
    /// </summary>
    private void UpdateWarningEffects(PowerUpCollectible powerUp, PowerUpLifetimeData data)
    {
        data.WarningTimer += Time.deltaTime;

        // Trigger blink effect at regular intervals
        if (data.WarningTimer >= _warningBlinkInterval)
        {
            data.WarningTimer = 0f;

            // Fire EventBus event for blinking visual effect
            EventBus.Get<PowerUpLifetimeBlinkEvent>().Invoke(powerUp);
        }
    }

    /// <summary>
    /// Expire a power-up (remove from tracking and destroy GameObject)
    /// This is where the actual cleanup happens
    /// CORRECTED: Only EventBus event, no duplicate C# event
    /// </summary>
    private void ExpirePowerUp(PowerUpCollectible powerUp)
    {
        if (powerUp != null && _trackedPowerUps.ContainsKey(powerUp))
        {
            // Remove from tracking dictionary
            _trackedPowerUps.Remove(powerUp);

            // Fire EventBus event for expiry effects
            EventBus.Get<PowerUpLifetimeExpiredEvent>().Invoke(powerUp);

            // Destroy the GameObject if it still exists
            if (powerUp.gameObject != null)
            {
                UnityEngine.Object.Destroy(powerUp.gameObject);
                Debug.Log($"Power-up {powerUp.PowerUpType} expired and destroyed");
            }
        }
    }

    /// <summary>
    /// CORRECTED: Event handler for level completion (has data parameter)
    /// </summary>
    private void OnLevelCompleted(LevelCompletionData eventData)
    {
        Debug.Log($"Level completed (Score: {eventData.finalScore}), cleaning up {_trackedPowerUps.Count} tracked power-ups");
        ClearAllTrackedPowerUps();
    }

    /// <summary>
    /// CORRECTED: Event handler for level failure (no parameters)
    /// </summary>
    private void OnLevelFailed()
    {
        Debug.Log($"Level failed, cleaning up {_trackedPowerUps.Count} tracked power-ups");
        ClearAllTrackedPowerUps();
    }

    /// <summary>
    /// CORRECTED: Event handler for game restart (no parameters)
    /// </summary>
    private void OnGameRestart()
    {
        Debug.Log($"Game restarted, cleaning up {_trackedPowerUps.Count} tracked power-ups");
        ClearAllTrackedPowerUps();
    }

    #endregion
}