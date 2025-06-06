using UnityEngine;
using Zenject;

public class PlayerWeapon : IPlayerWeapon
{
    private float _cooldownTime;
    private Transform _playerMuzzle;
    private Bullet.Pool _bulletPool;
    private float _currentCooldownTime;

    [Inject]
    public PlayerWeapon(Bullet.Pool bulletPool)
    {
        _bulletPool = bulletPool;
    }
    public void Initialize(float cooldownTime, Transform playerMuzzle)
    {
        _cooldownTime = cooldownTime;
        _playerMuzzle = playerMuzzle;
        _currentCooldownTime = 0f;
    }

    public void SetCooldown()
    {
        _currentCooldownTime = _cooldownTime;
    }

    public void TryShoot()
    {
        if (_currentCooldownTime <= 0 && _playerMuzzle != null)
        {
            _bulletPool.Spawn(_playerMuzzle.position, Quaternion.identity);
            SetCooldown();
        }

    }

    public void UpdateCooldown()
    {
        if (_currentCooldownTime > 0)
        {
            _currentCooldownTime -= Time.deltaTime;
        }
    }
}