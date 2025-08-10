using System.Collections.Generic;
using UnityEngine;

public class ProgressionService : IProgressionService
{
    // Base points for fragment sizes
    private const int LARGE_FRAGMENT_POINTS = 100;   // 1.5 radius
    private const int MEDIUM_FRAGMENT_POINTS = 200;  // 1.0 radius  
    private const int SMALL_FRAGMENT_POINTS = 400;   // 0.5 radius
    private const int TINY_FRAGMENT_POINTS = 800;    // 0.25 radius

    // Fragment radius thresholds
    private const float LARGE_RADIUS_THRESHOLD = 1.5f;
    private const float MEDIUM_RADIUS_THRESHOLD = 1.0f;
    private const float SMALL_RADIUS_THRESHOLD = 0.5f;

    // Bonus calculations
    private const int TIME_BONUS_MULTIPLIER = 10;
    private const int MAX_ACCURACY_BONUS = 500;
    private const int PERFECT_BONUS = 500;

    public int CalculateStarRating(int finalScore, LevelConfig levelConfig)
    {
        int maxScore = CalculateMaxPossibleScore(levelConfig);
        int twoStarThreshold = GetTwoStarThreshold(levelConfig);
        int threeStarThreshold = GetThreeStarThreshold(levelConfig);

        if (finalScore >= threeStarThreshold) return 3;
        if (finalScore >= twoStarThreshold) return 2;
        return 1; // Completing the level always gives at least 1 star
    }
    public int GetTwoStarThreshold(LevelConfig levelConfig)
    {
        if (levelConfig.twoStarThreshold > 0)
            return levelConfig.twoStarThreshold;

        return Mathf.RoundToInt(CalculateMaxPossibleScore(levelConfig) * 0.7f); // 70%
    }

    public int GetThreeStarThreshold(LevelConfig levelConfig)
    {
        if (levelConfig.threeStarThreshold > 0)
            return levelConfig.threeStarThreshold;

        return Mathf.RoundToInt(CalculateMaxPossibleScore(levelConfig) * 0.9f); // 90%
    }

    public int CalculateMaxPossibleScore(LevelConfig levelConfig)
    {
        if (levelConfig.maxPossibleScore > 0)
            return levelConfig.maxPossibleScore;

        int totalComboScore = 0;

        // Calculate weighted combo score for each fragment spawn
        foreach (var fragmentSpawn in levelConfig.fragments)
        {
            totalComboScore += CalculateWeightedComboScore(fragmentSpawn.recipe);
        }

        // Add maximum possible bonuses
        int maxTimeBonus = Mathf.RoundToInt(levelConfig.timeLimit * TIME_BONUS_MULTIPLIER);
        int maxAccuracyBonus = MAX_ACCURACY_BONUS;
        int maxPerfectBonus = PERFECT_BONUS;

        int totalMax = totalComboScore + maxTimeBonus + maxAccuracyBonus + maxPerfectBonus;

        return totalMax;
    }

    private int CalculateWeightedComboScore(LevelConfig.FragmentRecipe recipe)
    {
        // Get all fragments that will be created in destruction order
        var fragments = GetAllFragmentsInDestructionOrder(recipe);

        float totalWeightedScore = 0f;
        float totalBaseScore = 0f;


        for (int i = 0; i < fragments.Count; i++)
        {
            // Combo multipliers: 1st=1x, 2nd=1.5x, 3rd=2x, 4th+=2.5x
            float multiplier = (i + 1) switch
            {
                1 => 1.0f,
                2 => 1.5f,
                3 => 2.0f,
                _ => 2.5f
            };

            float fragmentScore = fragments[i].basePoints;
            float weightedScore = fragmentScore * multiplier;

            totalWeightedScore += weightedScore;
            totalBaseScore += fragmentScore;
        }

        float overallMultiplier = totalWeightedScore / totalBaseScore;
        int finalComboScore = Mathf.RoundToInt(totalWeightedScore);

        return finalComboScore;
    }

