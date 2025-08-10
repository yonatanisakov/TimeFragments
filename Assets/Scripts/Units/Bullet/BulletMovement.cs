
using UnityEngine;
using Zenject;

public class BulletMovement : IBulletMovement
{
    private IBoundsService _boundsService;
    private Bullet.Pool _bulletPool;

    [Inject]
    public BulletMovement(IBoundsService boundsService, Bullet.Pool bulletPool)
    {
        _boundsService = boundsService;
        _bulletPool = bulletPool;
    }

    public void Move(Bullet bullet,Transform bulletTransform, float bulletSpeed, float bulletHalfHeight)
    {
        bulletTransform.Translate(Vector3.up * bulletSpeed * Time.deltaTime);
        if (bulletTransform.position.y >= _boundsService.maxY + bulletHalfHeight)
        {
            _bulletPool.Despawn(bullet);
        }
    }
}
