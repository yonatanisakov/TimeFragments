using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseWindowUI : MonoBehaviour
{
    [Header("Root")]
    [SerializeField] private GameObject _root;

    [Header("Buttons")]
    [SerializeField] private Button _resumeBtn;
    [SerializeField] private Button _restartBtn;
    [SerializeField] private Button _settingsBtn;
    [SerializeField] private Button _quitBtn;

    [Inject] private IAudioService _audio;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip _openClip;
    [SerializeField] private AudioClip _closeClip;
    public bool IsOpen => _root != null && _root.activeSelf;

    void Awake()
    {
        if (_root == null) _root = gameObject;
        ClearListeners();
    }

    public void Show(
        Action onResume,
        Action onRestart,
        Action onSettings,
        Action onQuit)
    {
        if (_root == null) _root = gameObject;
        _root.SetActive(true);
        _audio?.PlayUi(_openClip);

        ClearListeners();

        if (_resumeBtn) _resumeBtn.onClick.AddListener(() => onResume?.Invoke());
        if (_restartBtn) _restartBtn.onClick.AddListener(() => onRestart?.Invoke());
        if (_settingsBtn) _settingsBtn.onClick.AddListener(() => onSettings?.Invoke());
        if (_quitBtn) _quitBtn.onClick.AddListener(() => onQuit?.Invoke());
    }

    public void Hide()
    {
        if (_root == null) return;
        gameObject.SetActive(false);
        _audio?.PlayUi(_closeClip);
        _root.SetActive(false);
        ClearListeners();
    }

    private void ClearListeners()
    {
        if (_resumeBtn) _resumeBtn.onClick.RemoveAllListeners();
        if (_restartBtn) _restartBtn.onClick.RemoveAllListeners();
        if (_settingsBtn) _settingsBtn.onClick.RemoveAllListeners();
        if (_quitBtn) _quitBtn.onClick.RemoveAllListeners();
    }
}
