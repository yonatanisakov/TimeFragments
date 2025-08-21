using Assets.Scripts.Utils;
using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Bullet bullerPrefab;
    [SerializeField] private Player playerPrefab;
    [SerializeField] private TimeFragment timeFragmentPrefab;
    [SerializeField] private WorldData[] _allWorlds;
    [SerializeField] private PowerUpConfig _powerUpConfig;

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
        Container.BindInterfacesAndSelfTo<ScoreService>().AsSingle();
        Container.Bind<IStatisticsService>().To<StatisticsService>().AsSingle();
        Container.Bind<IProgressionService>().To<ProgressionService>().AsSingle();
        Container.BindInterfacesTo<PlayerMovement>().AsTransient();
        Container.Bind<IPlayerWeapon>().To<PlayerWeapon>().AsTransient();
        Container.BindFactory<Player, Player.Factory>().FromComponentInNewPrefab(playerPrefab).AsSingle();
        Container.BindMemoryPool<Bullet, Bullet.Pool>().WithInitialSize(bulletPoolSize).FromComponentInNewPrefab(bullerPrefab).UnderTransformGroup("Bullets");
        Container.BindMemoryPool<TimeFragment, TimeFragment.Pool>().WithInitialSize(fragmentPoolSize).FromComponentInNewPrefab(timeFragmentPrefab).UnderTransformGroup("TimeFragments");
        Container.BindInterfacesTo<LevelManager>().AsSingle().NonLazy();
        Container.BindInterfacesTo<DiscretePlayerHealthManager>().AsSingle().NonLazy();
        Container.Bind<IFragmentPhysics>().To<FragmentPhysics>().AsSingle();
        Container.Bind<IFragmentSplitter>().To<FragmentSplitter>().AsSingle();
        Container.Bind<IUIService>().To<UIService>().AsSingle();
        Container.BindInterfacesAndSelfTo<ResultsUI>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<BottomHudUI>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<PauseWindowUI>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<ComboPopupWidget>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IBulletMovement>().To<BulletMovement>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameController>().AsSingle().NonLazy();
        Container.Bind<IBulletCollisionHandler>().To<BulletCollisionHandler>().AsSingle();
        Container.Bind<IFragmentCollisionHandler>().To<FragmentCollisionHandler>().AsSingle();
        Container.Bind<ISceneService>().To<SceneService>().AsSingle();
        Container.BindInterfacesAndSelfTo<GameNavigationService>().AsSingle().NonLazy();
        Container.BindInterfacesTo<PowerUpService>().AsSingle().NonLazy();
        Container.Bind<PowerUpConfig>().FromInstance(_powerUpConfig).AsSingle();
        Container.BindInterfacesTo<FragmentTimeScaleService>().AsSingle().NonLazy();
        Container.BindInterfacesTo<PowerUpLifetimeManager>().AsSingle().NonLazy();
        Container.BindInterfacesTo<ComboPopupPresenter>().AsSingle();
        Container.Bind<IAudioService>().To<AudioService>().AsSingle();

        // Bind LevelLoaderService with WorldData array
        Container.Bind<ILevelLoaderService>().To<LevelLoaderService>().AsSingle()
            .WithArguments(_allWorlds); // Pass the world data array as constructor argument

        // Bind LevelConfig lazily - will be resolved when first requested
        Container.Bind<LevelConfig>().FromMethod(GetCurrentLevelConfig).AsSingle();

    }
    /// <summary>
    /// Lazy factory method for getting current level config
    /// Called by Zenject when LevelConfig is first requested
    /// </summary>
    private LevelConfig GetCurrentLevelConfig(InjectContext context)
    {
        var levelLoaderService = context.Container.Resolve<ILevelLoaderService>();
        return levelLoaderService.GetCurrentLevelConfig();
    }
}