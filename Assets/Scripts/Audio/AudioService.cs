using UnityEngine;
using Zenject;
using System.Collections;

public class AudioService : IAudioService
{
    AudioSource _music;
    AudioSource _sfx;
    AudioSource _ui;

    void ResolveSources()
    {
        if (_music == null) _music = FindByTag("MusicAudio");
        if (_sfx == null) _sfx = FindByTag("SFXAudio");
        if (_ui == null) _ui = FindByTag("UIAudio");
    }

    AudioSource FindByTag(string tag)
    {
        var go = GameObject.FindWithTag(tag);
        return go ? go.GetComponent<AudioSource>() : null;
    }

    public void PlayMusic(AudioClip clip, bool loop = true, float fadeSeconds = 0.25f)
    {
        if (clip == null) return;
        ResolveSources();
        if (_music == null) return;

        _music.loop = loop;

        // crossfade чип
        _music.Stop();
        _music.clip = clip;
        _music.volume = 0f;
        _music.Play();
        if (fadeSeconds > 0f)
            _music.gameObject.GetComponent<MonoBehaviour>()?.StartCoroutine(FadeTo(_music, 1f, fadeSeconds));
        else
            _music.volume = 1f;
    }

    public void StopMusic(float fadeSeconds = 0.25f)
    {
        ResolveSources();
        if (_music == null || !_music.isPlaying) return;

        if (fadeSeconds > 0f)
            _music.gameObject.GetComponent<MonoBehaviour>()?.StartCoroutine(FadeTo(_music, 0f, fadeSeconds, stopAtEnd: true));
        else
            _music.Stop();
    }

    public void PlaySfx(AudioClip clip, float volume = 1f, float pitchJitter = 0f)
    {
        if (clip == null) return;
        ResolveSources();
        if (_sfx == null) return;

        float basePitch = 1f;
        _sfx.pitch = basePitch + Random.Range(-pitchJitter, pitchJitter);
        _sfx.PlayOneShot(clip, volume);
        _sfx.pitch = 1f;
    }

    public void PlayUi(AudioClip clip, float volume = 1f, float pitchJitter = 0f)
    {
        if (clip == null) return;
        ResolveSources();
        if (_ui == null) return;

        float basePitch = 1f;
        _ui.pitch = basePitch + Random.Range(-pitchJitter, pitchJitter);
        _ui.PlayOneShot(clip, volume);
        _ui.pitch = 1f;
    }

    System.Collections.IEnumerator FadeTo(AudioSource src, float target, float seconds, bool stopAtEnd = false)
    {
        float start = src.volume;
        float t = 0f;
        while (t < seconds)
        {
            t += Time.unscaledDeltaTime; 
            src.volume = Mathf.Lerp(start, target, t / seconds);
            yield return null;
        }
        src.volume = target;
        if (stopAtEnd && target <= 0f) src.Stop();
    }
}
