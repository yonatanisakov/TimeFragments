using EventBusScripts;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HudUI : MonoBehaviour
{

    [SerializeField] private TMP_Text healthText;
    IPlayerHealthManager healthManager;

    [Inject]
    public void Construct(IPlayerHealthManager healthManager)
    {
        this.healthManager = healthManager;
        EventBus.Get<PlayerHealthDecrease>().Subscribe(OnHealthChanged);
    }
    private void Start()
    {
        HealthDisplayText(healthManager.currentLives);

    }
    private void OnHealthChanged(int currentLives)
    {
        HealthDisplayText(currentLives);
    }

    private void HealthDisplayText(int currentLives)
    {
        healthText.text = $"Lives: {currentLives}";
    }
    private void OnDestroy()
    {
        EventBus.Get<PlayerHealthDecrease>().Unsubscribe(OnHealthChanged);
    }
}
