using UnityEngine;

/// <summary>
/// World Data - Contains all levels for one world
/// Includes progression requirements for world unlocking
/// </summary>
[CreateAssetMenu(fileName = "New World", menuName = "Time Fragments Game/World Data")]
public class WorldData : ScriptableObject
{
    [Header("World Information")]
    public string worldName = "Time Fragments";
    public Sprite worldIcon;
    public Color worldColor = Color.blue;

    [Header("World Progression")]
    [Tooltip("How many stars needed to unlock this world (0 = always unlocked)")]
    public int starsRequiredToUnlock = 0;
    [Tooltip("Short description shown when world is locked")]
    public string unlockDescription = "Complete previous world to unlock";

    [Header("Levels in this World")]
    [Tooltip("All levels in this world, in order")]
    public LevelConfig[] levels;

    /// <summary>
    /// Get total number of levels in this world
    /// </summary>
    public int GetLevelCount()
    {
        return levels?.Length ?? 0;
    }

    /// <summary>
    /// Get level config by index (safe)
    /// </summary>
    public LevelConfig GetLevel(int index)
    {
        if (levels == null || index < 0 || index >= levels.Length)
            return null;

        return levels[index];
    }

    /// <summary>
    /// Get level name by index (safe)
    /// </summary>
    public string GetLevelName(int index)
    {
        var level = GetLevel(index);
        return level?.levelName ?? $"Level {index + 1}";
    }

    /// <summary>
    /// Check if level exists
    /// </summary>
    public bool HasLevel(int index)
    {
        return GetLevel(index) != null;
    }

    /// <summary>
    /// Check if this world is unlocked based on player's total stars
    /// </summary>
    public bool IsUnlocked(int playerTotalStars)
    {
        return playerTotalStars >= starsRequiredToUnlock;
    }

    /// <summary>
    /// Get the maximum possible stars from this world
    /// </summary>
    public int GetMaxPossibleStars()
    {
        return GetLevelCount() * 3; // 3 stars per level
    }
}