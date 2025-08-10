/// <summary>
/// Service interface for handling game navigation logic
/// Manages scene transitions and level progression
/// </summary>
public interface IGameNavigationService
{
    /// <summary>
    /// Navigate to main menu from gameplay
    /// </summary>
    void GoToMainMenu();

    /// <summary>
    /// Navigate to next level (handles progression logic)
    /// </summary>
    void GoToNextLevel();

}