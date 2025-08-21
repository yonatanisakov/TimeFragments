using UnityEngine;

[DisallowMultipleComponent]
public class PulseGlow : MonoBehaviour
{
    [Range(0f, 0.2f)] public float scaleAmp = 0.06f;  // how much to grow/shrink
    [Range(0f, 1f)] public float alphaAmp = 0.25f;  // how much to vary alpha
    [Range(0.1f, 6f)] public float speed = 2.2f;      // cycles per second-ish

    SpriteRenderer _sr;
    Color _baseColor;
    Vector3 _baseScale;

    void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _baseColor = _sr.color;
        _baseScale = transform.localScale;
    }

    void Update()
    {
        float t = (Mathf.Sin(Time.time * speed) + 1f) * 0.5f; // 0..1
        transform.localScale = _baseScale * (1f + t * scaleAmp);

        var c = _baseColor;
        // center the alpha around base and pulse it up/down
        c.a = _baseColor.a * (1f - alphaAmp * 0.5f + t * alphaAmp);
        _sr.color = c;
    }
}
