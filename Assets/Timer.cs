using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private Text text;
    private WaitForSeconds second = new WaitForSeconds(2); // because game runs at twice the speed by default

    private int time;

    void Start()
    {
        text = transform.GetChild(0).GetComponent<Text>();
        StartCoroutine(Count());
    }

    private IEnumerator Count()
    {
        while (GameState.gameOver == false)
        {
            int minutes = time / 60;
            int seconds = time % 60;
            
            if (seconds < 10)
            {
                text.text = minutes.ToString() + ":0" + seconds.ToString();
            }
            else
            {
                text.text = minutes.ToString() + ":" + seconds.ToString();
            }

            time++;
            yield return second;
        }
    }
}