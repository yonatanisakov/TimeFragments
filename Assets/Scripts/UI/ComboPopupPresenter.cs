using EventBusScripts;
using UnityEngine;
using Zenject;

public class ComboPopupPresenter : IInitializable, System.IDisposable
{
    private readonly IUIService _ui;

    [InjectOptional] private readonly IPlayerMovement _playerMovement; 
    private Transform _playerTransform;

    private float _lastMult = 1f;
    public ComboPopupPresenter(IUIService uiService)
    {
        _ui = uiService;
    }

    public void Initialize()
    {
        if (_playerMovement is Component c)
            _playerTransform = c.transform;
        else
        {
            var go = GameObject.FindGameObjectWithTag("Player");
            _playerTransform = go ? go.transform : null;
        }

        Debug.Log($"[ComboPopupPresenter] playerTransform? {_playerTransform != null}");

        EventBus.Get<ComboMultiplierChangedEvent>().Subscribe(OnComboChanged);
    }

    public void Dispose()
    {
        EventBus.Get<ComboMultiplierChangedEvent>().Unsubscribe(OnComboChanged);
    }

    private void OnComboChanged(float mult)
    {
        Debug.Log($"[ComboPopupPresenter] OnComboChanged mult={mult} last={_lastMult}");
        if (_playerTransform == null) return;

        if (mult > 1f)
        {
            Debug.Log("[ComboPopupPresenter] SHOW POPUP");
            _ui.ShowFloatingTextFollow($"x{mult:0.##}", _playerTransform, Color.white);
        }
        _lastMult = mult;
    }
}
