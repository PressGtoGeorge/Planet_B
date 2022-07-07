using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public Sprite disabledSprite;
    public Sprite enabledSprite;

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
        Debug.Log("Pressed exit.");
        Application.Quit();
    }
}