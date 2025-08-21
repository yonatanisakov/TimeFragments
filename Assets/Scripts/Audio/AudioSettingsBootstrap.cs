using UnityEngine;
using UnityEngine.Audio;
using System.Collections;
using Unity.VisualScripting;

[DefaultExecutionOrder(-1000)]
public class AudioSettingsBootstrap : MonoBehaviour
{
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private string masterParam = "MasterVolume";
    [SerializeField] private string musicParam = "MusicVolume";
    [SerializeField] private string sfxParam = "SFXVolume";
    [SerializeField] private string uiParam = "UIVolume";

    const string PP_MASTER = "vol_master";
    const string PP_MUSIC = "vol_music";
    const string PP_SFX = "vol_sfx";
    const string PP_UI = "vol_ui";

    static float LinearToDb(float v) => (v < 0.0001f) ? -80f : Mathf.Log10(v) * 20f;

    void Start()
    {

        Apply(PlayerPrefs.GetFloat(PP_MASTER, 0.8f), masterParam);
        Apply(PlayerPrefs.GetFloat(PP_MUSIC, 0.8f), musicParam);
        Apply(PlayerPrefs.GetFloat(PP_SFX, 0.8f), sfxParam);
        Apply(PlayerPrefs.GetFloat(PP_UI, 0.8f), uiParam);
    }

    void Apply(float val, string param)
    {
        if (masterMixer && !string.IsNullOrEmpty(param))
        {
            float db = LinearToDb(val);
            bool ok = masterMixer.SetFloat(param, db);
        }
    }
}
