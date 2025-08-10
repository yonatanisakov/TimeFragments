
using UnityEngine;

public class LevelInitializer : ILevelInitializer
{
    private readonly IGameObjectFactory _gameObjectFactory;

    public LevelInitializer(IGameObjectFactory gameObjectFactory)
    {
        _gameObjectFactory = gameObjectFactory;
    }

    public void InitializeLevel(LevelConfig levelConfig)
    {
        if (levelConfig.background)
            _gameObjectFactory.Create(levelConfig.background,Vector3.zero,Quaternion.identity);

        if (levelConfig.music)
            AudioSource.PlayClipAtPoint(levelConfig.music, Vector3.zero);

        foreach(var wallConfig in levelConfig.staticWalls)
            _gameObjectFactory.Create(wallConfig.prefab,wallConfig.position,Quaternion.identity,wallConfig.scale);

        foreach (var hazardConfig in levelConfig.hazards)
            _gameObjectFactory.Create(hazardConfig.prefab, hazardConfig.position, Quaternion.identity);
        
    }
}
