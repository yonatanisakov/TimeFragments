
using UnityEngine;

public interface IBulletMovement
{
    void Move(Bullet bullet,Transform bulletTransform,float bulletSpeed,float bulletHalfHeight);
}