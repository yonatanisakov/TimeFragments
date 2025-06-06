using UnityEngine;
using Zenject;
using EventBus = EventBusScripts.EventBus;

public class TimeFragment : MonoBehaviour
{
    [SerializeField] private float _baseHeight;
    [SerializeField] private float _sizeFactor;
    [SerializeField] private float _horizontalKick;
    [SerializeField] private float _upwardKick;


    private IFragmentPhysics _fragmentPhysics;
    private IFragmentSplitter _fragmentSplitter;
    private Bullet.Pool _bulletPool;
    private Rigidbody2D _rb;

    private float _targetBounceVelocity;
    private int _splitDepth;
    private float _radius;

    [Inject]
    public void Construct(IFragmentPhysics fragmentPhysics, IFragmentSplitter fragmentSplitter,Bullet.Pool bulletPool)
    {
        _fragmentPhysics = fragmentPhysics;
        _fragmentSplitter = fragmentSplitter;
        _bulletPool = bulletPool;
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        _fragmentPhysics.HandleBoundsCollision(transform, _rb, _radius);
    }
    public void Configure(LevelConfig.FragmentRecipe fragmentRecipe)
    {
        _splitDepth = fragmentRecipe.splitDepth;
        _radius = fragmentRecipe.radius;
        transform.localScale = new Vector3(_radius, _radius, 0);
        _targetBounceVelocity = _fragmentPhysics.CalculateBounceVelocity(_baseHeight, _radius, _sizeFactor);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Bullet collidedBullet = collision.gameObject.GetComponent<Bullet>();
            _bulletPool.Despawn(collidedBullet);
            HandleBulletHit();
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            _fragmentPhysics.HandleGroundBounce(_rb, _targetBounceVelocity);
        }
    }
    private void HandleBulletHit()
    {

        EventBus.Get<FragmentPoppedEvent>().Invoke(transform.position);
        var recipe = new LevelConfig.FragmentRecipe
        {
            splitDepth = _splitDepth,
            radius = _radius,
        };
        _fragmentSplitter.SplitFragment(this, recipe, transform.position, _horizontalKick, _upwardKick);
    }

    public class Pool : MonoMemoryPool<LevelConfig.FragmentRecipe, TimeFragment>
    {
        protected override void Reinitialize(LevelConfig.FragmentRecipe fragRecipe, TimeFragment item)
        {
            item.Configure(fragRecipe);
        }
    }
}
