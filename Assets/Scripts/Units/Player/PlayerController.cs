using UnityEngine;

public class PlayerController 
{
    private readonly IPlayerInput playerInput;
    private readonly Transform transform;
    private readonly float moveSpeed;

    private float halfWidth;
    private float playerWidth;

    private float camXPos;
    public PlayerController(IPlayerInput playerInput, Transform transform, float moveSpeed = 5f)
    {
        this.playerInput = playerInput;
        this.transform = transform;
        this.moveSpeed = moveSpeed;

        playerWidth = transform.GetComponentInChildren<SpriteRenderer>().bounds.size.x/2;
        halfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        camXPos = Camera.main.transform.position.x;
    }
    public void Tick()
    {
        Vector2 move = playerInput.MoveInput;
        transform.Translate(Vector3.right * move.x * moveSpeed * Time.deltaTime);
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x,camXPos -halfWidth + playerWidth, camXPos + halfWidth - playerWidth);

        transform.position = pos;
    }
}
