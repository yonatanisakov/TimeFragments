
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using Zenject;

public class FragmentPhysics : IFragmentPhysics
{
    private readonly IBoundsService _boundsService;
    private readonly IFragmentTimeScaleService _fragmentTimeScaleService;

    [Inject]
    public FragmentPhysics(IBoundsService boundsService, IFragmentTimeScaleService fragmentTimeScaleService)
    {
        _boundsService = boundsService;
        _fragmentTimeScaleService = fragmentTimeScaleService;
    }
    public float CalculateBounceVelocity(float baseHeight, float radius, float sizeFactor)
    {
        float gravity = Mathf.Abs(Physics2D.gravity.y);
        float targetHeight = baseHeight + radius * sizeFactor;
        return Mathf.Sqrt(2f * targetHeight * gravity);
    }

    public void ApplyBoundsPhysics(TimeFragment fragment)
    {
        var velocity = fragment.RigiBody.linearVelocity;

        if (fragment.transform.position.x <= _boundsService.minX + fragment.Radius / 2)
            velocity.x = Mathf.Abs(velocity.x);

        if (fragment.transform.position.x >= _boundsService.maxX - fragment.Radius / 2)
            velocity.x = -Mathf.Abs(velocity.x);

        fragment.RigiBody.linearVelocity = velocity;
    }
    public void ApplyGroundBounce(TimeFragment fragment)
    {
        float bounceVelocity = fragment.TargetBounceVelocity;

        if (fragment.RigiBody.linearVelocity.y < bounceVelocity)
        {
            Vector2 newVelocity = new Vector2(
                fragment.RigiBody.linearVelocity.x,
                bounceVelocity
            );
            fragment.RigiBody.linearVelocity = newVelocity;
        }
    }
    public void ApplyWallBounce(TimeFragment fragment)
    {
        Vector2 hitDirection = fragment.LastCollision.contacts[0].normal;
        Debug.Log(hitDirection);
        Vector2 velocity = fragment.RigiBody.linearVelocity;
        if (hitDirection.y > 0.7)
            velocity.y = fragment.TargetBounceVelocity ;
        else if(Mathf.Abs(hitDirection.x)>0.7)
            velocity.x = -fragment.PreviousVelocity.x ;
        fragment.RigiBody.linearVelocity = velocity;
            
    }
    public void ApplySplitForce(Rigidbody2D rb, int direction, float horizontalKick, float upwardKick, float radius)
    {
        Vector2 force = new Vector2(direction * horizontalKick, upwardKick + radius) * _fragmentTimeScaleService.CurrentFragmentTimeScale;
        rb.AddForce(force, ForceMode2D.Impulse);
    }
}