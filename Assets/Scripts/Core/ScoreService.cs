using EventBusScripts;
using System;
using UnityEngine;

public class ScoreService : IScoreService
{
    private const float COMBO_WINDOW = 2f;

    public int CurrentScore { get; private set; }
    public float CurrentMultiplier { get; private set; }

    private float _lastHitTime;
    private int _comboCount;

    public void Initialize()
    {
        ResetScore();
    }

    public void ResetScore()
    {
        CurrentScore = 0;
        CurrentMultiplier = 1f;
        _lastHitTime = 0f;
        _comboCount = 0;

        EventBus.Get<ScoreUpdatedEvent>().Invoke(CurrentScore);
        EventBus.Get<ComboMultiplierChangedEvent>().Invoke(CurrentMultiplier);
    }

    public void AddFragmentScore(FragmentHitData hitData)
    {
        UpdateComboMultiplier();

        int finalScore = Mathf.RoundToInt(hitData.basePoints * CurrentMultiplier);
        CurrentScore += finalScore;

        _lastHitTime = UnityEngine.Time.time;

        EventBus.Get<ScoreUpdatedEvent>().Invoke(CurrentScore);
        EventBus.Get<ComboMultiplierChangedEvent>().Invoke(CurrentMultiplier);
    }

    private void UpdateComboMultiplier()
    {
        float timeSinceLastHit = UnityEngine.Time.time - _lastHitTime;

        if (timeSinceLastHit <= COMBO_WINDOW)
        {
            // Continue combo
            _comboCount++;
        }
        else
        {
            // Reset combo
            _comboCount = 1;
        }

        // Calculate multiplier based on combo count
        CurrentMultiplier = _comboCount switch
        {
            1 => 1f,     // First hit
            2 => 1.5f,   // Second hit
            3 => 2f,     // Third hit
            _ => 2.5f    // Fourth+ hits (max multiplier)
        };
    }
}