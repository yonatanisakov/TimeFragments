/// <summary>
/// Interface for Main Menu UI operations
/// Simple interface that defines what the main menu can do
/// </summary>
public interface IMainMenuUI
{
    /// <summary>
    /// Show the main menu panel
    /// </summary>
    void Show();

    /// <summary>
    /// Hide the main menu panel
    /// </summary>
    void Hide();

    /// <summary>
    /// Show the level selection panel
    /// </summary>
    void ShowLevelSelection();

    /// <summary>
    /// Show the settings panel
    /// </summary>
    void ShowSettings();

    /// <summary>
    /// Set the loading state
    /// </summary>
    void SetLoadingState(bool isLoading);
}