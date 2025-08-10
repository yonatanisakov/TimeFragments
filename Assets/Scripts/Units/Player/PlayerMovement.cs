using EventBusScripts;
using System;
using UnityEngine;

public class PlayerMovement:IPlayerMovement,IDisposable
{
    private Transform _playerTransform;
    private IBoundsService _boundsService;
    private Rigidbody2D _rigidbody;
    private float _playerSpeed;
    private float _playerHalfWidth;
    private bool _controlsInverted = false;

    public PlayerMovement(IBoundsService boundsService)
    {
        _boundsService = boundsService;
    }
    public void Initialize(Transform playerTransform, float playerSpeed)
    {
        _playerTransform = playerTransform;
        _playerSpeed = playerSpeed;
        _playerHalfWidth = _playerTransform.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2;
        _rigidbody = _playerTransform.GetComponent<Rigidbody2D>();
        if (_rigidbody == null)
        {
            _rigidbody = _playerTransform.gameObject.AddComponent<Rigidbody2D>();
        }

        // Configure for controlled movement
        _rigidbody.gravityScale = 0f; // No gravity in top-down/side game
        _rigidbody.freezeRotation = true; // Don't spin

        // Subscribe to control inversion events
        EventBus.Get<ControlsInvertedEvent>().Subscribe(OnControlsInverted);

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

        // Apply control inversion if active
        float moveDirection = _controlsInverted ? -direction.x : direction.x;
        Vector2 velocity = Vector2.right * moveDirection * _playerSpeed;
        _rigidbody.linearVelocity = velocity;
        ConstrainToPlayArea();
    }
    private void OnControlsInverted(bool inverted)
    {
        _controlsInverted = inverted;
        Debug.Log($"Controls inverted: {inverted}");
    }
    public void Dispose()
    {
        EventBus.Get<ControlsInvertedEvent>().Unsubscribe(OnControlsInverted);
    }
}