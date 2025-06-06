using EventBusScripts;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class GameOverUI : MonoBehaviour, IInitializable, IDisposable
{
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private Button _restartBtn;

    private Action _onRestartCallback;


    public void Initialize()
    {
        _restartBtn.onClick.AddListener(OnRestartClicked);
    }

    public void Show()
    {
        if (_gameOverPanel != null)
        {
            _gameOverPanel.SetActive(true);
        }
    }
    public void Hide()
    {
        if (_gameOverPanel != null)
            _gameOverPanel.SetActive(false);
    }

    public void SetRestartCallback(Action callback) => _onRestartCallback = callback;
    private void OnRestartClicked() => _onRestartCallback?.Invoke();

    public void Dispose()
    {
        _restartBtn.onClick.RemoveListener(OnRestartClicked);
        _onRestartCallback = null;
    }
}
