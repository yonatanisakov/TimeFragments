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
    private GameStateManager _gameStateManager;

    [Inject]
    public void Construct(GameStateManager gameStateManager)
    {
        _gameStateManager = gameStateManager;
    }
    public void Initialize()
    {
        _restartBtn.onClick.AddListener(OnRestart);
    }

    public void Show()=>_gameOverPanel.SetActive(true);
    public void Hide()=>_gameOverPanel?.SetActive(false);
    private void OnRestart()
    {
        _gameStateManager.ChangeGameState(_gameStateManager.PlayingSate);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Dispose()
    {
        _restartBtn.onClick.RemoveListener(OnRestart);
    }



}
