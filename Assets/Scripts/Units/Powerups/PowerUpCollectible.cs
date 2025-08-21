using EventBusScripts;
using System.Collections;
using UnityEngine;
using Zenject;

/// <summary>
/// MonoBehaviour component for power-up collectibles that spawn in the game world
/// Simple collectible that can be picked up by the player
/// </summary>
public class PowerUpCollectible : MonoBehaviour
{
    [Header("Power-up Configuration")]
    [SerializeField] private PowerUpType _powerUpType = PowerUpType.Shield;
    [SerializeField] private float _collectRadius = 0.5f;

    [Header("Visual Settings")]
    [SerializeField] private float _rotationSpeed = 45f;
    [SerializeField] private float _bobSpeed = 2f;
    [SerializeField] private float _bobDistance = 0.2f;

    [Header("Visual Border")]
    [SerializeField] private SpriteRenderer _borderRenderer;
    [SerializeField] private Color _goodPowerUpBorderColor = Color.green;
    [SerializeField] private Color _badPowerUpBorderColor = Color.red;
    [Header("Collect FX")]
    [SerializeField] private bool _flyToUI = true;
    [SerializeField] private string _uiAnchorTag = "PowerUpUIAnchor";
    [SerializeField] private float _burstScale = 1.3f;      
    [SerializeField] private float _burstDuration = 0.12f;  
    [SerializeField] private float _flyDuration = 0.45f;   
    [SerializeField] private AnimationCurve _burstEase = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private AnimationCurve _flyEase = AnimationCurve.EaseInOut(0, 0, 1, 1);

    bool _isCollectingFx;
    private Vector3 _startPosition;
    private SpriteRenderer _spriteRenderer;
    private float _baseHalfHeight;
    private float _baseHalfWidth;
    private bool _magnetActive = false;
    private Vector3 _playerPosition;
    private float _magnetForce;
    private float _magnetRange;
    private PowerUpConfig _powerUpConfig;
    // Lifetime management - dependency injection
    private IPowerUpLifetimeManager _lifetimeManager;

    // Lifetime state tracking
    private bool _isRegisteredWithLifetime = false;
    private bool _isInWarningState = false;

    // Visual effects for lifetime warnings
    private Color _originalColor;
    private Color _originalBorderColor;

    [Header("Lifetime Warning Effects")]
    [SerializeField] private float _blinkAlpha = 0.3f;
    [SerializeField] private Color _warningTint = Color.red;
    public PowerUpType PowerUpType => _powerUpType;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        _startPosition = transform.position;
        CalculateBaseBounds();


        // Ensure we have a trigger collider for collection
        var collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
        // Subscribe to magnet events
        EventBus.Get<MagnetActivatedEvent>().Subscribe(OnMagnetActivated);
        EventBus.Get<MagnetDeactivatedEvent>().Subscribe(OnMagnetDeactivated);
        EventBus.Get<MagnetUpdateEvent>().Subscribe(OnMagnetUpdate);
        // Store original colors for lifetime warning effects
        if (_spriteRenderer != null)
            _originalColor = _spriteRenderer.color;
        if (_borderRenderer != null)
            _originalBorderColor = _borderRenderer.color;

