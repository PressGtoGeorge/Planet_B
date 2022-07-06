using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioVolume : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider effectsSlider;
    public Slider musicSlider;

    private void Start()
    {
        SetMixerVolume();
        transform.parent.gameObject.SetActive(false);
    }

    public void SetMixerVolume()
    {
        mixer.SetFloat("EffectsVolume", Mathf.Log10(Settings.effectsVolume) * 20f);
        mixer.SetFloat("MusicVolume", Mathf.Log10(Settings.musicVolume) * 20f);
    }

    private void OnEnable()
    {
        effectsSlider.value = Settings.effectsVolume;
        musicSlider.value = Settings.musicVolume;
    }

    public void SetEffectsVolume(float value)
    {
        mixer.SetFloat("EffectsVolume", Mathf.Log10(value) * 20f);
        Settings.SetEffectsVolume(value);
    }

    public void SetMusicVolume(float value)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20f);
        Settings.SetMusicVolume(value);
    }
}