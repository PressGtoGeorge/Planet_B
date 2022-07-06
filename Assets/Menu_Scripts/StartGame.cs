using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
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
        SceneManager.LoadScene("Game");
    }
}