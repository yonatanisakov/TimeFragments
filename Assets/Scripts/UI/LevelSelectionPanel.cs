using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using TMPro;
using System.Collections;

/// <summary>
/// Smart Level Selection Panel - Data-driven from WorldData
/// Dynamically creates level buttons based on available levels
/// </summary>
public class LevelSelectionPanel : MonoBehaviour
{
    [Header("World Data")]
    [SerializeField] private WorldData _currentWorld;

    [Header("Dynamic Level Generation")]
    [SerializeField] private Transform _levelButtonsParent; // The LevelsGrid container
    [SerializeField] private Button _levelButtonPrefab; // Prefab for level buttons

    [Header("Level Info Display")]
    [SerializeField] private TextMeshProUGUI _levelNameText;
    [SerializeField] private TextMeshProUGUI _bestScoreText;
    [SerializeField] private Image[] _levelStars; // 3 star images
    [SerializeField] private Button _playLevelButton;
    [SerializeField] private Button _backButton;

    [Header("World Info Display")]
    [SerializeField] private TextMeshProUGUI _worldNameText; // Shows current world name

    private IProgressionService _progressionService;
    private ISceneService _sceneService;

    // Events to communicate with MainMenuUI
    public event Action OnBackRequested;
    public event Action<bool> OnShowLoadingRequested; // true=show, false=hide

    // Runtime data
    private List<Button> _dynamicLevelButtons = new List<Button>();
    private int _selectedLevelIndex = 0;

    [Zenject.Inject]
    public void Construct(IProgressionService progressionService, ISceneService sceneService)
    {
        _progressionService = progressionService;
        _sceneService = sceneService;
    }
    private void Start()
    {
        SetupStaticButtons();
        ValidateWorldData();
    }

    private void OnDestroy()
    {
        CleanupButtons();
    }

    /// <summary>
    /// Called when this panel becomes active
    /// </summary>
    public void OnPanelShown()
    {
        RefreshPanel();
    }

    /// <summary>
    /// Refresh the entire panel - recreate buttons and update display
    /// </summary>
    public void RefreshPanel()
    {
        if (_currentWorld == null)
        {
            Debug.LogError("No WorldData assigned to LevelSelectionPanel!");
            return;
        }

        UpdateWorldDisplay();
        CreateLevelButtons();
        SelectLevel(0); // Auto-select first level
    }

    /// <summary>
    /// Update world information display
    /// </summary>
    private void UpdateWorldDisplay()
    {
        if (_worldNameText != null)
        {
            _worldNameText.text = _currentWorld.worldName;
        }
    }

    /// <summary>
    /// Dynamically create level buttons based on WorldData
    /// </summary>
    private void CreateLevelButtons()
    {
        // Clear existing buttons
        ClearDynamicButtons();

        int levelCount = _currentWorld.GetLevelCount();

        // Create button for each level in the world
        for (int i = 0; i < levelCount; i++)
        {
            CreateLevelButton(i);
        }
    }

    /// <summary>
    /// Create a single level button
    /// </summary>
    private void CreateLevelButton(int levelIndex)
    {
        if (_levelButtonPrefab == null || _levelButtonsParent == null)
        {
            Debug.LogError("Level button prefab or parent not assigned!");
            return;
        }

        // Instantiate button
        Button newButton = Instantiate(_levelButtonPrefab, _levelButtonsParent);

        // Set button text to level number
        TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.text = (levelIndex + 1).ToString(); // "1", "2", "3", etc.
        }

        // Set button state (locked/unlocked)
        bool isUnlocked = levelIndex < _progressionService.GetUnlockedLevels();
        newButton.interactable = isUnlocked;

        // Visual feedback for locked/unlocked
        Image buttonImage = newButton.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.color = isUnlocked ? Color.white : Color.gray;
        }

        // Add click listener
        int capturedIndex = levelIndex; // Capture for closure
        newButton.onClick.AddListener(() => OnLevelButtonClicked(capturedIndex));

        // Store reference
        _dynamicLevelButtons.Add(newButton);
    }

    /// <summary>
    /// Clear all dynamic level buttons
    /// </summary>
    private void ClearDynamicButtons()
    {
        foreach (Button button in _dynamicLevelButtons)
        {
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                DestroyImmediate(button.gameObject);
            }
        }
        _dynamicLevelButtons.Clear();
    }

    /// <summary>
    /// Select a specific level and update info display
    /// </summary>
    private void SelectLevel(int levelIndex)
    {
        if (!_currentWorld.HasLevel(levelIndex)) return;

        _selectedLevelIndex = levelIndex;
        UpdateLevelInfo(levelIndex);
    }

    /// <summary>
    /// Update the level information display using WorldData
    /// </summary>
    private void UpdateLevelInfo(int levelIndex)
    {
        // Level name from WorldData (already real)
        _levelNameText.text = _currentWorld.GetLevelName(levelIndex);

        // Real best score from progression service
        int bestScore = _progressionService.GetLevelBestScore(levelIndex);
        _bestScoreText.text = $"Best Score: {bestScore:N0}";

        // Real stars from progression service
        int stars = _progressionService.GetLevelStars(levelIndex);
        UpdateStarDisplay(stars);


        // Play button state using real unlocked levels
        int unlockedLevels = _progressionService.GetUnlockedLevels();
        bool canPlay = levelIndex < unlockedLevels;
        _playLevelButton.interactable = canPlay;
        _playLevelButton.GetComponentInChildren<TextMeshProUGUI>().text = canPlay ? "PLAY LEVEL" : "LOCKED";
    }

    /// <summary>
    /// Update star display
    /// </summary>
    private void UpdateStarDisplay(int stars)
    {
        for (int i = 0; i < _levelStars.Length; i++)
        {
            _levelStars[i].enabled = (i < stars);
            _levelStars[i].color = (i < stars) ? Color.yellow : Color.gray;
        }
    }



    // Button Event Handlers
    private void OnLevelButtonClicked(int levelIndex)
    {
        int unlockedLevels = _progressionService.GetUnlockedLevels();
        if (levelIndex < unlockedLevels)
        {
            SelectLevel(levelIndex);
        }
    }

    private void OnPlayLevelButtonClicked()
    {
        int unlockedLevels = _progressionService.GetUnlockedLevels();
        if (_selectedLevelIndex < unlockedLevels)
        {
            // Notify MainMenuUI to show loading, then load scene
            OnShowLoadingRequested?.Invoke(true);

            // Start scene loading after a brief delay (for loading screen to show)
            _sceneService.LoadGameplayScene(_selectedLevelIndex);
        }
    }
    private void OnBackButtonClicked()
    {
        OnBackRequested?.Invoke();
    }

    // Setup and Validation
    private void SetupStaticButtons()
    {
        _playLevelButton.onClick.AddListener(OnPlayLevelButtonClicked);
        _backButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void CleanupButtons()
    {
        _playLevelButton.onClick.RemoveAllListeners();
        _backButton.onClick.RemoveAllListeners();
        ClearDynamicButtons();
    }

    private void ValidateWorldData()
    {
        if (_currentWorld == null)
        {
            Debug.LogError($"WorldData not assigned to {gameObject.name}!");
        }

        if (_levelButtonPrefab == null)
        {
            Debug.LogError($"Level button prefab not assigned to {gameObject.name}!");
        }

        if (_levelButtonsParent == null)
        {
            Debug.LogError($"Level buttons parent not assigned to {gameObject.name}!");
        }
    }
}