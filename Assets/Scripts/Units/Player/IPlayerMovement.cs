using UnityEngine;

public interface IPlayerMovement
{
    void Initialize(Transform playerTransform, float speed);

    void Move(Vector2 direction);
    void ConstrainToPlayArea();

}