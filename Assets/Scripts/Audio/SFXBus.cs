// SFXBus.cs
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SFXBus : MonoBehaviour
{
    public static SFXBus I { get; private set; }
    public enum SelectMode { Random, Sequential }

    [Header("Shoot")]
    [SerializeField] private AudioClip[] shootClips;
    [SerializeField] private SelectMode shootMode = SelectMode.Random;

    [Header("Impacts")]
    [SerializeField] private AudioClip[] fragmentHitClips;
    [SerializeField] private AudioClip playerHitClip;

    [Header("PowerUps")]
    [SerializeField] private AudioClip powerupPickupClip;
    [SerializeField] private AudioClip powerupExpireClip;

    [Header("Playback (global)")]
    [Range(0f, 1f)][SerializeField] private float volume = 1f;
    [Range(0f, 0.25f)][SerializeField] private float pitchJitter = 0.05f;
    [SerializeField] private float minShotInterval = 0.03f;

    // --- New: per-clip volume & cooldown for expire beep ---
    [Header("PowerUp Expire settings")]
    [Range(0f, 1f)][SerializeField] private float powerupExpireVolume = 0.25f;
    [SerializeField] private float powerupExpireCooldown = 0.2f;
    private float _lastPowerupExpire = -999f;

    private AudioSource _src;
    private int _shootIdx = 0;
    private float _lastShotTime = -999f;

    void Awake()
    {
        if (I != null && I != this) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);

        _src = GetComponent<AudioSource>();
        _src.playOnAwake = false;
        _src.loop = false;
        _src.spatialBlend = 0f; // 2D
    }


    public void PlayShoot()
    {
        if (Time.unscaledTime - _lastShotTime < minShotInterval) return;
        _lastShotTime = Time.unscaledTime;
        PlayFromSet(shootClips, shootMode);
    }

    public void PlayPlayerHit() => PlayOne(playerHitClip);
    public void PlayFragmentHit() => PlayFromSet(fragmentHitClips, SelectMode.Random);
    public void PlayPowerupPickup() => PlayOne(powerupPickupClip);

    public void PlayPowerupExpire()
    {
        if (!powerupExpireClip) return;

        if (Time.timeScale == 0f) return;
        if (Time.unscaledTime - _lastPowerupExpire < powerupExpireCooldown) return;
        _lastPowerupExpire = Time.unscaledTime;

        PlayWithJitter(powerupExpireClip, powerupExpireVolume);
    }

    public void StopAll()
    {
        if (_src && _src.isPlaying) _src.Stop();
    }

    void PlayFromSet(AudioClip[] set, SelectMode mode)
    {
        if (set == null || set.Length == 0) return;
        var clip = (mode == SelectMode.Random)
            ? set[Random.Range(0, set.Length)]
            : set[(_shootIdx++) % set.Length];
        PlayWithJitter(clip, volume);
    }

    void PlayOne(AudioClip c)
    {
        if (!c) return;
        PlayWithJitter(c, volume);
    }

    void PlayWithJitter(AudioClip c, float vol)
    {
        var jitter = Random.Range(-pitchJitter, pitchJitter);
        _src.pitch = 1f + jitter;
        _src.PlayOneShot(c, Mathf.Clamp01(vol));
        _src.pitch = 1f;
    }
}
