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
    private int bulletPoolSize = 5;
    private int fragmentPoolSize = 5;
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<GameStateManager>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<GameOverUI>().FromComponentInHierarchy().AsSingle().NonLazy();                  
        Container.Bind<IPlayerInput>().To<KeyboardPlayerInput>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IBoundsService>().To<ScreenBoundsService>().AsSingle();
        Container.BindFactory<Player, Player.Factory>().FromComponentInNewPrefab(playerPrefab).AsSingle();
        Container.BindMemoryPool<Bullet, Bullet.Pool>().WithInitialSize(bulletPoolSize).FromComponentInNewPrefab(bullerPrefab).UnderTransformGroup("Bullets");
        Container.BindMemoryPool<TimeFragment, TimeFragment.Pool>().WithInitialSize(fragmentPoolSize).FromComponentInNewPrefab(timeFragmentPrefab).UnderTransformGroup("TimeFragments");
        Container.BindInterfacesTo<LevelManager>().AsSingle().NonLazy();
        Container.BindInterfacesTo<DiscretePlayerHealthManager>().AsSingle().NonLazy();
        Container.BindInstance<LevelConfig>(currentLevel).AsSingle();

    }
}