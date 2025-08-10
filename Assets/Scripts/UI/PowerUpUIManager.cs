using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PowerUpUIManager : MonoBehaviour
{
    [Header("UI Setup")]
    [SerializeField] private Transform _powerUpContainer;
    [SerializeField] private GameObject _circularSliderPrefab;

    private IPowerUpService _powerUpService;
    private PowerUpConfig _powerUpConfig;

    private Dictionary<IPowerUpEffect, CircularPowerUpSlider> _activeSliders = new Dictionary<IPowerUpEffect, CircularPowerUpSlider>();

    [Inject]
    public void Construct(IPowerUpService powerUpService, PowerUpConfig powerUpConfig)
    {
        _powerUpService = powerUpService;
        _powerUpConfig = powerUpConfig;
    }

    private void Start()
    {
        // Subscribe to power-up events
        _powerUpService.OnPowerUpActivated += OnPowerUpActivated;
        _powerUpService.OnPowerUpExpired += OnPowerUpExpired;
    }

    private void Update()
    {
        // Update all active sliders
        foreach (var slider in _activeSliders.Values)
        {
            slider.UpdateProgress();
        }
    }

    private void OnPowerUpActivated(IPowerUpEffect effect)
    {
        // Create new slider for this power-up
        var sliderObject = Instantiate(_circularSliderPrefab, _powerUpContainer);
        var slider = sliderObject.GetComponent<CircularPowerUpSlider>();


        slider.Initialize(effect, _powerUpConfig);
        _activeSliders[effect] = slider;

        Debug.Log($"UI: Power-up activated - {effect.PowerUpType}");
    }

    private void OnPowerUpExpired(IPowerUpEffect effect)
    {
        // Remove slider for expired power-up
        if (_activeSliders.TryGetValue(effect, out var slider))
        {
            Destroy(slider.gameObject);
            _activeSliders.Remove(effect);

            Debug.Log($"UI: Power-up expired - {effect.PowerUpType}");
        }
    }

    private void OnDestroy()
    {
        if (_powerUpService != null)
        {
            _powerUpService.OnPowerUpActivated -= OnPowerUpActivated;
            _powerUpService.OnPowerUpExpired -= OnPowerUpExpired;
        }
    }
}