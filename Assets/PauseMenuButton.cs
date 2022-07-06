using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuButton : MonoBehaviour
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
        pauseMenu.SetActive(true);
        GameState.pauseMenuOpen = true;
    }
}