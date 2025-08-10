
using UnityEngine;
using Zenject;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 5f;
    private IBoundsService bounds;
    private float bulletHalfHeight;
    Pool pool;

    [Inject]
    public void Construct(IBoundsService bounds, Bullet.Pool pool)
    {
        this.bounds = bounds;
        bulletHalfHeight = GetComponentInChildren<SpriteRenderer>().bounds.size.y / 2;
        this.pool = pool;

    }

    private void Update()
    {
        transform.Translate(Vector3.up * bulletSpeed * Time.deltaTime);

        if (transform.position.y>=bounds.maxY+bulletHalfHeight)
            pool.Despawn(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // future logic for collision with time fragments
        //if (collision.CompareTag("TimeFragment"))
        //{
        //    
        //}
    }

    public void ResetPosition(Vector3 pos,Quaternion rot)
    {
    transform.SetPositionAndRotation(pos, rot); 
    }

    public class Pool: MonoMemoryPool<Vector3,Quaternion, Bullet> {
        protected override void Reinitialize(Vector3 pos,Quaternion rot, Bullet bullet)
        {
            bullet.ResetPosition(pos,rot);
        }
    }

}
