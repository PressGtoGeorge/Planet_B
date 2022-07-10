using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public static GameObject instance;

    public static bool stopRotation = false;
    public static bool stopTutorial = false;

    public static float musicVolume = 1;
    public static float effectsVolume = 1;

    public AudioSource setHoverSource;
    public AudioSource setClickSource;

    public static AudioSource hoverSource;
    public static AudioSource clickSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = gameObject;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        LoadSettings();
    }

    private void Start()
    {
        hoverSource = setHoverSource;
        clickSource = setClickSource;
    }

    public static void SetEffectsVolume(float value)
    {
        effectsVolume = value;
    }

    public static void SetMusicVolume(float value)
    {
        musicVolume = value;
    }

    public static void SaveSettings()
    {
        PlayerPrefs.SetFloat("effectsVolume", effectsVolume);
        PlayerPrefs.SetFloat("musicVolume", musicVolume);

        PlayerPrefs.SetString("stopRotation", stopRotation.ToString());
        PlayerPrefs.SetString("stopTutorial", stopTutorial.ToString());
    }

    public static void LoadSettings()
    {
        effectsVolume = PlayerPrefs.GetFloat("effectsVolume", 1);
        musicVolume = PlayerPrefs.GetFloat("musicVolume", 1);

        if (PlayerPrefs.GetString("stopRotation", "False") == "True") stopRotation = true;
        else stopRotation = false;

        if (PlayerPrefs.GetString("stopTutorial", "False") == "True") stopTutorial = true;
        else stopTutorial = false;
    }

    public static void PlayHover()
    {
        hoverSource.Play();
    }

    public static void PlayClick()
    {
        clickSource.Play();
    }

}