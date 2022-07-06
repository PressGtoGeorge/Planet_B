using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOption : MonoBehaviour
{
    public Sprite disabledSprite;
    public Sprite enabledSprite;

    public GameObject checkMark;
    public Sprite notRotatingCheckMark;
    public Sprite rotatingCheckMark;

    private void OnEnable()
    {
        if (Settings.stopRotation)
        {
            checkMark.GetComponent<SpriteRenderer>().sprite = notRotatingCheckMark;
        }
        else
        {
            checkMark.GetComponent<SpriteRenderer>().sprite = rotatingCheckMark;
        }
    }

    private void OnMouseEnter()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = enabledSprite;
    }

    private void OnMouseExit()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = disabledSprite;
    }

    private void OnMouseDown()
    {
        Settings.stopRotation = !Settings.stopRotation;
        if (Settings.stopRotation)
        {
            checkMark.GetComponent<SpriteRenderer>().sprite = notRotatingCheckMark;
        }
        else
        {
            checkMark.GetComponent<SpriteRenderer>().sprite = rotatingCheckMark;
        }
    }
}
