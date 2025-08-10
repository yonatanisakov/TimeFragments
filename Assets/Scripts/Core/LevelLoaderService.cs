using UnityEngine;

/// <summary>
/// Service for loading level configurations based on current selection
/// Handles the business logic of determining which level to load
/// </summary>
public class LevelLoaderService : ILevelLoaderService
{
    private readonly ISceneService _sceneService;
    private readonly WorldData[] _allWorlds;

    public LevelLoaderService(ISceneService sceneService, WorldData[] allWorlds)
    {
        _sceneService = sceneService;
        _allWorlds = allWorlds;
    }

    public LevelConfig GetCurrentLevelConfig()
    {
        WorldData currentWorld = GetCurrentWorldData();
        if (currentWorld == null)
        {
            Debug.LogError("No valid world data found!");
            return null;
        }

        int selectedLevelIndex = _sceneService.GetSelectedLevelIndex();

        // Validate level index
        if (selectedLevelIndex < 0 || selectedLevelIndex >= currentWorld.GetLevelCount())
        {
            Debug.LogError($"Invalid level index {selectedLevelIndex} for world. Using level 0 as fallback.");
            selectedLevelIndex = 0;
        }

        // Get the level
        LevelConfig level = currentWorld.GetLevel(selectedLevelIndex);
        if (level != null)
        {
            Debug.Log($"Loading level {selectedLevelIndex}: {level.levelName}");
            return level;
        }

        Debug.LogError("Could not load level config!");
        return null;
    }

    public WorldData GetCurrentWorldData()
    {
        int selectedWorldIndex = _sceneService.GetSelectedWorldIndex();

        // Validate world index
        if (_allWorlds == null || selectedWorldIndex < 0 || selectedWorldIndex >= _allWorlds.Length)
        {
            Debug.LogError($"Invalid world index {selectedWorldIndex}. Using world 0 as fallback.");
            selectedWorldIndex = 0;
        }

        if (_allWorlds != null && selectedWorldIndex < _allWorlds.Length)
        {
            WorldData world = _allWorlds[selectedWorldIndex];
            if (world != null)
            {
                Debug.Log($"Loading world {selectedWorldIndex}: {world.worldName}");
                return world;
            }
        }

        Debug.LogError("No valid world data available!");
        return null;
    }
}