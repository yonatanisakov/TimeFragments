
using UnityEngine;
using Zenject;

public class BulletCollisionHandler : IBulletCollisionHandler
{
    private Bullet.Pool _bulletPool;

    [Inject]
    public BulletCollisionHandler(Bullet.Pool bulletPool)
    {
        _bulletPool = bulletPool;
    }
    public void OnBulletCollision(Bullet bullet,GameObject collisionObject)
    {
        if (collisionObject.CompareTag("Wall"))
             _bulletPool.Despawn(bullet);
    }
}