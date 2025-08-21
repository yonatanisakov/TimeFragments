using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
public class UIButtonSfx : MonoBehaviour,
    IPointerEnterHandler, IPointerClickHandler,
    ISelectHandler, ISubmitHandler
{
    [Header("Clips")]
    [SerializeField] private AudioClip hoverClip;
    [SerializeField] private AudioClip clickClip;
    [SerializeField] private AudioClip lockedClip;


    [Header("Lookup")]
    [SerializeField] private string uiAudioTag = "UIAudio";

    private AudioSource uiSource;

    void Awake() => ResolveSource();

    private void ResolveSource()
    {
        if (uiSource != null) return;

        var go = GameObject.FindWithTag(uiAudioTag);
        if (go != null) uiSource = go.GetComponent<AudioSource>();

        if (uiSource == null)
        {
#if UNITY_2022_3_OR_NEWER || UNITY_6000_0_OR_NEWER
            uiSource = Object.FindFirstObjectByType<AudioSource>();
#else
            uiSource = Object.FindObjectOfType<AudioSource>();
#endif
        }
    }

    private void Play(AudioClip clip)
    {
        if (clip == null) return;
        if (uiSource == null) ResolveSource();
        uiSource?.PlayOneShot(clip);
    }

    // Mouse / Pointer
    public void OnPointerEnter(PointerEventData e) => Play(hoverClip);
    public void OnPointerClick(PointerEventData e)
    {
        Play(clickClip);
    }


    // Keyboard / Gamepad
    public void OnSelect(BaseEventData e) => Play(hoverClip);
    public void OnSubmit(BaseEventData e) => Play(clickClip);
}
