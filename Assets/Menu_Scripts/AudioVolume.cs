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
        effectsSlider.value = Settings.effectsVolume;
        musicSlider.value = Settings.musicVolume;

        mixer.SetFloat("EffectsVolume", Mathf.Log10(Settings.effectsVolume) * 20f);
        mixer.SetFloat("MusicVolume", Mathf.Log10(Settings.musicVolume) * 20f);
    }

    private void OnEnable()
    {
        effectsSlider.value = Settings.effectsVolume;
        musicSlider.value = Settings.musicVolume;
    }

    public void SetEffectsVolume(float value) // called by event system on slider
    {
        mixer.SetFloat("EffectsVolume", Mathf.Log10(value) * 20f);
        Settings.SetEffectsVolume(value);
    }

    public void SetMusicVolume(float value) // called by event system on slider
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20f);
        Settings.SetMusicVolume(value);
    }
}