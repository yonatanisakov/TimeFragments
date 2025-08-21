using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(SpriteRenderer))]
public class StarTwinkleSimple : MonoBehaviour
{
    [Header("Alpha range")]
    [Range(0f, 1f)] public float minAlpha = 0.30f;
    [Range(0f, 1f)] public float maxAlpha = 1.00f;

    [Header("Speed / randomness")]
    [Tooltip("Average twinkles per second.")]
    public float twinklesPerSecond = 0.5f;
    [Tooltip("How irregular the twinkle timing is (0 = perfectly even, 1 = very irregular).")]
    [Range(0f, 1f)] public float randomness = 0.35f;

    [Header("Micro jitter (optional)")]
    public float jitterAmplitude = 0.03f;   // e.g., 0.02f to enable
    public float jitterFrequency = 0.25f;

    [Header("Color target (material vs sprite)")]
    [Tooltip("Enable if your stars use a URP Unlit Transparent material (controls _BaseColor alpha). " +
             "Disable if you use Sprites/Default and want to change SpriteRenderer.color.")]
    public bool useMaterialBaseColor = false;

    SpriteRenderer _sr;
    Material _mat;
    static readonly int _BaseColorID = Shader.PropertyToID("_BaseColor");

    Color _baseColor;   // starting tint (without alpha)
    Vector3 _baseLocalPos;
    float _phase;       // random offset
    float _freq;        // actual frequency per object

    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _baseLocalPos = transform.localPosition;

        var rng = new System.Random(GetInstanceID());
        _phase = (float)rng.NextDouble() * Mathf.PI * 2f;

        float f = Mathf.Max(0.05f, twinklesPerSecond);
        float jitter = Mathf.Lerp(1f, 1f + randomness, (float)rng.NextDouble());
        _freq = f * jitter;

        if (useMaterialBaseColor)
        {
            _mat = _sr.material; // instance at runtime
            if (_mat.HasProperty(_BaseColorID))
            {
                var c = _mat.GetColor(_BaseColorID);
                _baseColor = new Color(c.r, c.g, c.b, 1f);
            }
        }
        else
        {
            var c = _sr.color;
            _baseColor = new Color(c.r, c.g, c.b, 1f);
        }
    }

    void Update()
    {
        // visible, snappy twinkle: ease-in-out between min/max using a sine
        float t = Time.time;
        float s = 0.5f * (1f + Mathf.Sin((t + _phase) * (Mathf.PI * 2f) * _freq)); // 0..1
        float a = Mathf.Lerp(minAlpha, maxAlpha, s);

        if (useMaterialBaseColor && _mat != null && _mat.HasProperty(_BaseColorID))
        {
            var c = _baseColor; c.a = a; _mat.SetColor(_BaseColorID, c);
        }
        else
        {
            var c = _baseColor; c.a = a; _sr.color = c;
        }

        if (jitterAmplitude > 0f)
        {
            float jx = Mathf.Sin(t * jitterFrequency + _phase) * jitterAmplitude;
            float jy = Mathf.Cos((t + 1.234f) * jitterFrequency + _phase) * (jitterAmplitude * 0.5f);
            transform.localPosition = _baseLocalPos + new Vector3(jx, jy, 0f);
        }
    }
}
