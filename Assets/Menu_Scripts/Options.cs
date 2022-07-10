using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour
{
    public bool inGameOptions;
    public GameObject pauseMenu;

    public Sprite disabledSprite;
    public Sprite enabledSprite;

    public GameObject optionScreen;

    private void OnMouseEnter()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = enabledSprite;
        Settings.PlayHover();
    }

    private void OnMouseExit()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = disabledSprite;
    }

    private void OnMouseDown()
    {
        Settings.PlayClick();

        if (inGameOptions) pauseMenu.SetActive(false);
        gameObject.GetComponent<SpriteRenderer>().sprite = disabledSprite;
        optionScreen.SetActive(true);
    }
}