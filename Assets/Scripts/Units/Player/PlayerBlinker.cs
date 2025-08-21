using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventBusScripts;

[DisallowMultipleComponent]
public class PlayerBlinker : MonoBehaviour
{
    [Header("Blink look")]
    [Range(0.5f, 30f)] public float blinkHz = 10f;
    [Range(0f, 1f)] public float minAlpha = 0.25f;

    SpriteRenderer[] _renders;
    MaterialPropertyBlock _mpb;
    static readonly int _colorID = Shader.PropertyToID("_Color");

    Coroutine _blinkCo;
    float _blinkUntil; 

    void Awake()
    {
        _renders = GetComponentsInChildren<SpriteRenderer>(true);
        _mpb = new MaterialPropertyBlock();
    }

    void OnEnable()
    {
        EventBus.Get<PlayerInvulnerabilityStartedEvent>().Subscribe(OnInvulnStarted);
    }

    void OnDisable()
    {
        EventBus.Get<PlayerInvulnerabilityStartedEvent>().Unsubscribe(OnInvulnStarted);
        StopAllCoroutines();
        SetAlpha(1f);
    }

    void OnInvulnStarted(float duration)
    {
        _blinkUntil = Time.time + duration;

        if (_blinkCo == null)
            _blinkCo = StartCoroutine(BlinkRoutine());
    }

    IEnumerator BlinkRoutine()
    {
        while (Time.time < _blinkUntil)
        {
            float t = (Mathf.Sin(Time.time * Mathf.PI * 2f * blinkHz) * 0.5f + 0.5f);
            float a = Mathf.Lerp(minAlpha, 1f, t);
            SetAlpha(a);
            yield return null;
        }

        SetAlpha(1f);
        _blinkCo = null;
    }

    void SetAlpha(float a)
    {
        for (int i = 0; i < _renders.Length; i++)
        {
            var sr = _renders[i];
            if (!sr) continue;

            // שומרים Tint קיים, משנים רק אלפא דרך PropertyBlock (בלי ליצור instance של Material)
            var c = sr.color; c.a = a;
            _mpb.Clear();
            _mpb.SetColor(_colorID, c);
            sr.SetPropertyBlock(_mpb);
        }
    }
}
