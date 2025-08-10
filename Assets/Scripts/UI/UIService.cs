
using System;
using UnityEngine;

public class UIService : IUIService
{
    private readonly ResultsUI _resultsUI;
    private readonly BottomHudUI _hudUI;

    public UIService(ResultsUI resultsUI, BottomHudUI hudUI)
    {
        _resultsUI = resultsUI;
        _hudUI = hudUI;
    }

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

    public void ShowFloatingText(string text, Vector3 worldPosition, Color color)
    {
        // TODO: Implement floating text system for combo multipliers
        // This will show combo text at fragment impact locations
    }
}
