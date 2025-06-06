
using UnityEngine;
using Zenject;

public class Bullet : MonoBehaviour
{
    IBulletMovement _bulletMovement;
    [SerializeField] private float _bulletSpeed = 5f;
    private float _bulletHalfHeight;

    [Inject]
    public void Construct(IBulletMovement bulletMovement)
    {
        _bulletMovement = bulletMovement;
        _bulletHalfHeight = GetComponentInChildren<SpriteRenderer>().bounds.size.y / 2;
    }

    private void Update()
    {
        _bulletMovement.Move(this,transform,_bulletSpeed, _bulletHalfHeight);
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
