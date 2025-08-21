using EventBusScripts;
using System;
using UnityEngine;
using Zenject;

public class DiscretePlayerHealthManager : IInitializable, IPlayerHealthManager, IDisposable
{
    private readonly IUIService _uiService;
    private int _initLives = 3;
    private int _maxLives = 5;

    private const float INVULN_SECONDS = 2f;
    private float _invulnerableUntil = 0f;
    public int currentLives { get; private set; }
    private IPowerUpService _powerUpService;

    public bool IsInvulnerable => Time.time < _invulnerableUntil;

    [Inject]
    public DiscretePlayerHealthManager(IUIService uiService, IPowerUpService powerUpService)
    {
        _uiService = uiService;
        _powerUpService = powerUpService;
    }

    public void Initialize()
    {
        currentLives = _initLives;
        _uiService.InitHealthDisplay(currentLives);
        EventBus.Get<PlayerGetHitEvent>().Subscribe(OnPlayerHit);
        _invulnerableUntil = 0f;

    }

    public void OnPlayerHit()
    {
        if (IsInvulnerable)
            return;

        if (_powerUpService.HasActivePowerUp(PowerUpType.Shield))
        {
            EventBus.Get<ShieldConsumedEvent>().Invoke();
            return; // Shield blocks damage
        }
        else if (currentLives > 0)
        {
            currentLives--;
            _uiService.UpdateHealthDisplay(false);

            GrantInvulnerability(INVULN_SECONDS);
            SFXBus.I?.PlayPlayerHit();

            if (currentLives <= 0)
                EventBus.Get<LevelFailEvent>().Invoke();
        }
    }
    public void GrantInvulnerability(float duration)
    {
        if (duration <= 0f) return;
        float until = Time.time + duration;
        if (until > _invulnerableUntil)
            _invulnerableUntil = until;

        float remaining = Mathf.Max(0f, _invulnerableUntil - Time.time);
        EventBus.Get<PlayerInvulnerabilityStartedEvent>().Invoke(remaining);

    }

    public void ClearInvulnerability()
    {
        _invulnerableUntil = 0f;
    }
    public void Dispose()
    {
        EventBus.Get<PlayerGetHitEvent>().Unsubscribe(OnPlayerHit);
    }
}