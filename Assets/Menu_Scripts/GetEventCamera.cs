using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetEventCamera : MonoBehaviour
{
    private Canvas canvas;

    private void Start()
    {
        canvas = gameObject.GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
    }
}
