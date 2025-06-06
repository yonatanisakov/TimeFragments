
using System;

public interface IUIService
{
    // Game Over UI
    void ShowGameOverUI(Action onRestartCallBack);
    void HideGameOverUI();

    // HUI UI
    void ShowHudUI();
    void HideHudUI();
    void UpdateHealthDisplay(int currentLives);
}