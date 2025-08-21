using EventBusScripts;
using System;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class BottomHudUI : MonoBehaviour, IInitializable
{

    [Header("Health UI")]
    [SerializeField] private Transform _livesContainer;
    [SerializeField] private GameObject _hearthPrefab;

    [Header("Timer UI")]
    [SerializeField] private Slider _timerBar;

    [Header("Timer Colors")]
    [SerializeField] private Color _timerColorGreen = Color.green;
    [SerializeField] private Color _timerColorOrange = new Color(1f, 0.65f, 0f); // Orange
    [SerializeField] private Color _timerColorRed = Color.red;

    [Header("Score UI")]
    [SerializeField] private TextMeshProUGUI _scoreText;

    [Header("General")]
    [SerializeField] private GameObject _hudPanel;

    private Stack<GameObject> _hearthPrefabs = new Stack<GameObject>();
    private float _totalTime;
    private Image _timerBarFillRectImage;
    public void Initialize()
    {
        Show();
    }

    public void Show()
    {
        if (_hudPanel != null)
            _hudPanel.SetActive(true);
    }
    public void Hide()
    {
        if (_hudPanel != null)
            _hudPanel.SetActive(false);
    }
    public void AddHearth()
    {
        GameObject hearth = Instantiate(_hearthPrefab, _livesContainer);
        _hearthPrefabs.Push(hearth);
    }
    public void RemoveHearth()
    {
        Destroy(_hearthPrefabs.Pop());
    }

    public void InitTimer(float time)
    {
        _timerBarFillRectImage = _timerBar.fillRect.GetComponent<Image>();
        _totalTime = time;
        _timerBar.maxValue = time;
        UpdateTimer(time);
    }
    public void UpdateTimer(float time)
    {
        _timerBar.value = time;
        UpdateTimerColor(time);
    }
    public void UpdateScore(int score)
    {
        if (_scoreText != null)
            _scoreText.text = score.ToString();
    }
    private void UpdateTimerColor(float time)
    {
        if (_totalTime > 0)
        {
            float timerPercentage = time / _totalTime;
            if (timerPercentage > 0.5 )
                _timerBarFillRectImage.color = _timerColorGreen;
            else if (timerPercentage > 0.25)
                _timerBarFillRectImage.color = _timerColorOrange;
            else
                _timerBarFillRectImage.color = _timerColorRed;

        }

    }
}
