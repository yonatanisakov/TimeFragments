using Assets.Scripts.Utils;
using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private Bullet bullerPrefab;
    [SerializeField] private Player playerPrefab;
    private int bulletPoolSize = 5;
    public override void InstallBindings()
    {
        Container.Bind<IPlayerInput>().To<KeyboardPlayerInput>().FromComponentInHierarchy().AsSingle();
        Container.Bind<IBoundsService>().To<ScreenBoundsService>().AsSingle();
        Container.BindFactory<Player,Player.Factory>().FromComponentInNewPrefab(playerPrefab).AsSingle();
        Container.BindInterfacesAndSelfTo<GameManager>().AsSingle();
        Container.BindMemoryPool<Bullet, Bullet.Pool>().WithInitialSize(bulletPoolSize).FromComponentInNewPrefab(bullerPrefab).UnderTransformGroup("Bullets");

    }
}