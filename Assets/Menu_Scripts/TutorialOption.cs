using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialOption : MonoBehaviour
{
    public GameObject checkMark;
    public Sprite noTutorialCheckMark;
    public Sprite tutorialCheckMark;

    private void OnEnable()
    {
        if (Settings.stopTutorial)
        {
            checkMark.GetComponent<SpriteRenderer>().sprite = noTutorialCheckMark;
        }
        else
        {
            checkMark.GetComponent<SpriteRenderer>().sprite = tutorialCheckMark;
        }
    }

    private void OnMouseDown()
    {
        Settings.stopTutorial = !Settings.stopTutorial;
        if (Settings.stopTutorial)
        {
            checkMark.GetComponent<SpriteRenderer>().sprite = noTutorialCheckMark;
        }
        else
        {
            checkMark.GetComponent<SpriteRenderer>().sprite = tutorialCheckMark;
        }
    }
}