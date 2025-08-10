/// <summary>
/// Scene Service interface for managing scene transitions
/// Keeps it simple and focused on what we actually need
/// </summary>
public interface ISceneService
{
    /// <summary>
    /// Load the gameplay scene with a specific level
    /// </summary>
    /// <param name="levelIndex">Index of the level to load</param>
    void LoadGameplayScene(int levelIndex,int worldIndex=0);

    /// <summary>
    /// Load the main menu scene
    /// </summary>
    void LoadMainMenuScene();

    /// <summary>
    /// Get the currently selected level index (set before scene transition)
    /// </summary>
    int GetSelectedLevelIndex();
    /// <summary>
    /// Get the currently selected world index
    /// </summary>
    int GetSelectedWorldIndex();
}