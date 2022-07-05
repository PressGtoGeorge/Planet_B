using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRatio : MonoBehaviour
{
    public float orthographicStartSize;

    private void Update()
    {
        float aspectRatioDesign = (16f / 9f);

        float inverseAspectRatio = 1 / aspectRatioDesign;
        float currentAspectRatio = (float)Screen.width / (float)Screen.height;

        if (currentAspectRatio > aspectRatioDesign)
        {
            currentAspectRatio -= (currentAspectRatio - aspectRatioDesign);
        }
        else if (currentAspectRatio < inverseAspectRatio)
        {
            currentAspectRatio += (currentAspectRatio - inverseAspectRatio);
        }

        gameObject.GetComponent<Camera>().orthographicSize = aspectRatioDesign * (orthographicStartSize / currentAspectRatio);
    }
}