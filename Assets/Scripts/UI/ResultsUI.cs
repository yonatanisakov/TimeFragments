using EventBusScripts;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ResultsUI : MonoBehaviour, IInitializable, IDisposable
{
    [Header("UI References")]
    [SerializeField] private GameObject _resultsPanel;

    [Header("Title")]
    [SerializeField] private TextMeshProUGUI _titleText;

    [Header("Score Section")]
    [SerializeField] private TextMeshProUGUI _finalScoreText;
    [SerializeField] private TextMeshProUGUI _timeBonusText;
    [SerializeField] private TextMeshProUGUI _accuracyBonusText;
    [SerializeField] private TextMeshProUGUI _perfectBonusText;

    [Header("Statistics Section")]
    [SerializeField] private TextMeshProUGUI _accuracyText;
    [SerializeField] private TextMeshProUGUI _timeRemainingText;
    [SerializeField] private TextMeshProUGUI _shotsText;

    [Header("Star Rating")]
    [SerializeField] private Transform _starContainer;
    [SerializeField] private Image[] _starImages; // Assign the 3 star images
    [SerializeField] private Sprite _filledStarSprite;
    [SerializeField] private Sprite _emptyStarSprite;

    [Header("Buttons")]
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _continueButton;
    [SerializeField] private Button _mainMenuButton;



    public void Initialize()
    {
        _restartButton.onClick.AddListener(OnRestartClicked);
        _continueButton.onClick.AddListener(OnContinueClicked);
        _mainMenuButton.onClick.AddListener(OnMainMenuClicked);

        // Subscribe to game events
        EventBus.Get<GameEndEvent>().Subscribe(OnGameEnd);

        Hide();
    }

    public void Dispose()
    {
        _restartButton.onClick.RemoveListener(OnRestartClicked);
        _continueButton.onClick.RemoveListener(OnContinueClicked);
        _mainMenuButton.onClick.RemoveListener(OnMainMenuClicked);

        EventBus.Get<GameEndEvent>().Unsubscribe(OnGameEnd);
    }
    private void OnGameEnd(GameEndData gameEndData)
    {
        if (gameEndData.isVictory)
        {
            ShowVictory(gameEndData.completionData);
        }
        else
        {
            ShowDefeat();
        }
    }

    private void ShowVictory(LevelCompletionData completionData)
    {
        // Set victory UI state
        _titleText.text = "Victory!";

        // Show all score details
        SetScoreVisibility(true);
        DisplayScoreData(completionData);

        // Show continue button
        _continueButton.gameObject.SetActive(true);
        _restartButton.gameObject.SetActive(true);
        _mainMenuButton.gameObject.SetActive(true);

        // Show and update stars
        SetStarVisibility(true);
        DisplayStarRating(completionData.starRating);

        Show();
    }

    private void ShowDefeat()
    {
        // Set defeat UI state
        _titleText.text = "Game Over";

        // Hide score details for defeat
        SetScoreVisibility(false);

        // Hide continue button, show only restart
        _continueButton.gameObject.SetActive(false);
        _restartButton.gameObject.SetActive(true);
        _mainMenuButton.gameObject.SetActive(true);

        // Hide stars
        SetStarVisibility(false);

        Show();
    }
    public void Show()
    {
        if (_resultsPanel != null)
            _resultsPanel.SetActive(true);
    }

    public void Hide()
    {
        if (_resultsPanel != null)
            _resultsPanel.SetActive(false);
    }

    

    private void DisplayScoreData(LevelCompletionData data)
    {
        _finalScoreText.text = $"Final Score: {data.finalScore:N0}";
        _timeBonusText.text = $"Time Bonus: +{data.timeBonus:N0}";
        _accuracyBonusText.text = $"Accuracy Bonus: +{data.accuracyBonus:N0}";
        _perfectBonusText.text = $"Perfect Bonus: +{data.perfectBonus:N0}";

        _accuracyText.text = $"Accuracy: {data.accuracyPercentage:F1}%";
        _timeRemainingText.text = $"Time Remaining: {data.timeRemaining:F1}s";
        _shotsText.text = $"Shots: {data.bulletsHit}/{data.bulletsFired}";

    }

    private void DisplayStarRating(int starCount)
    {
        for (int i = 0; i < _starImages.Length && i < 3; i++)
        {
            if (_starImages[i] != null)
            {
                _starImages[i].sprite = i < starCount ? _filledStarSprite : _emptyStarSprite;
            }
        }
    }

    private void SetScoreVisibility(bool visible)
    {
        _finalScoreText.gameObject.SetActive(visible);
        _timeBonusText.gameObject.SetActive(visible);
        _accuracyBonusText.gameObject.SetActive(visible);
        _perfectBonusText.gameObject.SetActive(visible);
        _accuracyText.gameObject.SetActive(visible);
        _timeRemainingText.gameObject.SetActive(visible);
        _shotsText.gameObject.SetActive(visible);
    }

    private void SetStarVisibility(bool visible)
    {
        if (_starContainer != null)
            _starContainer.gameObject.SetActive(visible);
    }



    private void OnRestartClicked()
    {
        Hide();
        EventBus.Get<RestartGameEvent>().Invoke();
    }

    private void OnContinueClicked()
    {
        Hide();
        EventBus.Get<NextLevelRequestedEvent>().Invoke(new object());
    }

    private void OnMainMenuClicked()
    {
        Hide();
        EventBus.Get<MainMenuRequestedEvent>().Invoke(new object());

    }
}