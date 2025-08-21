using System;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayPanel : MonoBehaviour
{
    [SerializeField] private Button _backButton;

    public event Action OnBackRequested;

    private void Awake()
    {
        if (_backButton != null)
            _backButton.onClick.AddListener(Back);
    }

    private void OnDestroy()
    {
        if (_backButton != null)
            _backButton.onClick.RemoveListener(Back);
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);

    private void Back()
    {
        Hide();
        OnBackRequested?.Invoke();
    }
}