    private List<FragmentData> GetAllFragmentsInDestructionOrder(LevelConfig.FragmentRecipe recipe)
    {
        var fragments = new List<FragmentData>();

        // Build fragments level by level (larger to smaller = destruction order)
        float currentRadius = recipe.radius;
        int currentSplitDepth = recipe.splitDepth;

        while (currentSplitDepth >= 0)
        {
            int fragmentsAtThisLevel = (int)Mathf.Pow(2, recipe.splitDepth - currentSplitDepth);
            int pointsPerFragment = GetPointsForRadius(currentRadius);

            // Add all fragments at this level
            for (int i = 0; i < fragmentsAtThisLevel; i++)
            {
                fragments.Add(new FragmentData
                {
                    radius = currentRadius,
                    basePoints = pointsPerFragment
                });
            }

            if (currentSplitDepth == 0) break;

            currentRadius -= 0.5f; // Each split reduces radius by 0.5
            currentSplitDepth--;
        }

        return fragments;
    }

    // helper struct for fragment data
    private struct FragmentData
    {
        public float radius;
        public int basePoints;
    }

    private int GetPointsForRadius(float radius)
    {
        if (radius >= LARGE_RADIUS_THRESHOLD) return LARGE_FRAGMENT_POINTS;
        if (radius >= MEDIUM_RADIUS_THRESHOLD) return MEDIUM_FRAGMENT_POINTS;
        if (radius >= SMALL_RADIUS_THRESHOLD) return SMALL_FRAGMENT_POINTS;
        return TINY_FRAGMENT_POINTS;
    }

    public void SaveLevelCompletion(int levelIndex, int stars, int score)
    {
        // Only save stars if they're better than current
        int currentStars = PlayerPrefs.GetInt($"Level_{levelIndex}_Stars", 0);
        if (stars > currentStars)
        {
            PlayerPrefs.SetInt($"Level_{levelIndex}_Stars", stars);
            Debug.Log($"New best stars for level {levelIndex}: {stars} (was {currentStars})");
        }

        // Only save score if it's higher than current best
        int currentBestScore = PlayerPrefs.GetInt($"Level_{levelIndex}_HighScore", 0);
        if (score > currentBestScore)
        {
            PlayerPrefs.SetInt($"Level_{levelIndex}_HighScore", score);
            Debug.Log($"New high score for level {levelIndex}: {score} (was {currentBestScore})");
        }

        // Update unlocked levels (unlock next level if completing the currently highest unlocked level)
        int currentUnlocked = PlayerPrefs.GetInt("UnlockedLevels", 1);
        if (levelIndex == currentUnlocked - 1) // If completing the last unlocked level
        {
            int newUnlockedCount = levelIndex + 2; // Unlock the next level
            PlayerPrefs.SetInt("UnlockedLevels", newUnlockedCount);
            Debug.Log($"Completed level {levelIndex}, unlocked level {levelIndex + 1} (total unlocked: {newUnlockedCount})");
        }

        PlayerPrefs.Save();
    }

    public bool IsWorldUnlocked(int worldNumber)
    {
        // TODO: Implement based on star collection requirements
        // World 1: Always available
        // World 2: Complete all World 1 + earn 15/30 stars  
        // World 3: Complete all World 2 + earn 20/30 stars

        return worldNumber == 1; // For now, only World 1 is available
    }

    public int GetUnlockedLevels()
    {
        return PlayerPrefs.GetInt("UnlockedLevels", 1); // Start with level 1 unlocked
    }

    public int GetLevelStars(int levelIndex)
    {
        return PlayerPrefs.GetInt($"Level_{levelIndex}_Stars", 0);
    }

    public int GetLevelBestScore(int levelIndex)
    {
        return PlayerPrefs.GetInt($"Level_{levelIndex}_HighScore", 0);
    }
}