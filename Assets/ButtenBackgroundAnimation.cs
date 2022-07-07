using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtenBackgroundAnimation : MonoBehaviour
{
    private float speed = 0.1f;
    public float distance;

    private Vector3 originalPos;

    public Sprite enabledSprite;
    public Sprite disabledSprite;

    private List<SpriteRenderer> allChildrenRenderers = new List<SpriteRenderer>();

    private bool switchedLastFrame;
    private bool switchingLastFrame;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;

        foreach (Transform child in transform)
        {
            allChildrenRenderers.Add(child.GetComponent<SpriteRenderer>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += transform.right * speed * (Time.deltaTime / Time.timeScale);
        distance += speed * (Time.deltaTime / Time.timeScale);

        if (distance >= 2.4f)
        {
            transform.position = originalPos;
            distance = 0;
        }

        /*
        if (GameState.switched == false && switchingLastFrame == false && GameState.switching)
        {
            foreach (SpriteRenderer renderer in allChildrenRenderers)
            {
                renderer.sprite = disabledSprite;
            }
        }
        else if (GameState.switched == false && switchingLastFrame && GameState.switching == false)
        {
            foreach (SpriteRenderer renderer in allChildrenRenderers)
            {
                renderer.sprite = enabledSprite;
            }
        }

        switchedLastFrame = GameState.switched;
        switchingLastFrame = GameState.switching;
    */

    }
}