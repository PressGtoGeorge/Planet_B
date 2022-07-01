using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    public Slider gameSpeedSlider;

    public static int totalCharacters;
    public static bool dragging; // true if dragging an object onto planet_b

    public List<GameObject> spawnButtons = new List<GameObject>();

    public static bool gameOver;

    private void Start()
    {
        Time.timeScale = 2;
        totalCharacters = 0;

        gameOver = false;
    }

    public void ChangeGameSpeed()
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