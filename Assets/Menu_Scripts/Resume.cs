using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resume : MonoBehaviour
{
    public Sprite disabledSprite;
    public Sprite enabledSprite;

    public GameObject pauseMenu;

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
        pauseMenu.SetActive(false);
        GameState.pauseMenuOpen = false;
        GameState.gameSpeedSlider.interactable = true;

        gameObject.GetComponent<SpriteRenderer>().sprite = disabledSprite;
    }
}
