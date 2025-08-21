
using System;
using UnityEngine;

public class UIService : IUIService
{
    private readonly ResultsUI _resultsUI;
    private readonly BottomHudUI _hudUI;
    private readonly ComboPopupWidget _comboPopup;
    // ==== Pause ====
    [SerializeField] private PauseWindowUI _pauseWindow;
    private bool _pauseOpen;

    // ==== Results callbacks (מוזרקים ע"י ה-Controller) ====
    private Action _onResultsRestart;
    private Action _onResultsMainMenu;
    private Action _onResultsNextLevel;

    public UIService(ResultsUI resultsUI, BottomHudUI hudUI, ComboPopupWidget comboPopup, PauseWindowUI pauseWindow)
    {
        _resultsUI = resultsUI;
        _hudUI = hudUI;
        _comboPopup = comboPopup;
        _pauseWindow = pauseWindow;

    }

    // ========== RESULTS ==========
    public void SetResultsHandlers(Action onRestart, Action onMainMenu, Action onNextLevel)
    {
        _onResultsRestart = onRestart;
        _onResultsMainMenu = onMainMenu;
        _onResultsNextLevel = onNextLevel;

        _resultsUI.SetHandlers(_onResultsRestart, _onResultsMainMenu, _onResultsNextLevel);
    }
    // ========== HUD ==========

    public void HideResultsUI()
    {
        _resultsUI.Hide();
    }

    public void HideHudUI()
    {
        _hudUI.Hide();
    }
    public void ShowHudUI()
    {
        _hudUI.Show();
    }

    public void UpdateHealthDisplay(bool increaseLife)
    {
        if (increaseLife)
            _hudUI.AddHearth();
        else
            _hudUI.RemoveHearth();
    }

    public void InitHealthDisplay(int lives)
    {
        for (int i = 0; i < lives; i++)
            _hudUI.AddHearth();
    }

    public void InitTimer(float time)
    {
        _hudUI.InitTimer(time);
    }

    public void UpdateTimer(float time)
    {
        _hudUI.UpdateTimer(time);
    }

    public void UpdateScore(int score)
    {
        _hudUI.UpdateScore(score);
    }

    public void InitScore()
    {
        _hudUI.UpdateScore(0);
    }
    // ========== FX ==========

    public void ShowFloatingTextFollow(string text, Transform followTarget, Color color)
    {
        _comboPopup?.Show(followTarget, text, color);
    }
    // ========== PAUSE ==========
    public bool IsPauseWindowOpen => _pauseOpen && _pauseWindow != null && _pauseWindow.IsOpen;

    public void ShowPauseWindow(Action onResume, Action onRestart, Action onSettings, Action onQuit)
    {
        if (_pauseWindow == null)
        {
            Debug.LogWarning("[UIService] PauseWindowUI is not assigned on Canvas.");
            return;
        }
        _pauseOpen = true;
        _pauseWindow.Show(
            onResume: () => { _pauseOpen = false; onResume?.Invoke(); },
            onRestart: () => { _pauseOpen = false; onRestart?.Invoke(); },
            onSettings: () => { onSettings?.Invoke(); },
            onQuit: () => { _pauseOpen = false; onQuit?.Invoke(); }
        );
    }

    public void HidePauseWindow()
    {
        if (_pauseWindow == null) return;
        _pauseWindow.Hide();
        _pauseOpen = false;
    }


}
