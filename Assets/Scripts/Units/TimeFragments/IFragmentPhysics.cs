using UnityEngine;

/// <summary>
/// Handles fragment physics behavior
/// </summary>
public interface IFragmentPhysics
{
    void ApplyBoundsPhysics(TimeFragment fragment);
    void ApplyGroundBounce(TimeFragment fragment);
    void ApplyWallBounce(TimeFragment fragment);
    void ApplySplitForce(Rigidbody2D rb, int direction, float horizontalKick, float upwardKick, float radius);


    float CalculateBounceVelocity(float baseHeight, float radius, float sizeFactor);

}