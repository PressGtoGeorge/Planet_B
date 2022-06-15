using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetForeground : MonoBehaviour
{
    private bool overPlanet;
    private bool overPlanetLastFrame;

    private SpriteRenderer renderer2d;
    private SpriteRenderer cloudRenderer2d;

    private Collider2D collider2d;

    private void Start()
    {
        renderer2d = gameObject.GetComponent<SpriteRenderer>();
        cloudRenderer2d = transform.GetChild(0).GetComponent<SpriteRenderer>();

        collider2d = gameObject.GetComponent<Collider2D>();
    }

    private void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        overPlanet = collider2d.OverlapPoint(mousePosition);

        if (overPlanet && overPlanetLastFrame == false)
        {
            StopAllCoroutines();
            StartCoroutine(FadeOut());
        }
        else if (overPlanet == false && overPlanetLastFrame)
        {
            StopAllCoroutines();
            StartCoroutine(FadeIn());
        }

        overPlanetLastFrame = overPlanet;
    }

    private IEnumerator FadeOut()
    {
        float trans = renderer2d.GetComponent<SpriteRenderer>().color.a;

        while (trans > 0)
        {
            trans -= 0.75f * Time.unscaledDeltaTime;
            trans = Mathf.Clamp(trans, 0, 1);
            Color col = renderer2d.GetComponent<SpriteRenderer>().color;
            col.a = trans;
            renderer2d.GetComponent<SpriteRenderer>().color = col;
            cloudRenderer2d.GetComponent<SpriteRenderer>().color = col;

            yield return null;
        }

        yield break;
    }

    private IEnumerator FadeIn()
    {
        float trans = renderer2d.GetComponent<SpriteRenderer>().color.a;

        while (trans < 1)
        {
            trans += 0.75f * Time.unscaledDeltaTime;
            trans = Mathf.Clamp(trans, 0, 1);
            Color col = renderer2d.GetComponent<SpriteRenderer>().color;
            col.a = trans;
            renderer2d.GetComponent<SpriteRenderer>().color = col;
            cloudRenderer2d.GetComponent<SpriteRenderer>().color = col;

            yield return null;
        }

        yield break;
    }

}