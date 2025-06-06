using UnityEngine;

/// <summary>
/// Handles fragment physics behavior
/// </summary>
public interface IFragmentPhysics
{
    void HandleBoundsCollision(Transform fragmentTransform, Rigidbody2D rb, float radius);
    void HandleGroundBounce(Rigidbody2D rb, float targetBounceVelocity);
    float CalculateBounceVelocity(float baseHeight, float radius, float sizeFactor);
    void ApplySplitForce(Rigidbody2D rb, int direction, float horizontalKick, float upwardKick, float radius);

}