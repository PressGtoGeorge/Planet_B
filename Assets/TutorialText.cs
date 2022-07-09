using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialText : MonoBehaviour
{
    private string completeText;

    private bool clickAway;
    private bool completed;
    private WaitForSecondsRealtime delay = new WaitForSecondsRealtime(0.015f);

    private Coroutine completeTextCo;
    private bool pauseMenuOpenLastFrame;

    private void OnEnable()
    {
        completeTextCo = StartCoroutine(CompleteText());
        StartCoroutine(ResetClickAway());
    }

    IEnumerator CompleteText()
    {
        completeText = gameObject.GetComponent<TextMeshProUGUI>().text;
        string shownText = "";

        int completeLength = completeText.Length;
        int shownLength = 0;

        while (shownLength < completeLength)
        {
            shownLength++;
            shownText += completeText[shownLength - 1];

            gameObject.GetComponent<TextMeshProUGUI>().text = shownText;
            yield return delay;
        }

        completed = true;

        yield break;
    }

    IEnumerator ResetClickAway()
    {
        yield return null;
        clickAway = true;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameState.pauseMenuOpen == false && pauseMenuOpenLastFrame == false)
        {
            if (clickAway && completed)
            {
                clickAway = false;
                completed = false;
                transform.parent.gameObject.SetActive(false);
            }
            else
            {
                StopCoroutine(completeTextCo);
                completed = true;
                gameObject.GetComponent<TextMeshProUGUI>().text = completeText;
            }
        }

        pauseMenuOpenLastFrame = GameState.pauseMenuOpen;
    }

    public void DisableText()
    {
        clickAway = false;
        completed = false;
        transform.parent.gameObject.SetActive(false);
    }
}