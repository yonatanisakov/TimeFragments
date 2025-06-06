using UnityEngine;

public interface IPowerUpDrop
{
    void TryDropPowerUp(Vector3 position, LevelConfig.LootTable lootTable);
}