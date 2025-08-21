using UnityEngine;

[DisallowMultipleComponent]
public class ParallaxLayer : MonoBehaviour
{
    [Header("Auto scroll (texture UV units/sec)")]
    public Vector2 autoScroll = new Vector2(0f, 0f);   // e.g., Nebula: (0.10, 0.01)

    [Header("Parallax by target (player)")]
    public string targetTag = "Player";
    [Range(0f, 1f)] public float targetFactorX = 0.0f;
    public float maxSwayX = 0.8f;
    public float smoothTime = 0.08f;

    [Header("Parallax by camera (optional)")]
    public bool useCamera = false;
    [Range(0f, 1f)] public float cameraFactorX = 0.0f;

    private Transform _target;
    private Transform _cam;
    private Vector3 _basePos;
    private float _velocityX;
    private Renderer _renderer;
    private Vector2 _uvOffset = Vector2.zero;

    void Start()
    {
        _basePos = transform.position;
        _renderer = GetComponent<Renderer>();

        if (!string.IsNullOrEmpty(targetTag))
        {
            var go = GameObject.FindGameObjectWithTag(targetTag);
            if (go) _target = go.transform;
        }

        if (useCamera && Camera.main)
            _cam = Camera.main.transform;
    }

    void Update()
    {
        float dt = Time.deltaTime;

        // 1) Auto UV scroll (infinite background)
        if (_renderer != null && _renderer.material != null)
        {
            _uvOffset += autoScroll * dt;
            _renderer.material.mainTextureOffset = _uvOffset;
        }

        // 2) Target-based sway
        float swayX = 0f;
        if (_target && targetFactorX > 0f)
        {
            float desired = Mathf.Clamp(_target.position.x * targetFactorX, -maxSwayX, maxSwayX);
            swayX = Mathf.SmoothDamp(transform.position.x - _basePos.x, desired, ref _velocityX, smoothTime);
        }

        // 3) Camera-based parallax
        if (_cam && useCamera && cameraFactorX > 0f)
        {
            float camOffsetX = _cam.position.x * cameraFactorX;
            transform.position = new Vector3(_basePos.x + swayX + camOffsetX, _basePos.y, _basePos.z);
        }
        else
        {
            transform.position = new Vector3(_basePos.x + swayX, _basePos.y, _basePos.z);
        }
    }
}
