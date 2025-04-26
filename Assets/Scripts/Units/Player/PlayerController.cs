using UnityEngine;

public class PlayerController 
{
    private readonly IPlayerInput playerInput;
    private readonly Transform transform;
    private readonly float moveSpeed;
    private readonly IBoundsService bounds;

    private float playerWidth;
    public PlayerController(IPlayerInput playerInput, Transform transform, float moveSpeed,IBoundsService bounds)
    {
        this.playerInput = playerInput;
        this.transform = transform;
        this.moveSpeed = moveSpeed;
        this.bounds = bounds;

        playerWidth = transform.GetComponentInChildren<SpriteRenderer>().bounds.size.x/2;
    }
    public void Tick()
    {
        Vector2 move = playerInput.MoveInput;
        transform.Translate(Vector3.right * move.x * moveSpeed * Time.deltaTime);
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, bounds.minX + playerWidth,  bounds.maxX - playerWidth);

        transform.position = pos;
    }
}
