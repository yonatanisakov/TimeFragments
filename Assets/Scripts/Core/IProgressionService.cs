public interface IProgressionService
{
    // Star rating calculation
    int CalculateStarRating(int finalScore, LevelConfig levelConfig);
    int CalculateMaxPossibleScore(LevelConfig levelConfig);

    // Score threshold calculations
    int GetTwoStarThreshold(LevelConfig levelConfig);
    int GetThreeStarThreshold(LevelConfig levelConfig);

    // Level progression (for future implementation)
    void SaveLevelCompletion(int levelIndex, int stars, int score);
    bool IsWorldUnlocked(int worldNumber);
    int GetUnlockedLevels();
    int GetLevelStars(int levelIndex);
    int GetLevelBestScore(int levelIndex);
}