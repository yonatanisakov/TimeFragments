using System;
using UnityEngine;

public interface IUIService
{
    // ========== HUD ==========
    void ShowHudUI();
    void HideHudUI();
    void InitHealthDisplay(int lives);
    void UpdateHealthDisplay(bool addLive);
    void InitTimer(float time);
    void UpdateTimer(float time);
    void UpdateScore(int score);
    void InitScore();

    // ========== RESULTS ==========
    // ה-Controller מזרים Delegates לפני הצגה, וה-ResultsUI יפעיל אותם בלחיצת כפתור
    void SetResultsHandlers(Action onRestart, Action onMainMenu, Action onNextLevel);
    void HideResultsUI();

    // ========== PAUSE ==========
    bool IsPauseWindowOpen { get; }
    void ShowPauseWindow(Action onResume, Action onRestart, Action onSettings, Action onQuit);
    void HidePauseWindow();

    // ========== FX ==========
    public void ShowFloatingTextFollow(string text, Transform followTarget, Color color);
}
