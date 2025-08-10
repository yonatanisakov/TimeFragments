
using UnityEngine;
using Zenject;

public class Bullet : MonoBehaviour
{
    private IBulletMovement _bulletMovement;
    private IBulletCollisionHandler _bulletCollisionHandler;

    [SerializeField] private float _bulletSpeed = 5f;
    private float _bulletHalfHeight;

    [Inject]
    public void Construct(IBulletMovement bulletMovement, IBulletCollisionHandler bulletCollisionHandler)
    {
        _bulletMovement = bulletMovement;
        _bulletHalfHeight = GetComponentInChildren<SpriteRenderer>().bounds.size.y / 2;
        _bulletCollisionHandler = bulletCollisionHandler;
    }

    private void Update()
    {
        _bulletMovement.Move(this,transform,_bulletSpeed, _bulletHalfHeight);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _bulletCollisionHandler.OnBulletCollision(this, collision.gameObject);
    }

    public void ResetPosition(Vector3 pos, Quaternion rot)
    {
        transform.SetPositionAndRotation(pos, rot);
    }

    public class Pool : MonoMemoryPool<Vector3, Quaternion, Bullet>
    {
        protected override void Reinitialize(Vector3 pos, Quaternion rot, Bullet bullet)
        {
            bullet.ResetPosition(pos, rot);
        }
    }

}
