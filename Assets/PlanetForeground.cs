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

    public AudioSource wheelSource;
    private float maxVolume = 0.2f;

    public Camera planet_A_Camera;
    private bool switchedLastFrame;
    private bool switchingLastFrame;

    private void Start()
    {
        renderer2d = gameObject.GetComponent<SpriteRenderer>();
        cloudRenderer2d = transform.GetChild(0).GetComponent<SpriteRenderer>();

        collider2d = gameObject.GetComponent<Collider2D>();
    }

    private void Update()
    {
        Vector2 mousePosition = planet_A_Camera.ScreenToWorldPoint(Input.mousePosition);
        overPlanet = collider2d.OverlapPoint(mousePosition);

        if (((overPlanet && overPlanetLastFrame == false && GameState.switched && GameState.switching == false) || (overPlanet && switchingLastFrame && GameState.switched && GameState.switching == false))
            && GameState.pauseMenuOpen == false)
        {
            StopAllCoroutines();
            StartCoroutine(FadeOut());
        }
        else if ((overPlanet == false && overPlanetLastFrame && GameState.switched) || (GameState.switched && GameState.switching && switchingLastFrame == false))
        {
            StopAllCoroutines();
            StartCoroutine(FadeIn());
        }

        overPlanetLastFrame = overPlanet;

        switchedLastFrame = GameState.switched;
        switchingLastFrame = GameState.switching;
    }

    private IEnumerator FadeOut()
    {
        float trans = renderer2d.GetComponent<SpriteRenderer>().color.a;

        while (trans > 0)
        {
            trans -= 1f * (Time.deltaTime / Time.timeScale);
            trans = Mathf.Clamp(trans, 0, 1);
            Color col = renderer2d.GetComponent<SpriteRenderer>().color;
            col.a = trans;
            renderer2d.GetComponent<SpriteRenderer>().color = col;
            cloudRenderer2d.GetComponent<SpriteRenderer>().color = col;

            wheelSource.volume += maxVolume * (Time.deltaTime / Time.timeScale);
            wheelSource.volume = Mathf.Clamp(wheelSource.volume, 0, maxVolume);

            yield return null;
        }

        wheelSource.volume = maxVolume;

        yield break;
    }

    private IEnumerator FadeIn()
    {
        float trans = renderer2d.GetComponent<SpriteRenderer>().color.a;

        while (trans < 1)
        {
            trans += 1f * (Time.deltaTime / Time.timeScale);
            trans = Mathf.Clamp(trans, 0, 1);
            Color col = renderer2d.GetComponent<SpriteRenderer>().color;
            col.a = trans;
            renderer2d.GetComponent<SpriteRenderer>().color = col;
            cloudRenderer2d.GetComponent<SpriteRenderer>().color = col;

            wheelSource.volume -= maxVolume * (Time.deltaTime / Time.timeScale);
            wheelSource.volume = Mathf.Clamp(wheelSource.volume, 0, maxVolume);

            yield return null;
        }

        wheelSource.volume = 0;

        yield break;
    }

}