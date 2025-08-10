using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using IInitializable = Zenject.IInitializable;
using EventBus = EventBusScripts.EventBus;
using System;
public class LevelManager : ILevelManager,IDisposable
{
    private readonly LevelConfig _levelConfig;
    private readonly IFragmentSpawner _fragmentSpawner;
    private readonly ILevelInitializer _levelInitializer;
    private readonly IPowerUpDrop _powerUpDrop;
    private readonly IUIService _uiService;
    private readonly IScoreService _scoreService;
    private readonly IStatisticsService _statisticsService;
    private readonly IProgressionService _progressionService;
    private int _fragmentsRemaining;
    private float _timeLeft;

    [Inject]
    public LevelManager(IUIService uiService, LevelConfig levelConfig, IFragmentSpawner fragmentSpawner, ILevelInitializer levelInitializer,
        IPowerUpDrop powerUpDrop, IScoreService scoreService, IStatisticsService statisticsService, IProgressionService progressionService)
    {
        _levelConfig = levelConfig;
        _fragmentSpawner = fragmentSpawner;
        _levelInitializer = levelInitializer;
        _powerUpDrop = powerUpDrop;
        _uiService = uiService;
        _scoreService = scoreService;
        _statisticsService = statisticsService;
        _progressionService = progressionService;
    }
    public void Initialize()
    {
        _timeLeft = _levelConfig.timeLimit;
        _levelInitializer.InitializeLevel(_levelConfig);

        _uiService.InitTimer(_timeLeft);
        _uiService.InitScore();

        // Initialize score service and connect to UI updates
        _scoreService.Initialize();
        _statisticsService.Initialize();

        EventBus.Get<ScoreUpdatedEvent>().Subscribe(OnScoreUpdated);
        _fragmentsRemaining = 0;
        foreach (var fragmentConfig in _levelConfig.fragments)
        {
            _fragmentSpawner.SpawnFragment(fragmentConfig);
            _fragmentsRemaining += _fragmentSpawner.CalculateTotalFragments(fragmentConfig.recipe.splitDepth);
        }
        EventBus.Get<FragmentPoppedEvent>().Subscribe(OnFragmentPopped);
    }
    public void Tick()
    {
        if (_levelConfig.timeLimit > 0)
        {
            _timeLeft -= Time.deltaTime;

            _uiService.UpdateTimer(_timeLeft);
            if (_timeLeft <= 0 && _fragmentsRemaining > 0)
            {
                EventBus.Get<LevelFailEvent>().Invoke();
            }
        }
    }

    void OnFragmentPopped(FragmentHitData hitData)
    {
        _fragmentsRemaining--;
        _powerUpDrop.TryDropPowerUp(hitData.position, _levelConfig.powerUpDrops);

        _scoreService.AddFragmentScore(hitData);

        if (_fragmentsRemaining == 0)
        {
            CalculateEndOfLevelBonuses();
        }
    }

    private void CalculateEndOfLevelBonuses()
    {
        // Calculate all bonuses
        int timeBonus = Mathf.RoundToInt(_timeLeft * 10f);
        int accuracyBonus = _statisticsService.CalculateAccuracyBonus();
        int perfectBonus = _statisticsService.CalculatePerfectBonus();

        // Add bonuses to current score for final score
        int finalScore = _scoreService.CurrentScore + timeBonus + accuracyBonus + perfectBonus;

        // Calculate star rating
        int starRating = _progressionService.CalculateStarRating(finalScore, _levelConfig);

        // Save progression
        _progressionService.SaveLevelCompletion(_levelConfig.levelIndex, starRating, finalScore);

        // Create completion data
        var completionData = new LevelCompletionData
        {
            finalScore = finalScore,
            starRating = starRating,
            timeBonus = timeBonus,
            accuracyBonus = accuracyBonus,
            perfectBonus = perfectBonus,
            accuracyPercentage = _statisticsService.AccuracyPercentage,
            isPerfectRun = _statisticsService.IsPerfectRun,
            timeRemaining = _timeLeft,
            bulletsFired = _statisticsService.BulletsFired,
            bulletsHit = _statisticsService.BulletsHit
        };

        // Fire completion event for UI to display results
        EventBus.Get<LevelCompletedEvent>().Invoke(completionData);

        // Debug log for testing
        Debug.Log($"Level Complete! Final Score: {finalScore} ({starRating} stars)");
        Debug.Log($"Bonuses - Time: {timeBonus}, Accuracy: {accuracyBonus}, Perfect: {perfectBonus}");
        Debug.Log($"Star Thresholds - 2★: {_progressionService.GetTwoStarThreshold(_levelConfig)}, 3★: {_progressionService.GetThreeStarThreshold(_levelConfig)}");
    }
    private void OnScoreUpdated(int newScore)
    {
        _uiService.UpdateScore(newScore);
    }
    public void Dispose()
    {
        EventBus.Get<FragmentPoppedEvent>().Unsubscribe(OnFragmentPopped);
        EventBus.Get<ScoreUpdatedEvent>().Unsubscribe(OnScoreUpdated);
    }
}
