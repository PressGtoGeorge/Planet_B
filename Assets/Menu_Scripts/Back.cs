using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back : MonoBehaviour
{
    public Sprite disabledSprite;
    public Sprite enabledSprite;

    private GameObject screen;

    private void Start()
    {
        screen = transform.parent.gameObject;
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
        Settings.SaveSettings();
        screen.SetActive(false);
    }
}
