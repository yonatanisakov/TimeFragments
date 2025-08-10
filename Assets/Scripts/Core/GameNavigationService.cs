using UnityEngine;
using EventBusScripts;
using Zenject;
using System;

/// <summary>
/// Service for handling game navigation logic
/// Contains all the business logic for scene transitions and level progression
/// </summary>
public class GameNavigationService : IGameNavigationService, IDisposable
{
    private readonly ISceneService _sceneService;
    private readonly ILevelLoaderService _levelLoaderService;
    private readonly IProgressionService _progressionService;


    public GameNavigationService(
        ISceneService sceneService,
        ILevelLoaderService levelLoaderService,
        IProgressionService progressionService)
    {
        _sceneService = sceneService;
        _levelLoaderService = levelLoaderService;
        _progressionService = progressionService;

        // Subscribe to navigation events
        EventBus.Get<MainMenuRequestedEvent>().Subscribe(OnMainMenuRequested);
        EventBus.Get<NextLevelRequestedEvent>().Subscribe(OnNextLevelRequested);
    }

    public void Dispose()
    {
        // Unsubscribe from events
        EventBus.Get<MainMenuRequestedEvent>().Unsubscribe(OnMainMenuRequested);
        EventBus.Get<NextLevelRequestedEvent>().Unsubscribe(OnNextLevelRequested);

    }    public void GoToMainMenu()
    {
        Debug.Log("Navigating to main menu");
        _sceneService.LoadMainMenuScene();
    }

    public void GoToNextLevel()
    {
        int currentLevel = _sceneService.GetSelectedLevelIndex();
        int nextLevel = currentLevel + 1;

        // Check if next level exists in current world
        WorldData currentWorld = _levelLoaderService.GetCurrentWorldData();
        if (currentWorld != null && nextLevel < currentWorld.GetLevelCount())
        {
            // Check if next level is unlocked
            int unlockedLevels = _progressionService.GetUnlockedLevels();
            Debug.Log($"DEBUG: currentLevel={currentLevel}, nextLevel={nextLevel}, unlockedLevels={unlockedLevels}");

            if (nextLevel < unlockedLevels)
            {
                Debug.Log($"Loading next level: {nextLevel + 1}");
                _sceneService.LoadGameplayScene(nextLevel);
            }
            else
            {
                Debug.Log("Next level is locked. Returning to main menu.");
                GoToMainMenu();
            }
        }
        else
        {
            // No more levels in current world
            Debug.Log("Completed all levels in current world! Returning to main menu.");
            // TODO: In future, this could unlock next world or show world completion screen
            GoToMainMenu();
        }
    }

    private bool HasNextLevel()
    {
        int currentLevel = _sceneService.GetSelectedLevelIndex();
        int nextLevel = currentLevel + 1;

        // Check if next level exists and is unlocked
        WorldData currentWorld = _levelLoaderService.GetCurrentWorldData();
        if (currentWorld != null && nextLevel < currentWorld.GetLevelCount())
        {
            int unlockedLevels = _progressionService.GetUnlockedLevels();
            return nextLevel < unlockedLevels;
        }

        return false;
    }

    // Event handlers
    private void OnMainMenuRequested(object data)
    {
        GoToMainMenu();
    }

    private void OnNextLevelRequested(object data)
    {
        GoToNextLevel();
    }


}