using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitToMenu : MonoBehaviour
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
        SceneManager.LoadScene("Menu");
    }
}
