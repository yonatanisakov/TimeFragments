using Assets.Scripts.Utils;
using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;

public class GameInstaller : MonoInstaller
{
    [SerializeField] Player _playerPrefab;
    public override void InstallBindings()
    {
        Container.Bind<IPlayerInput>().To<KeyboardPlayerInput>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IBoundsService>().To<ScreenBoundsService>().AsSingle();
        Container.InstantiatePrefab( _playerPrefab );
    }
}