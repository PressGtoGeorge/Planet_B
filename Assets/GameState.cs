using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameState : MonoBehaviour
{
    public Slider setGameSpeedSlider;
    public static Slider gameSpeedSlider;

    public static int totalCharacters;
    public static bool dragging; // true if dragging an object onto planet_b

    public List<GameObject> spawnButtons = new List<GameObject>();

    public static bool gameOver;
    public static bool switched;
    public static bool switching;

    public static bool pauseMenuOpen;

    public static AudioClip flyAudio;
    public AudioClip setFlyAudio;

    public static AudioMixerGroup effectsGroup;
    public AudioMixerGroup setEffectsGroup;

    private void Start()
    {
        Time.timeScale = 2;
        totalCharacters = 0;

        gameSpeedSlider = setGameSpeedSlider;

        gameOver = false;
        switched = false;
        switching = false;
        pauseMenuOpen = false;

        flyAudio = setFlyAudio;
        effectsGroup = setEffectsGroup;
    }

    public void ChangeGameSpeed() // called by event system when slider changes value
    {
        float newGameSpeed = gameSpeedSlider.value;
        Time.timeScale = newGameSpeed;
    }

    public void FadeInAllButtons()
    {
        foreach (GameObject spawnButton in spawnButtons)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            bool overButton = spawnButton.GetComponent<ButtonAnimation>().collider2d.OverlapPoint(mousePosition);

            if (overButton == false)
            {
                spawnButton.GetComponent<ButtonAnimation>().StartFadeIn();
            }
            else
            {
                spawnButton.GetComponent<ButtonAnimation>().StartFadeOut();
            }
        }
    }
}