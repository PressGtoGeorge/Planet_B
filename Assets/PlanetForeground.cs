using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetForeground : MonoBehaviour
{
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
        bool overPlanet = collider2d.OverlapPoint(mousePosition);

        if (overPlanet)
        {
            renderer2d.enabled = false;
            cloudRenderer2d.enabled = false;
        }
        else
        {
            renderer2d.enabled = true;
            cloudRenderer2d.enabled = true;
        }
    }
}