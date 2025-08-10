using UnityEngine;
using Zenject;

public class PlayerWeapon : IPlayerWeapon
{
    private float _cooldownTime;
    private Transform _playerMuzzle;
    private Bullet.Pool _bulletPool;
    private IStatisticsService _statisticsService;
    private float _currentCooldownTime;
    private Animator _muzzleAnimator;


    [Inject]
    public PlayerWeapon(Bullet.Pool bulletPool, IStatisticsService statisticsService)
    {
        _bulletPool = bulletPool;
        _statisticsService = statisticsService;
    }
    public void Initialize(float cooldownTime, Transform playerMuzzle)
    {
        _cooldownTime = cooldownTime;
        _playerMuzzle = playerMuzzle;
        _muzzleAnimator = playerMuzzle.GetComponent<Animator>();
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
            _statisticsService.OnBulletFired();
            _muzzleAnimator?.SetTrigger("Shoot");

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
    public void CancelCooldown()
    {
        _currentCooldownTime = 0f;
    }
}