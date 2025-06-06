using UnityEngine;

public class PowerUpDrop : IPowerUpDrop
{

    private readonly IGameObjectFactory _gameObjectFactory;

    public PowerUpDrop(IGameObjectFactory gameObjectFactory)
    {
        _gameObjectFactory = gameObjectFactory;
    }

    public void TryDropPowerUp(Vector3 position, LevelConfig.LootTable lootTable)
    {
        if(!lootTable.prefab) return;

        if(Random.value <= lootTable.dropChanceEach)
            _gameObjectFactory.Create(lootTable.prefab,position,Quaternion.identity);
    }
}
