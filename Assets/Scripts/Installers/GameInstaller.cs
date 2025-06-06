using Assets.Scripts.Utils;
using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Bullet bullerPrefab;
    [SerializeField] private Player playerPrefab;
    [SerializeField] private TimeFragment timeFragmentPrefab;
    [SerializeField] private LevelConfig currentLevel;

    [Header("Configuration")]
    [SerializeField] private int bulletPoolSize = 5;
    [SerializeField] private int fragmentPoolSize = 5;
    public override void InstallBindings()
    {
        Container.Bind<IPlayerInput>().To<KeyboardPlayerInput>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IBoundsService>().To<ScreenBoundsService>().AsSingle();

        Container.Bind<IGameObjectFactory>().To<GameObjectFactory>().AsSingle();
        Container.Bind<IFragmentSpawner>().To<FragmentSpawner>().AsSingle();
        Container.Bind<ILevelInitializer>().To<LevelInitializer>().AsSingle();
        Container.Bind<IPowerUpDrop>().To<PowerUpDrop>().AsSingle();
        Container.Bind<IPlayerSpawner>().To<PlayerSpawner>().AsSingle();
        Container.Bind<IPlayerMovement>().To<PlayerMovement>().AsTransient();
        Container.Bind<IPlayerWeapon>().To<PlayerWeapon>().AsTransient();
        Container.BindFactory<Player, Player.Factory>().FromComponentInNewPrefab(playerPrefab).AsSingle();
        Container.BindMemoryPool<Bullet, Bullet.Pool>().WithInitialSize(bulletPoolSize).FromComponentInNewPrefab(bullerPrefab).UnderTransformGroup("Bullets");
        Container.BindMemoryPool<TimeFragment, TimeFragment.Pool>().WithInitialSize(fragmentPoolSize).FromComponentInNewPrefab(timeFragmentPrefab).UnderTransformGroup("TimeFragments");
        Container.BindInterfacesTo<LevelManager>().AsSingle().NonLazy();
        Container.BindInterfacesTo<DiscretePlayerHealthManager>().AsSingle().NonLazy();
        Container.BindInstance<LevelConfig>(currentLevel).AsSingle();
        Container.Bind<IFragmentPhysics>().To<FragmentPhysics>().AsSingle();
        Container.Bind<IFragmentSplitter>().To<FragmentSplitter>().AsSingle();
        Container.Bind<IUIService>().To<UIService>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameOverUI>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<HudUI>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<IBulletMovement>().To<BulletMovement>().AsSingle();
        Container.Bind<IGameController>().To<GameController>().AsSingle().NonLazy();

    }
}