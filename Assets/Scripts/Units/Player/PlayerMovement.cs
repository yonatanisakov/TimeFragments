using UnityEngine;

public class PlayerMovement:IPlayerMovement
{
    private Transform _playerTransform;
    private IBoundsService _boundsService;
    private float _playerSpeed;
    private float _playerHalfWidth;

    public PlayerMovement(IBoundsService boundsService)
    {
        _boundsService = boundsService;
    }
    public void Initialize(Transform playerTransform, float playerSpeed)
    {
        _playerTransform = playerTransform;
        _playerSpeed = playerSpeed;
        _playerHalfWidth = _playerTransform.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2;
    }
    public void ConstrainToPlayArea()
    {
        Vector3 pos = _playerTransform.position;
        pos.x = Mathf.Clamp(pos.x, _boundsService.minX + _playerHalfWidth, _boundsService.maxX - _playerHalfWidth);
        _playerTransform.position = pos;
    }



    public void Move(Vector2 direction)
    {
        if (_playerTransform == null) return;

        _playerTransform.Translate(Vector3.right*direction.x*_playerSpeed*Time.deltaTime);
        ConstrainToPlayArea();
    }
}