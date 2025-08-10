using EventBusScripts;
using System;
using UnityEngine;
using Zenject;

public class DiscretePlayerHealthManager : IInitializable, IPlayerHealthManager, IDisposable
{
    private readonly IUIService _uiService;
    private int _initLives = 3;
    private int _maxLives = 5;

    public int currentLives { get; private set; }
    private IPowerUpService _powerUpService;

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
    }

    public void OnPlayerHit()
    {
        if (_powerUpService.HasActivePowerUp(PowerUpType.Shield))
        {
            EventBus.Get<ShieldConsumedEvent>().Invoke();
            return; // Shield blocks damage
        }
        else if (currentLives > 0)
        {
            currentLives--;
            _uiService.UpdateHealthDisplay(false);

            if (currentLives <= 0)
                EventBus.Get<LevelFailEvent>().Invoke();
        }
    }
    public void Dispose()
    {
        EventBus.Get<PlayerGetHitEvent>().Unsubscribe(OnPlayerHit);
    }
}