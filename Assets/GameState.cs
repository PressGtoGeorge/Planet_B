using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    public Slider gameSpeedSlider;

    public static int totalCharacters = 0;
    public static bool dragging; // true if dragging an object onto planet_b

    public List<GameObject> spawnButtons = new List<GameObject>();

    private void Start()
    {
        Time.timeScale = 1;
    }

    public void ChangeGameSpeed()
    {
        float newGameSpeed = gameSpeedSlider.value;
        Time.timeScale = newGameSpeed;
    }

    public void FadeInAllButtons()
    {
        foreach(GameObject spawnButton in spawnButtons)
        {
            spawnButton.GetComponent<ButtonAnimation>().StartFadeIn();
        }
    }
}