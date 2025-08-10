using Unity.VisualScripting;
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
    private IFragmentCollisionHandler _fragmentCollisionHandler;
    private IFragmentTimeScaleService _fragmentTimeScaleService;

    private float _baseBounceVelocity;
    private float _originalGravityScale = 0.8f;
    private float _currentScale = 1f;
    public Rigidbody2D RigiBody { get; private set; }
    public float TargetBounceVelocity { get; private set; }
    public Vector2 PreviousVelocity { get; private set; }
    public Collision2D LastCollision { get; private set; }
    public int SplitDepth { get; private set; }
    public float Radius { get; private set; }

    public float HorizontalKick => _horizontalKick;
    public float UpwardKick => _upwardKick;

    [Inject]
    public void Construct(IFragmentPhysics fragmentPhysics, IFragmentCollisionHandler collisionHandler, IFragmentTimeScaleService fragmentTimeScaleService)
    {
        _fragmentPhysics = fragmentPhysics;
        _fragmentCollisionHandler = collisionHandler;
        _fragmentTimeScaleService = fragmentTimeScaleService;

        RigiBody = GetComponent<Rigidbody2D>();

    }


    void FixedUpdate()
    {
        PreviousVelocity = RigiBody.linearVelocity;
        _fragmentPhysics.ApplyBoundsPhysics(this);
    }
    public void Configure(LevelConfig.FragmentRecipe fragmentRecipe)
    {
        EventBus.Get<TimeScaleChangedEvent>().Subscribe(OnTimeScaleChanged);

        SplitDepth = fragmentRecipe.splitDepth;
        Radius = fragmentRecipe.radius;
        transform.localScale = new Vector3(Radius, Radius, 0);
        TargetBounceVelocity = _fragmentPhysics.CalculateBounceVelocity(_baseHeight, Radius, _sizeFactor);
        _baseBounceVelocity = TargetBounceVelocity;
        _currentScale = 1f;
        if (_fragmentTimeScaleService.IsSlowMotionActive)
        {
            ApplyTimeScale(_fragmentTimeScaleService.CurrentFragmentTimeScale);

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        LastCollision = collision;
        _fragmentCollisionHandler.OnFragmentCollision(this, collision.gameObject);
    }
    private void ApplyTimeScale(float newScale)
    {
        if (Mathf.Approximately(newScale, _currentScale)) return;
        float rel = newScale / _currentScale;
        RigiBody.linearVelocity *= rel;            // k·v
        RigiBody.gravityScale = _originalGravityScale * newScale * newScale; // k²·g
        TargetBounceVelocity = _baseBounceVelocity * newScale;              // k·v₀
        _currentScale = newScale;
    }

    private void OnTimeScaleChanged(TimeScaleData d) =>
            ApplyTimeScale(d.isApply ? d.fragmentTimeScale : 1f);

    private void OnDestroy()
    {
        EventBus.Get<TimeScaleChangedEvent>().Unsubscribe(OnTimeScaleChanged);
    }

    public class Pool : MonoMemoryPool<LevelConfig.FragmentRecipe, TimeFragment>
    {
        protected override void Reinitialize(LevelConfig.FragmentRecipe fragRecipe, TimeFragment item)
        {
            item.Configure(fragRecipe);
        }
        protected override void OnDespawned(TimeFragment item)
        {   
            EventBus.Get<TimeScaleChangedEvent>().Unsubscribe(item.OnTimeScaleChanged);
            base.OnDespawned(item);

        }

    }
}
