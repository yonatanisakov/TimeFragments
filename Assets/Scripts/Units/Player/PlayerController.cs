using Assets.Scripts.Utils;
using System.ComponentModel;
using UnityEngine;
using Zenject;
public class PlayerController 
{
    private readonly IPlayerInput playerInput;
    private readonly Transform transform;
    private readonly float moveSpeed;
    private readonly IBoundsService bounds;
    private readonly Transform playerMuzzle;
    private float playerWidth;
    private Bullet.Pool bulletPool;
    private float bulletCoolDown ;
    public PlayerController(IPlayerInput playerInput, Transform transform,
        float moveSpeed,IBoundsService bounds,Transform playerMuzzle,Bullet.Pool bulletPool)
    {
        this.playerInput = playerInput;
        this.transform = transform;
        this.moveSpeed = moveSpeed;
        this.bounds = bounds;
        this.playerMuzzle = playerMuzzle;
        this.bulletPool = bulletPool;


        playerWidth = transform.GetComponentInChildren<SpriteRenderer>().bounds.size.x/2;
    }
    public void Tick()
    {
        if (playerInput.ShootInput && bulletCoolDown<=0)
        {
            bulletPool.Spawn(playerMuzzle.position,Quaternion.identity);
            bulletCoolDown = 0.2f;
        }
        Vector2 move = playerInput.MoveInput;
        transform.Translate(Vector3.right * move.x * moveSpeed * Time.deltaTime);
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, bounds.minX + playerWidth,  bounds.maxX - playerWidth);

        transform.position = pos;
        bulletCoolDown-=Time.deltaTime;
    }
}
