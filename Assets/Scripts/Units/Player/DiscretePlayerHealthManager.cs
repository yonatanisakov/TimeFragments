using EventBusScripts;
using System;
using UnityEngine;
using Zenject;

public class DiscretePlayerHealthManager : IInitializable, IPlayerHealthManager, IDisposable
{
    private int maxLives = 3;

    public int currentLives { get; private set; }

    public void Initialize()
    {
        currentLives = maxLives;
        EventBus.Get<PlayerGetHitEvent>().Subscribe(OnPlayerHit);
    }

    public void OnPlayerHit()
    {
        if (currentLives > 0)
        {
            currentLives--;

            EventBus.Get<PlayerHealthDecrease>().Invoke(currentLives);

            if (currentLives <= 0)
                EventBus.Get<LevelFailEvent>().Invoke();
        }
    }
    public void Dispose()
    {
        EventBus.Get<PlayerGetHitEvent>().Unsubscribe(OnPlayerHit);
    }
}