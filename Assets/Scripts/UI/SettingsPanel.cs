using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using System;

public class SettingsPanel : MonoBehaviour
{
    [Header("Mixer & Param Names")]
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private string musicParam = "MusicVolume";
    [SerializeField] private string sfxParam = "SFXVolume";
    [SerializeField] private string uiParam = "UIVolume";
    [SerializeField] private string masterParam = "MasterVolume"; 

    [Header("Sliders")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider uiSlider;

    [Header("Value Texts (optional)")]
    [SerializeField] private TextMeshProUGUI masterValueText;
    [SerializeField] private TextMeshProUGUI musicValueText;
    [SerializeField] private TextMeshProUGUI sfxValueText;
    [SerializeField] private TextMeshProUGUI uiValueText;

    [Header("Buttons")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button resetButton; 

    public event Action OnBackRequested;

    const string PP_MASTER = "vol_master";
    const string PP_MUSIC = "vol_music";
    const string PP_SFX = "vol_sfx";
    const string PP_UI = "vol_ui";

    void Awake()
    {
        SetupSlider(masterSlider, OnMasterChanged);
        SetupSlider(musicSlider, OnMusicChanged);
        SetupSlider(sfxSlider, OnSFXChanged);
        SetupSlider(uiSlider, OnUIChanged);

        if (backButton) backButton.onClick.AddListener(() => OnBackRequested?.Invoke());
        if (resetButton) resetButton.onClick.AddListener(ResetToDefaults);
    }

    void OnEnable()
    {
        float m = PlayerPrefs.GetFloat(PP_MASTER, 0.8f);
        float mu = PlayerPrefs.GetFloat(PP_MUSIC, 0.8f);
        float sx = PlayerPrefs.GetFloat(PP_SFX, 0.8f);
        float ui = PlayerPrefs.GetFloat(PP_UI, 0.8f);

        if (masterSlider) masterSlider.SetValueWithoutNotify(m);
        if (musicSlider) musicSlider.SetValueWithoutNotify(mu);
        if (sfxSlider) sfxSlider.SetValueWithoutNotify(sx);
        if (uiSlider) uiSlider.SetValueWithoutNotify(ui);

        OnMasterChanged(m);
        OnMusicChanged(mu);
        OnSFXChanged(sx);
        OnUIChanged(ui);
    }

    void OnDisable() => Save();

    void Save()
    {
        if (masterSlider) PlayerPrefs.SetFloat(PP_MASTER, masterSlider.value);
        if (musicSlider) PlayerPrefs.SetFloat(PP_MUSIC, musicSlider.value);
        if (sfxSlider) PlayerPrefs.SetFloat(PP_SFX, sfxSlider.value);
        if (uiSlider) PlayerPrefs.SetFloat(PP_UI, uiSlider.value);
        PlayerPrefs.Save();
    }

    void ResetToDefaults() => SetAll(0.8f);

    void SetAll(float v)
    {
        if (masterSlider) masterSlider.value = v;
        if (musicSlider) musicSlider.value = v;
        if (sfxSlider) sfxSlider.value = v;
        if (uiSlider) uiSlider.value = v;
    }

    static float LinearToDb(float v) => (v < 0.0001f) ? -80f : Mathf.Log10(v) * 20f;

    void UpdateText(TextMeshProUGUI t, float v)
    {
        if (t) t.text = Mathf.RoundToInt(v * 100f) + "%";
    }

    void SetupSlider(Slider s, Action<float> cb)
    {
        if (!s) return;
        s.minValue = 0f; s.maxValue = 1f; s.wholeNumbers = false;
        s.onValueChanged.AddListener(v => { cb(v); Save(); });
    }

    void OnMasterChanged(float v)
    {
        if (masterMixer && !string.IsNullOrEmpty(masterParam))
            masterMixer.SetFloat(masterParam, LinearToDb(v));
        UpdateText(masterValueText, v);
    }
    void OnMusicChanged(float v)
    {
        if (masterMixer) masterMixer.SetFloat(musicParam, LinearToDb(v));
        UpdateText(musicValueText, v);
    }
    void OnSFXChanged(float v)
    {
        if (masterMixer) masterMixer.SetFloat(sfxParam, LinearToDb(v));
        UpdateText(sfxValueText, v);
    }
    void OnUIChanged(float v)
    {
        if (masterMixer) masterMixer.SetFloat(uiParam, LinearToDb(v));
        UpdateText(uiValueText, v);
    }
}
