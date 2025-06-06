
using System;

public class UIService : IUIService
{
    private readonly GameOverUI _gameOverUI;
    private readonly HudUI _hudUI;


    public UIService(GameOverUI gameOverUI, HudUI hudUI)
    {
        _gameOverUI = gameOverUI;
        _hudUI = hudUI;
    }

    public void HideGameOverUI()
    {
        _gameOverUI.Hide();
    }



    public void ShowGameOverUI(Action onRestartCallBack)
    {
        _gameOverUI.SetRestartCallback(onRestartCallBack);
        _gameOverUI.Show();

    }
    public void HideHudUI()
    {
        _hudUI.Hide();
    }
    public void ShowHudUI()
    {
        _hudUI.Show();
    }

    public void UpdateHealthDisplay(int currentLives)
    {
        _hudUI.UpdateHealthDisplay(currentLives);
    }
}
