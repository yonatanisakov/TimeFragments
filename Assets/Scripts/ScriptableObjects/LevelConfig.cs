using UnityEngine;

[CreateAssetMenu(fileName = "New Level", menuName = "Time Fragments Game/Level")]
public class LevelConfig : ScriptableObject
{
    [Header("Meta & Presentation")]
    public string levelName = "Stage 1";
    [Tooltip("Index of this level in the world (0-based: 0, 1, 2, 3, 4...)")]
    public int levelIndex = 0;  // ADD THIS LINE
    public Sprite background;
    public AudioClip music;
    public float timeLimit = 0f; // 0 -> no timer

    [Header("Star Rating System")]
    [Tooltip("Maximum possible score for this level (calculated automatically if 0)")]
    public int maxPossibleScore = 0;
    [Tooltip("Score needed for 2 stars (70% of max if 0)")]
    public int twoStarThreshold = 0;
    [Tooltip("Score needed for 3 stars (90% of max if 0)")]
    public int threeStarThreshold = 0;

    [Header("Fragment Setup")]
    public FragmentSpawn[] fragments;

    [Header("World Geometry")]
    public WallPieceSpawn[] staticWalls;
    public DynamicHazardSpawn[] hazards;

    [Header("PowerUps")]
    public LootTable powerUpDrops;

    [System.Serializable]
    public struct FragmentSpawn
    {
        public Vector3 position;
        public FragmentRecipe recipe; // size,split-depth
    }

    [System.Serializable]
    public struct FragmentRecipe
    {
        public TimeFragment prefab;
        [Min(0.1f)] public float radius; // 0.5,1.0,1.5...
        [Range(0, 10)] public int splitDepth; // 0 -> no split
    }

    [System.Serializable]
    public struct WallPieceSpawn
    {
        public GameObject prefab; // any static collide tile, WILL REPLACE WITH SPECIFIC WallPiece GameObject
        public Vector3 position;
        public Vector3 scale;
    }
    [System.Serializable]
    public struct DynamicHazardSpawn
    {
        public GameObject prefab; // crasher,saw,gate... WILL REPLACE WITH SPECIFIC HAZARD GameObject
        public Vector3 position;
      //  public HazardParams parameters; // speed,delay,movement implement in future..
    }
    [System.Serializable]
    public struct LootTable
    {
        public GameObject prefab; // pooled powerup WILL REPLACE WITH SPECIFIC POWERUP GameObject
        [Range(0, 1f)] public float dropChanceEach; // per fragment pop
    }
}
