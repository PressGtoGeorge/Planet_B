using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialText : MonoBehaviour
{
    private string completeText;

    private bool clickAway;
    private bool completed;
    private WaitForSecondsRealtime delay = new WaitForSecondsRealtime(0.015f);

    private Coroutine completeTextCo;

    private void OnEnable()
    {
        completeTextCo = StartCoroutine(CompleteText());
        StartCoroutine(ResetClickAway());
    }

    IEnumerator CompleteText()
    {
        completeText = gameObject.GetComponent<Text>().text;
        string shownText = "";

        int completeLength = completeText.Length;
        int shownLength = 0;

        while (shownLength < completeLength)
        {
            shownLength++;
            shownText += completeText[shownLength - 1];

            gameObject.GetComponent<Text>().text = shownText;
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
        if (Input.GetMouseButtonDown(0))
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
                gameObject.GetComponent<Text>().text = completeText;
            }
        }
    }
}