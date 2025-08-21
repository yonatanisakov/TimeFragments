public interface IAudioService
{
    void PlayMusic(UnityEngine.AudioClip clip, bool loop = true, float fadeSeconds = 0.25f);
    void StopMusic(float fadeSeconds = 0.25f);

    void PlaySfx(UnityEngine.AudioClip clip, float volume = 1f, float pitchJitter = 0f);
    void PlayUi(UnityEngine.AudioClip clip, float volume = 1f, float pitchJitter = 0f);
}
