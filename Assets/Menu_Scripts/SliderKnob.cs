using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderKnob : MonoBehaviour
{
    public Sprite disabledSprite;
    public Sprite enabledSprite;

    private Image image;

    private void Start()
    {
        image = gameObject.GetComponent<Image>();
    }

    private void OnMouseEnter()
    {
        image.sprite = enabledSprite;
    }

    private void OnMouseExit()
    {
        image.sprite = disabledSprite;
    }

}