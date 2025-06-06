
using UnityEngine;
using Zenject;

public class FragmentPhysics : IFragmentPhysics
{
    private readonly IBoundsService _boundsService;

    [Inject]
    public FragmentPhysics(IBoundsService boundsService)
    {
        _boundsService = boundsService;
    }
    public float CalculateBounceVelocity(float baseHeight, float radius, float sizeFactor)
    {
        float gravity = Mathf.Abs(Physics2D.gravity.y);
        float targetHeight = baseHeight + radius * sizeFactor;
        return Mathf.Sqrt(2f * targetHeight * gravity);
    }

    public void HandleBoundsCollision(Transform fragmentTransform, Rigidbody2D rb, float radius)
    {
        var velocity = rb.linearVelocity;

        if (fragmentTransform.position.x <= _boundsService.minX + radius / 2)
            velocity.x = Mathf.Abs(velocity.x);

        if (fragmentTransform.position.x >= _boundsService.maxX - radius / 2)
            velocity.x = -Mathf.Abs(velocity.x);

        rb.linearVelocity = velocity;
    }

    public void HandleGroundBounce(Rigidbody2D rb, float targetBounceVelocity)
    {
        if (rb.linearVelocity.y < targetBounceVelocity)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, targetBounceVelocity);
        }
    }
    public void ApplySplitForce(Rigidbody2D rb, int direction, float horizontalKick, float upwardKick, float radius)
    {
        Vector2 force = new Vector2(direction * horizontalKick, upwardKick + radius);
        rb.AddForce(force, ForceMode2D.Impulse);
    }
}