        // Subscribe to lifetime events
        EventBus.Get<PowerUpLifetimeWarningEvent>().Subscribe(OnLifetimeWarning);
        EventBus.Get<PowerUpLifetimeBlinkEvent>().Subscribe(OnLifetimeBlink);
        EventBus.Get<PowerUpLifetimeExpiredEvent>().Subscribe(OnLifetimeExpired);
    }

    void Update()
    {
        // Handle magnet effect first
        if (_magnetActive)
        {
            ApplyMagnetEffect();
            return; // Skip normal bobbing when being pulled by magnet
        }
        // Simple visual effects - rotation and bobbing
        transform.Rotate(0, 0, _rotationSpeed * Time.deltaTime);

        var bobOffset = Mathf.Sin(Time.time * _bobSpeed) * _bobDistance;
        transform.position = _startPosition + Vector3.right * bobOffset;
    }

    /// <summary>
    /// Configure this power-up collectible with a specific type
    /// Used when spawning from the PowerUpDrop system
    /// </summary>
    /// <param name="powerUpType">Type of power-up this collectible represents</param>
    /// <param name="icon">Sprite to display (optional)</param>
    public void Configure(PowerUpType powerUpType, PowerUpConfig powerUpConfig,IPowerUpLifetimeManager powerUpLifetimeManager, float? customLifetime = null)
    {
        _powerUpType = powerUpType;
        _powerUpConfig = powerUpConfig;
        _lifetimeManager = powerUpLifetimeManager;
        var icon = _powerUpConfig.GetIcon(_powerUpType);
        Debug.Log("ICON = " + icon);
        Debug.Log("SPRITE RENEDERER = "+ _spriteRenderer);
        if (icon != null && _spriteRenderer != null)
        {
            _spriteRenderer.sprite = icon;
        }
        SetBorderColor();
        // Register with lifetime manager if available
        if (_lifetimeManager != null && !_isRegisteredWithLifetime)
        {
            _lifetimeManager.RegisterPowerUp(this, customLifetime);
            _isRegisteredWithLifetime = true;
        }
    }
    /// <summary>
    /// Get effective half height considering rotation
    /// </summary>
    public float EffectiveHalfHeight
    {
        get
        {
            CalculateBaseBounds();
            return GetEffectiveBounds().y;
        }
    }
    /// <summary>
    /// Get effective half width considering rotation AND bobbing
    /// </summary>
    public float EffectiveHalfWidth
    {
        get
        {
            CalculateBaseBounds();
            return GetEffectiveBounds().x + _bobDistance; // Add bobbing distance
        }
    }
    /// <summary>
    /// Calculate base sprite bounds (without rotation)
    /// </summary>
    private void CalculateBaseBounds()
    {
        if (_baseHalfHeight > 0 && _baseHalfWidth > 0) return; // Already calculated

        if (_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();

        if (_spriteRenderer != null && _spriteRenderer.sprite != null)
        {
            // Use sprite size, not bounds (bounds change with rotation)
            var spriteSize = _spriteRenderer.sprite.bounds.size;
            _baseHalfHeight = spriteSize.y / 2f;
            _baseHalfWidth = spriteSize.x / 2f;
        }
        else
        {
            _baseHalfHeight = 0.5f; // Default fallback
            _baseHalfWidth = 0.5f;  // Default fallback
        }
    }

    /// <summary>
    /// Calculate effective bounds considering current rotation
    /// Returns the maximum possible width/height the sprite can occupy
    /// </summary>
    private Vector2 GetEffectiveBounds()
    {
        // For any rotation, the worst case is 45° where both width and height contribute
        // Effective size = sqrt(width² + height²) - but this is overkill

        // Simpler approach: use the larger of width/height as both dimensions
        // This ensures the power-up never goes out of bounds regardless of rotation
        var maxDimension = Mathf.Max(_baseHalfWidth, _baseHalfHeight);

        return new Vector2(maxDimension, maxDimension);
    }
    /// <summary>
    /// Called when the collectible is picked up
    /// </summary>
    public void Collect()
    {
        if (_isCollectingFx) return;
        _isCollectingFx = true;
        // Unregister from lifetime manager
        if (_lifetimeManager != null && _isRegisteredWithLifetime)
        {
            _lifetimeManager.UnregisterPowerUp(this);
            _isRegisteredWithLifetime = false;
        }
        var col = GetComponent<Collider2D>();
        if (col) col.enabled = false;

        StartCoroutine(CollectSequence());

    }
    private void SetBorderColor()
    {
        if (_borderRenderer == null) return;

        bool isGoodPowerUp = PowerUpTypeHelper.IsGoodPowerUp(_powerUpType);
        _borderRenderer.color = isGoodPowerUp ? _goodPowerUpBorderColor : _badPowerUpBorderColor;

        Debug.Log($"Power-up border set: {_powerUpType} = {(isGoodPowerUp ? "Good (Green)" : "Bad (Red)")}");
    }
    private void OnMagnetActivated(MagnetData magnetData)
    {
        _magnetForce = magnetData.force;
        _magnetRange = magnetData.range;
        _magnetActive = true;
    }

    private void OnMagnetDeactivated()
    {
        _magnetActive = false;
        _startPosition = transform.position; // Update start position for bobbing
    }

    private void OnMagnetUpdate(MagnetData magnetData)
    {
        if (_magnetActive)
        {
            var player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                _playerPosition = player.transform.position;
            }
        }
    }

    private void ApplyMagnetEffect()
    {
        var distanceToPlayer = Vector3.Distance(transform.position, _playerPosition);

        // Only pull if within range
        if (distanceToPlayer <= _magnetRange && distanceToPlayer > 0.1f)
        {
            var direction = (_playerPosition - transform.position).normalized;
            var pullForce = _magnetForce * Time.deltaTime;

            transform.position += direction * pullForce;
        }
    }
    #region Lifetime Event Handlers

    /// <summary>
    /// Handle lifetime warning state - called when power-up is about to expire
    /// Changes visual appearance to warn player
    /// </summary>
    private void OnLifetimeWarning(PowerUpLifetimeWarningData warningData)
    {
        // Only respond to our own warning
        if (warningData.powerUp != this) return;

        _isInWarningState = true;

        // Apply warning tint to main sprite
        if (_spriteRenderer != null)
        {
            _spriteRenderer.color = Color.Lerp(_originalColor, _warningTint, 0.3f);
        }

        Debug.Log($"Power-up {_powerUpType} entering warning state: {warningData.remainingTime:F1}s remaining");
    }

    /// <summary>
    /// Handle blink effect during warning period
    /// Toggles transparency to create urgency
    /// </summary>
    private void OnLifetimeBlink(PowerUpCollectible blinkingPowerUp)
    {
        // Only respond to our own blink
        if (blinkingPowerUp != this || !_isInWarningState) return;

        // Toggle alpha between normal and blink alpha
        if (_spriteRenderer != null)
        {
            var currentAlpha = _spriteRenderer.color.a;
            var targetAlpha = Mathf.Approximately(currentAlpha, 1f) ? _blinkAlpha : 1f;

            var currentColor = _spriteRenderer.color;
            currentColor.a = targetAlpha;
            _spriteRenderer.color = currentColor;
        }

        // Also blink border if available
        if (_borderRenderer != null)
        {
            var currentAlpha = _borderRenderer.color.a;
            var targetAlpha = Mathf.Approximately(currentAlpha, 1f) ? _blinkAlpha : 1f;

            var currentColor = _borderRenderer.color;
            currentColor.a = targetAlpha;
            _borderRenderer.color = currentColor;
        }
    }

    /// <summary>
    /// Handle lifetime expiry - called just before destruction
    /// Could add particle effects, sound, etc.
    /// </summary>
    private void OnLifetimeExpired(PowerUpCollectible expiredPowerUp)
    {
        // Only respond to our own expiry
        if (expiredPowerUp != this) return;

        Debug.Log($"Power-up {_powerUpType} expired via lifetime manager");

        // The lifetime manager will destroy this GameObject
        // This handler exists for potential final effects before destruction
    }

    private IEnumerator CollectSequence()
    {
        var sr = _spriteRenderer;
        var border = _borderRenderer;

        Vector3 startScale = transform.localScale;
        Vector3 burstScale = startScale * _burstScale;

        // 1) Scale Burst
        float t = 0f;
        while (t < _burstDuration)
        {
            t += Time.deltaTime;
            float k = _burstEase.Evaluate(Mathf.Clamp01(t / _burstDuration));
            transform.localScale = Vector3.LerpUnclamped(startScale, burstScale, k);
            yield return null;
        }

        // 2) Fly-to-UI (àåôöéåðìé)
        if (_flyToUI)
        {
            Camera cam = Camera.main;
            var anchorGo = GameObject.FindWithTag(_uiAnchorTag);
            if (cam != null && anchorGo != null)
            {
                // äîøä î-UI (îñê) ìòåìí
                Vector3 screenPos = anchorGo.transform.position; // òåáã âí á-Screen Space Overlay/Camera
                Vector3 targetWorld = cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, -cam.transform.position.z));
                targetWorld.z = transform.position.z;

                Vector3 fromPos = transform.position;
                Vector3 fromScale = transform.localScale;

                Color srFrom = sr ? sr.color : Color.white;
                Color borderFrom = border ? border.color : Color.white;

                t = 0f;
                while (t < _flyDuration)
                {
                    t += Time.deltaTime;
                    float k = _flyEase.Evaluate(Mathf.Clamp01(t / _flyDuration));

                    transform.position = Vector3.LerpUnclamped(fromPos, targetWorld, k);
                    transform.localScale = Vector3.LerpUnclamped(fromScale, startScale * 0.6f, k);

                    if (sr)
                    {
                        var c = srFrom; c.a = Mathf.Lerp(1f, 0f, k);
                        sr.color = c;
                    }
                    if (border)
                    {
                        var c2 = borderFrom; c2.a = Mathf.Lerp(1f, 0.2f, k);
                        border.color = c2;
                    }
                    yield return null;
                }
            }
        }

        // 3) ñéåí
        Destroy(gameObject);
    }

    #endregion
    private void OnDestroy()
    {
        // Unregister from lifetime manager
        if (_lifetimeManager != null && _isRegisteredWithLifetime)
        {
            _lifetimeManager.UnregisterPowerUp(this);
        }

        // Unsubscribe from magnet events
        EventBus.Get<MagnetActivatedEvent>().Unsubscribe(OnMagnetActivated);
        EventBus.Get<MagnetDeactivatedEvent>().Unsubscribe(OnMagnetDeactivated);
        EventBus.Get<MagnetUpdateEvent>().Unsubscribe(OnMagnetUpdate);

        // Unsubscribe from lifetime events
        EventBus.Get<PowerUpLifetimeWarningEvent>().Unsubscribe(OnLifetimeWarning);
        EventBus.Get<PowerUpLifetimeBlinkEvent>().Unsubscribe(OnLifetimeBlink);
        EventBus.Get<PowerUpLifetimeExpiredEvent>().Unsubscribe(OnLifetimeExpired);
    }
}