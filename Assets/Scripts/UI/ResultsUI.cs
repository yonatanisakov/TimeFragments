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
    [SerializeField] private Button _statsButton;
    [SerializeField] private Button _statsBackButton;
    [Header("Groups")]
    [SerializeField] private GameObject _mainGroup;   
    [SerializeField] private GameObject _statsPanel;  
    [SerializeField] private CanvasGroup _statsCanvas;

    [Header("Audio Clips")]                  
    [SerializeField] private AudioClip _victoryStinger;
    [SerializeField] private AudioClip _defeatStinger;


    [Inject] private IAudioService _audio;

    private Action _onRestart;
    private Action _onMainMenu;
    private Action _onNextLevel;
    public void Initialize()
    {
        _restartButton.onClick.AddListener(OnRestartClicked);
        _continueButton.onClick.AddListener(OnContinueClicked);
        _mainMenuButton.onClick.AddListener(OnMainMenuClicked);
        if (_statsButton != null) _statsButton.onClick.AddListener(ShowStats);
        if (_statsBackButton != null) _statsBackButton.onClick.AddListener(HideStats);

        // Subscribe to game events
        EventBus.Get<GameEndEvent>().Subscribe(OnGameEnd);

        HideStats();
        Hide();
    }

    public void Dispose()
    {
        _restartButton.onClick.RemoveListener(OnRestartClicked);
        _continueButton.onClick.RemoveListener(OnContinueClicked);
        _mainMenuButton.onClick.RemoveListener(OnMainMenuClicked);

        if (_statsButton != null) _statsButton.onClick.RemoveListener(ShowStats);
        if (_statsBackButton != null) _statsBackButton.onClick.RemoveListener(HideStats);

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
        HideStats();

        // Set victory UI state
        _titleText.text = "Victory!";
        _titleText.color = Color.green;

        // Show all score details
        SetScoreVisibility(true);
        DisplayScoreData(completionData);

        // Show continue button
        _continueButton.gameObject.SetActive(true);
        _restartButton.gameObject.SetActive(true);
        _mainMenuButton.gameObject.SetActive(true);
        _statsButton.gameObject.SetActive(true);
        // Show and update stars
        SetStarVisibility(true);
        DisplayStarRating(completionData.starRating);

        Show();
        _audio?.PlaySfx(_victoryStinger, 1f, 0.02f);
    }

    private void ShowDefeat()
    {

        HideStats();

        // Set defeat UI state
        _titleText.text = "Game Over";
        _titleText.color = Color.red;
        // Hide score details for defeat
        SetScoreVisibility(false);

        // Hide continue button, show only restart
        _continueButton.gameObject.SetActive(false);
        _statsButton.gameObject.SetActive(false);
        _restartButton.gameObject.SetActive(true);
        _mainMenuButton.gameObject.SetActive(true);

        // Hide stars
        SetStarVisibility(true);
        DisplayStarRating(0);
        Show();
        _audio?.PlaySfx(_defeatStinger, 1f, 0.00f);
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
        _finalScoreText.text = $"Final Score \n {data.finalScore:N0}";
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
    private void ShowStats()
{
    if (_mainGroup != null) _mainGroup.SetActive(false);

    if (_statsPanel != null)
    {
        _statsPanel.SetActive(true);

        if (_statsCanvas != null)
        {
            _statsCanvas.alpha = 1f;
            _statsCanvas.blocksRaycasts = true;
            _statsCanvas.interactable = true;
        }
    }
}

private void HideStats()
{
    if (_statsCanvas != null)
    {
        _statsCanvas.alpha = 0f;
        _statsCanvas.blocksRaycasts = false;
        _statsCanvas.interactable = false;
    }

    if (_statsPanel != null) _statsPanel.SetActive(false);
    if (_mainGroup != null)  _mainGroup.SetActive(true);
}

    public void SetHandlers(Action onRestart, Action onMainMenu, Action onNextLevel)
    {
        _onRestart = onRestart;
        _onMainMenu = onMainMenu;
        _onNextLevel = onNextLevel;
    }
    private void OnRestartClicked()
    {
        Hide();
        _onRestart?.Invoke();
    }

    private void OnContinueClicked()
    {
        Hide();
        _onNextLevel?.Invoke();
    }

    private void OnMainMenuClicked()
    {
        Hide();
        _onMainMenu?.Invoke();
    }

}