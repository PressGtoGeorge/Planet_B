using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameState : MonoBehaviour
{
    public Slider gameSpeedSlider;
    // private float gameSpeed = 1f;

    public void ChangeGameSpeed()
    {
        float newGameSpeed = gameSpeedSlider.value;
        Time.timeScale = newGameSpeed;
    }
}
