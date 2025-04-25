using UnityEngine;

public interface IPlayerInput
{
    Vector2 MoveInput { get; }
    bool ShootInput { get; }
    bool AbilityInput { get; }
}
