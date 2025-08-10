
using System;
using UnityEngine;

public interface IUIService
{
    // bottomHUD UI
    void ShowHudUI();
    void HideHudUI();
    void InitHealthDisplay(int lives);
    void UpdateHealthDisplay(bool addLive);
    void InitTimer(float time);
    void UpdateTimer(float time);
    void UpdateScore(int score);
    void InitScore();

    // Unified Results UI (handles both win/lose)
    void HideResultsUI();

    //  Floating Text Effects (for future combo display)
    void ShowFloatingText(string text, Vector3 worldPosition, Color color);

}