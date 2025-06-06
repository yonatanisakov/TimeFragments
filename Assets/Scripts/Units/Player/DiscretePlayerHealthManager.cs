using EventBusScripts;
using System;
using UnityEngine;
using Zenject;

public class DiscretePlayerHealthManager : IInitializable, IPlayerHealthManager, IDisposable
{
    private readonly IUIService _uiService;
    private int maxLives = 3;

    public int currentLives { get; private set; }

    [Inject]
    public DiscretePlayerHealthManager(IUIService uiService)
    {
        _uiService = uiService;
    }

    public void Initialize()
    {
        currentLives = maxLives;
        _uiService.UpdateHealthDisplay(currentLives);
        EventBus.Get<PlayerGetHitEvent>().Subscribe(OnPlayerHit);
    }

    public void OnPlayerHit()
    {
        if (currentLives > 0)
        {
            currentLives--;
            _uiService.UpdateHealthDisplay(currentLives);

            if (currentLives <= 0)
                EventBus.Get<LevelFailEvent>().Invoke();
        }
    }
    public void Dispose()
    {
        EventBus.Get<PlayerGetHitEvent>().Unsubscribe(OnPlayerHit);
    }
}