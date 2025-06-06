using EventBusScripts;
using System;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Zenject;

public class HudUI : MonoBehaviour,IInitializable
{

    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private GameObject _hudPanel;
    public void Initialize()
    {
        Show();
    }

    public void Show()
    {
        if(_hudPanel != null)
            _hudPanel.SetActive(true);
    }
    public void Hide()
    {
        if (_hudPanel != null)
            _hudPanel.SetActive(false);
    }

    public void UpdateHealthDisplay(int currentLives)
    {
        if (_healthText != null)
            _healthText.text = $"Lives: {currentLives}";
    }


}
