/// <summary>
/// Service interface for loading level configurations
/// Handles the business logic of determining which level to load
/// </summary>
public interface ILevelLoaderService
{
    /// <summary>
    /// Get the LevelConfig for the currently selected level
    /// </summary>
    LevelConfig GetCurrentLevelConfig();

    /// <summary>
    /// Get the WorldData for the currently selected world
    /// </summary>
    WorldData GetCurrentWorldData();
}