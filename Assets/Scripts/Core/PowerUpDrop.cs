using UnityEngine;

/// <summary>
/// Updated PowerUpDrop service that spawns PowerUpCollectible objects
/// Integrates with existing LevelConfig.LootTable system
/// </summary>
public class PowerUpDrop : IPowerUpDrop
{
    private readonly IGameObjectFactory _gameObjectFactory;
    private readonly IBoundsService _boundsService;
    private readonly PowerUpConfig _powerUpConfig;
    private IPowerUpLifetimeManager _lifetimeManager;

    public PowerUpDrop(IGameObjectFactory gameObjectFactory, IBoundsService boundsService, PowerUpConfig powerUpConfig, IPowerUpLifetimeManager lifetimeManager)
    {
        _gameObjectFactory = gameObjectFactory;
        _boundsService = boundsService;
        _powerUpConfig = powerUpConfig;
        _lifetimeManager = lifetimeManager;
    }

    public void TryDropPowerUp(Vector3 position, LevelConfig.LootTable lootTable)
    {
        if (!lootTable.prefab) return;

        if (Random.value <= lootTable.dropChanceEach)
        {

            // Spawn the power-up prefab
            var powerUpObject = _gameObjectFactory.Create(lootTable.prefab, position, Quaternion.identity);

            // Configure the power-up if it has a PowerUpCollectible component
            var collectible = powerUpObject.GetComponent<PowerUpCollectible>();
            if (collectible != null)
            {
                // For now, randomly assign a power-up type
                // Later we can make this configurable in the LootTable
                var randomType = GetRandomPowerUpType();
                collectible.Configure(randomType, _powerUpConfig, _lifetimeManager);

                // Calculate safe positioning using effective bounds
                var effectiveHalfHeight = collectible.EffectiveHalfHeight;
                var effectiveHalfWidth = collectible.EffectiveHalfWidth;

                Debug.Log($"Effective bounds - Width: {effectiveHalfWidth}, Height: {effectiveHalfHeight}");

                // Position with proper offsets
                var properY = _boundsService.minY + effectiveHalfHeight;
                var randomX = GetSafeRandomXPosition(effectiveHalfWidth);
                var finalPosition = new Vector3(randomX, properY, position.z);

                powerUpObject.transform.position = finalPosition;

                Debug.Log($"Power-up dropped: {PowerUpTypeHelper.GetDisplayName(randomType)} at {finalPosition}");
            }
            else
            {
                Debug.LogWarning("Spawned power-up prefab doesn't have PowerUpCollectible component!");
            }
        }
    }
    /// <summary>
    /// Get a safe random X position that accounts for rotation and bobbing
    /// </summary>
    /// <param name="effectiveHalfWidth">Half width including rotation and bobbing safety margin</param>
    /// <returns>Safe random X position</returns>
    private float GetSafeRandomXPosition(float effectiveHalfWidth)
    {
        // Calculate safe X range with effective bounds
        var leftEdge = _boundsService.minX + effectiveHalfWidth;
        var rightEdge = _boundsService.maxX - effectiveHalfWidth;

        // Ensure we have a valid range
        if (leftEdge >= rightEdge)
        {
            Debug.LogWarning("Power-up too large for screen! Using screen center.");
            return 0f; // Screen center
        }

        return Random.Range(leftEdge, rightEdge);
    }
    private PowerUpType GetRandomPowerUpType()
    {
        return _powerUpConfig.GetRandomPowerUpType();
    }
}