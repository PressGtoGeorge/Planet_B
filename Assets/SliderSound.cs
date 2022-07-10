using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderSound : MonoBehaviour
{
    private void OnMouseDown()
    {
        Settings.PlayClick();
    }

    private void OnMouseUp()
    {
        Settings.PlayHover();
    }
}
