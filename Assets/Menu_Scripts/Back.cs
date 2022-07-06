using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back : MonoBehaviour
{
    public bool inGameOptions;

    public Sprite disabledSprite;
    public Sprite enabledSprite;

    private GameObject screen;
    public GameObject pauseMenu;

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
        gameObject.GetComponent<SpriteRenderer>().sprite = disabledSprite;

        if (inGameOptions) pauseMenu.SetActive(true);
        screen.SetActive(false);
    }
}
