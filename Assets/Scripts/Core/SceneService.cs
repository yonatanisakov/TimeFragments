using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Scene Service implementation for handling scene transitions
/// Simple and reliable scene management
/// </summary>
public class SceneService : ISceneService
{
    // Scene name constants to avoid magic strings
    private const string MAIN_MENU_SCENE = "MainMenuScene";
    private const string GAMEPLAY_SCENE = "GameplayScene";

    // Selected level data that survives scene transitions
    private const string SELECTED_LEVEL_KEY = "SelectedLevelIndex";
    private const string SELECTED_WORLD_KEY = "SelectedWorldIndex";

    public void LoadGameplayScene(int levelIndex, int worldIndex = 0)
    {

        // Save both world and level indices
        PlayerPrefs.SetInt(SELECTED_WORLD_KEY, worldIndex);
        PlayerPrefs.SetInt(SELECTED_LEVEL_KEY, levelIndex);
        PlayerPrefs.Save();

        Debug.Log($"Loading GameplayScene with world {worldIndex}, level {levelIndex}");

        // Load the gameplay scene
        SceneManager.LoadScene(GAMEPLAY_SCENE);
    }

    public void LoadMainMenuScene()
    {
        Debug.Log("Loading MainMenuScene");
        SceneManager.LoadScene(MAIN_MENU_SCENE);
    }

    public int GetSelectedLevelIndex()
    {
        return PlayerPrefs.GetInt(SELECTED_LEVEL_KEY, 0); // Default to level 0
    }
    public int GetSelectedWorldIndex()
    {
        return PlayerPrefs.GetInt(SELECTED_WORLD_KEY, 0); // Default to world 0
    }
}