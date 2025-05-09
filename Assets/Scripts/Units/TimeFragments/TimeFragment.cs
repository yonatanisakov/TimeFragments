using EventBusScripts;
using System;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using EventBus = EventBusScripts.EventBus;
using Random = UnityEngine.Random;

public class TimeFragment : MonoBehaviour
{
    private const int SPLIT_SIZE = 2;

    private IBoundsService boundsService;
    private int splitDepth;
    private float radius;
    private TimeFragment.Pool fragPool;
    private Rigidbody2D rb;
    private float velocityBounce;
    [SerializeField] float baseHeight;
    [SerializeField] float sizeFactor;
    [SerializeField] float horizontalKick;
    [SerializeField] float upwardKick;

    [Inject]
    public void Construct(IBoundsService boundsService, TimeFragment.Pool fragPool)
    {
        this.boundsService = boundsService;
        this.fragPool = fragPool;
        rb = GetComponent<Rigidbody2D>();
        
    }



    void FixedUpdate()
    {
        var v = rb.linearVelocity;

        if (transform.position.x <= boundsService.minX + radius / 2)
            v.x = Mathf.Abs(v.x);

        if (transform.position.x >= boundsService.maxX - radius / 2)
            v.x = -Mathf.Abs(v.x);

        rb.linearVelocity = v;
    }
    public void Configure(LevelConfig.FragmentRecipe fragmentRecipe)
    {
        splitDepth = fragmentRecipe.splitDepth;
        radius = fragmentRecipe.radius;
        transform.localScale = new Vector3(radius, radius, 0);
        velocityBounce = CalculateFragmentBounceVelocity();
    }
    private float CalculateFragmentBounceVelocity()
    {
        float g = Mathf.Abs(Physics2D.gravity.y);
        float targetH = baseHeight + radius * sizeFactor;
        return Mathf.Sqrt(2f * targetH * g);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            PopFragment();
            collision.gameObject.SetActive(false);
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (rb.linearVelocity.y < velocityBounce)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, velocityBounce);
            }
        }
    }
    private void PopFragment()
    {

        EventBus.Get<FragmentPoppedEvent>().Invoke(transform.position);

        if (splitDepth > 0)
        {

            for (int i = 0; i < SPLIT_SIZE; i++)
            {
                SpawnChild(i == 0 ? -1 : 1);
            }
        }
        fragPool.Despawn(this);
    }

    private void SpawnChild(int direction)
    {
        var child = fragPool.Spawn(new LevelConfig.FragmentRecipe
        {
            splitDepth = this.splitDepth - 1,
            radius = this.radius - 0.5f,
        });
        child.transform.position = transform.position;
        child.rb.AddForce(new Vector2(direction * horizontalKick, upwardKick + radius), ForceMode2D.Impulse);
    }

    public class Pool : MonoMemoryPool<LevelConfig.FragmentRecipe, TimeFragment>
    {
        protected override void Reinitialize(LevelConfig.FragmentRecipe fragRecipe, TimeFragment item)
        {
            item.Configure(fragRecipe);
        }
    }
}
