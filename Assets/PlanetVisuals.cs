using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetVisuals : MonoBehaviour
{
    public List<SpriteRenderer> backgrounds = new List<SpriteRenderer>();
    public List<SpriteRenderer> skys = new List<SpriteRenderer>();

    public List<SpriteRenderer> foregrounds = new List<SpriteRenderer>();
    public List<SpriteRenderer> clouds = new List<SpriteRenderer>();

    public Collider2D collider2d;

    private Ecosystem ecosystem;

    private float switchPoint_0 = 500;
    private float switchPoint_1 = 1000;

    private float lastGas;

    private float progress = 0;
    private float rest = 1;

    // for fading foreground in/out
    private bool overPlanet;
    private bool overPlanetLastFrame;

    private bool passedSwitchPoint;

    private bool fading;

    public float test;

    private void Start()
    {
        ecosystem = gameObject.GetComponent<Ecosystem>();
    }

    void Update()
    {
        UpdatePlanetVisuals();
        UpdateForegroundTransparency();
    }

    private void UpdateForegroundTransparency()
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

        fading = true;

        SpriteRenderer foreGround0;
        SpriteRenderer cloud0;

        SpriteRenderer foreGround1;
        SpriteRenderer cloud1;

        if (passedSwitchPoint == false)
        {
            foreGround0 = foregrounds[0];
            cloud0 = clouds[0];

            foreGround1 = foregrounds[1];
            cloud1 = clouds[1];
        }
        else
        {
            foreGround0 = foregrounds[1];
            cloud0 = clouds[1];

            foreGround1 = foregrounds[2];
            cloud1 = clouds[2];
        }

        float cloud0trans = cloud0.color.a;
        float cloud1trans = cloud1.color.a;

        float foreGround0trans = foreGround0.color.a;

        float startCloud0trans = cloud0trans;
        float startCloud1trans = cloud1trans;
        float startForeGround0trans = foreGround0trans;

        while (foreGround0trans > 0)
        {
            cloud0trans -= 1f * startCloud0trans * Time.unscaledDeltaTime;
            cloud0trans = Mathf.Clamp(cloud0trans, 0, 1);

            cloud1trans -= 1f * startCloud1trans * Time.unscaledDeltaTime;
            cloud1trans = Mathf.Clamp(cloud1trans, 0, 1);

            foreGround0trans -= 1f * startForeGround0trans * Time.unscaledDeltaTime;
            foreGround0trans = Mathf.Clamp(foreGround0trans, 0, 1);

            Color col = Color.white;
            col.a = foreGround0trans;
            foreGround0.color = col;

            col.a = cloud0trans;
            cloud0.color = col;

            col.a = cloud1trans;

            foreGround1.color = col;
            cloud1.color = col;

            yield return null;
        }

        fading = false;

        while (true)
        {
            Color col = Color.white;
            col.a = 0;

            foreGround0.color = col;
            cloud0.color = col;

            foreGround1.color = col;
            cloud1.color = col;
            yield return null;
        }

        // yield break;
    }

    private IEnumerator FadeIn()
    {
        fading = true;

        SpriteRenderer foreground0;
        SpriteRenderer cloud0;

        SpriteRenderer foreGround1;
        SpriteRenderer cloud1;

        if (passedSwitchPoint == false)
        {
            foreground0 = foregrounds[0];
            cloud0 = clouds[0];

            foreGround1 = foregrounds[1];
            cloud1 = clouds[1];
        }
        else
        {
            foreground0 = foregrounds[1];
            cloud0 = clouds[1];

            foreGround1 = foregrounds[2];
            cloud1 = clouds[2];
        }

        float cloud0trans = cloud0.color.a;
        float cloud1trans = cloud1.color.a;

        float foreground0trans = foreground0.color.a;

        // float goalCloud0trans = rest; //  - cloud0trans;
        // float goalCloud1trans = progress; // - cloud1trans;
        float goalForeground0trans = 1;

        while (foreground0trans < 1)
        {
            cloud0trans += 1f * rest * Time.unscaledDeltaTime;
            cloud0trans = Mathf.Clamp(cloud0trans, 0, 1);

            cloud1trans += 1f * progress * Time.unscaledDeltaTime;
            cloud1trans = Mathf.Clamp(cloud1trans, 0, 1);

            foreground0trans += 1f * goalForeground0trans * Time.unscaledDeltaTime;
            foreground0trans = Mathf.Clamp(foreground0trans, 0, 1);

            Color col = Color.white;
            col.a = foreground0trans;
            foreground0.color = col;

            col.a = cloud0trans;
            cloud0.color = col;

            col.a = cloud1trans;
            
            foreGround1.color = col;
            cloud1.color = col;

            yield return null;
        }

        fading = false;

        yield break;
    }

    private void UpdatePlanetVisuals()
    {
        float currentGas = ecosystem.GetCurrentGas();

        // currentGas = test;

        if (lastGas == currentGas) return;

        if (currentGas < switchPoint_0)
        {
            if (passedSwitchPoint == true) // if switching from above switchpoint to below
            {
                foregrounds[0].enabled = true;
                clouds[0].enabled = true;
                backgrounds[0].enabled = true;
                skys[0].enabled = true;

                foregrounds[2].enabled = false;
                clouds[2].enabled = false;
                backgrounds[2].enabled = false;
                skys[2].enabled = false;

                foregrounds[0].color = Color.white;
                clouds[0].color = Color.white;
                backgrounds[0].color = Color.white;
                skys[0].color = Color.white;

                foregrounds[1].color = Color.white;
                clouds[1].color = Color.white;
                backgrounds[1].color = Color.white;
                skys[1].color = Color.white;
            }

            passedSwitchPoint = false;

            progress = currentGas / switchPoint_0;
            rest = 1 - progress;

            Color col = Color.white;
            col.a = progress;

            backgrounds[1].color = col;
            skys[1].color = col;
            foregrounds[1].color = col;
            clouds[1].color = col;

            col.a = rest;

            skys[0].color = col;
            clouds[0].color = col;
        }
        else if (currentGas < switchPoint_1)
        {
            if (passedSwitchPoint == false) // if switching from below switchpoint to above
            {
                foregrounds[0].enabled = false;
                clouds[0].enabled = false;
                backgrounds[0].enabled = true;
                skys[0].enabled = true;

                foregrounds[2].enabled = true;
                clouds[2].enabled = true;
                backgrounds[2].enabled = true;
                skys[2].enabled = true;

                foregrounds[1].color = Color.white;
                clouds[1].color = Color.white;
                backgrounds[1].color = Color.white;
                skys[1].color = Color.white;

                Color transparentColor = Color.white;
                transparentColor.a = 0;

                foregrounds[2].color = transparentColor;
                clouds[2].color = transparentColor;
                backgrounds[2].color = transparentColor;
                skys[2].color = transparentColor;
            }

            passedSwitchPoint = true;

            progress = (currentGas - switchPoint_0) / (switchPoint_1 - switchPoint_0);
            rest = 1 - progress;

            Color col = Color.white;
            col.a = progress;

            backgrounds[2].color = col;
            skys[2].color = col;
            foregrounds[2].color = col;
            clouds[2].color = col;

            col.a = rest;

            skys[1].color = col;
            clouds[1].color = col;
        }

        lastGas = currentGas;
    }
}