using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAnimation : MonoBehaviour
{
    public Collider2D collider2d;
    private SpriteRenderer backgroundRenderer;
    private SpriteRenderer textRenderer;

    private bool overButton;
    private bool overButtonLastFrame;

    private GameState gameState;

    void Start()
    {
        collider2d = gameObject.GetComponent<Collider2D>();
        backgroundRenderer = transform.GetChild(0).GetChild(3).GetComponent<SpriteRenderer>();
        textRenderer = transform.GetChild(0).GetChild(5).GetComponent<SpriteRenderer>();

        gameState = GameObject.FindGameObjectWithTag("GameState").GetComponent<GameState>();
        gameState.spawnButtons.Add(gameObject);
    }

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        overButton = collider2d.OverlapPoint(mousePosition);

        if (overButton && overButtonLastFrame == false && GameState.dragging == false)
        {
            StartFadeOut();
        }
        else if (overButton == false && overButtonLastFrame && GameState.dragging == false)
        {
            StartFadeIn();
        }

        overButtonLastFrame = overButton;
    }

    public void StartFadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }

    public void StartFadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float trans = backgroundRenderer.GetComponent<SpriteRenderer>().color.a;

        while (trans > 0)
        {
            trans -= 2f * Time.unscaledDeltaTime;
            trans = Mathf.Clamp(trans, 0, 1);
            Color col = backgroundRenderer.GetComponent<SpriteRenderer>().color;
            col.a = trans;
            backgroundRenderer.GetComponent<SpriteRenderer>().color = col;

            col.a = 1 - trans;
            textRenderer.GetComponent<SpriteRenderer>().color = col;

            yield return null;
        }

        yield break;
    }

    private IEnumerator FadeIn()
    {
        float trans = backgroundRenderer.GetComponent<SpriteRenderer>().color.a;

        while (trans < 1)
        {
            trans += 2f * Time.unscaledDeltaTime;
            trans = Mathf.Clamp(trans, 0, 1);
            Color col = backgroundRenderer.GetComponent<SpriteRenderer>().color;
            col.a = trans;
            backgroundRenderer.GetComponent<SpriteRenderer>().color = col;

            col.a = 1 - trans;
            textRenderer.GetComponent<SpriteRenderer>().color = col;

            yield return null;
        }

        yield break;
    }
}