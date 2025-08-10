
using UnityEngine;

public interface IBulletCollisionHandler
{
    public void OnBulletCollision(Bullet bullet, GameObject collisionObject);
}