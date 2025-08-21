using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Main Menu UI Coordinator - Just handles panel navigation, no detailed logic
/// Each panel manages its own functionality
/// </summary>
public class MainMenuUI : MonoBehaviour, IMainMenuUI
{
    [Header("Main Menu Panels")]
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _levelSelectionPanel;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _howToPlayPanel;
    [SerializeField] private GameObject _loadingPanel;

    [Header("Main Menu Buttons")]
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _howToPlayButton; 
    [SerializeField] private Button _exitButton;

    [Header("Panel Components")]
    [SerializeField] private LevelSelectionPanel _levelSelectionPanelScript;
    [SerializeField] private HowToPlayPanel _howToPlayPanelScript;
    [SerializeField] private SettingsPanel _settingsPanelScript;

    private void Start()
    {
        SetupButtonCallbacks();
        SetupPanelEvents();
        Show(); // Start with main menu visible
    }

    private void OnDestroy()
    {
        CleanupButtonCallbacks();
        CleanupPanelEvents();
    }

    // IMainMenuUI Interface Implementation
    public void Show()
    {
        _mainMenuPanel.SetActive(true);
        HideAllOtherPanels();
    }

    public void Hide()
    {
        _mainMenuPanel.SetActive(false);
    }

    public void ShowLevelSelection()
    {
        _levelSelectionPanel.SetActive(true);
        _mainMenuPanel.SetActive(false);

        // Tell the panel it's now visible so it can refresh
        _levelSelectionPanelScript.OnPanelShown();
    }

    public void ShowSettings()
    {
        _settingsPanel.SetActive(true);
        _mainMenuPanel.SetActive(false);
    }

    public void ShowHowToPlay()
    {
        if (_howToPlayPanel != null) _howToPlayPanel.SetActive(true);
        if (_mainMenuPanel != null) _mainMenuPanel.SetActive(false);
        //_howToPlayPanelScript?.Show();
    }


    public void SetLoadingState(bool isLoading)
    {
        _loadingPanel.SetActive(isLoading);

        if (isLoading)
        {
            // Hide all other panels when loading
            _mainMenuPanel.SetActive(false);
            _levelSelectionPanel.SetActive(false);
            _settingsPanel.SetActive(false);
            _howToPlayPanel.SetActive(false);
                }
    }

    // Main Menu Button Handlers
    private void OnPlayButtonClicked()
    {
        ShowLevelSelection();
    }

    private void OnSettingsButtonClicked()
    {
        ShowSettings();
    }

    private void OnHowToPlayButtonClicked()
    {
        ShowHowToPlay();
    }
    private void OnExitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Panel Event Handlers (panels tell us what to do)
    private void OnLevelSelectionBackRequested()
    {
        Show(); // Go back to main menu
    }
    private void OnHowToPlayBackRequested()
    {
        Show(); 
    }
    private void OnSettingsBackRequested()
    {
        Show(); 
    }
    // Helper Methods
    private void HideAllOtherPanels()
    {
        _levelSelectionPanel.SetActive(false);
        _settingsPanel.SetActive(false);
        _howToPlayPanel.SetActive(false);
        _loadingPanel.SetActive(false);
    }

    private void SetupButtonCallbacks()
    {
        _playButton.onClick.AddListener(OnPlayButtonClicked);
        if (_howToPlayButton != null)
            _howToPlayButton.onClick.AddListener(OnHowToPlayButtonClicked);
        if (_settingsButton != null)
            _settingsButton.onClick.AddListener(OnSettingsButtonClicked);

        _exitButton.onClick.AddListener(OnExitButtonClicked);
    }

    private void CleanupButtonCallbacks()
    {
        _playButton.onClick.RemoveAllListeners();
        _settingsButton.onClick.RemoveAllListeners();
        _howToPlayButton.onClick.RemoveAllListeners();
        _exitButton.onClick.RemoveAllListeners();
    }

    private void SetupPanelEvents()
    {
        // Subscribe to level selection panel events
        _levelSelectionPanelScript.OnBackRequested += OnLevelSelectionBackRequested;
        _levelSelectionPanelScript.OnShowLoadingRequested += SetLoadingState;

        if (_howToPlayPanelScript != null)
            _howToPlayPanelScript.OnBackRequested += OnHowToPlayBackRequested;
        if (_settingsPanelScript != null)
            _settingsPanelScript.OnBackRequested += OnSettingsBackRequested;
    }

    private void CleanupPanelEvents()
    {
        // Unsubscribe from panel events
        _levelSelectionPanelScript.OnBackRequested -= OnLevelSelectionBackRequested;
        _levelSelectionPanelScript.OnShowLoadingRequested -= SetLoadingState;

        if (_howToPlayPanelScript != null)
            _howToPlayPanelScript.OnBackRequested -= OnHowToPlayBackRequested;
        if (_settingsPanelScript != null)
            _settingsPanelScript.OnBackRequested -= OnSettingsBackRequested;
    }
}