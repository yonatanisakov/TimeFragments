using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class CircularPowerUpSlider : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private Image _borderImage; // Border IS the slider
    [SerializeField] private Image _iconImage;

    [Header("Timer Colors (like game timer)")]
    [SerializeField] private Color _fullTimeColor = Color.green;
    [SerializeField] private Color _halfTimeColor = Color.yellow;
    [SerializeField] private Color _lowTimeColor = Color.red;


    private IPowerUpEffect _powerUpEffect;
    private PowerUpConfig _powerUpConfig;


    public void Initialize(IPowerUpEffect powerUpEffect, PowerUpConfig powerUpConfig)
    {
        _powerUpConfig = powerUpConfig;
        _powerUpEffect = powerUpEffect;

        // Get icon from centralized config
        var icon = _powerUpConfig.GetIcon(powerUpEffect.PowerUpType);
        if (icon != null && _iconImage != null)
        {
            _iconImage.sprite = icon;
        }
    }

    public void UpdateProgress()
    {
        if (_powerUpEffect.Duration <= 0f)
        {
            // Instant/until-condition effects - keep full color
            _borderImage.color = _fullTimeColor;
            _borderImage.fillAmount = 1f;
        }
        else
        {
            // Timed effects - change color and fill based on remaining time
            float progress = _powerUpEffect.RemainingTime / _powerUpEffect.Duration;
            _borderImage.fillAmount = progress;

            // Color transition like game timer
            if (progress > 0.5f)
                _borderImage.color = _fullTimeColor;
            else if (progress > 0.25f)
                _borderImage.color = _halfTimeColor;
            else
                _borderImage.color = _lowTimeColor;
        }
    }
}