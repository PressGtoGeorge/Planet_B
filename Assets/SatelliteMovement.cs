using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatelliteMovement : MonoBehaviour
{
    private float currentPos;
    private float maxPos = 0.2f;
    private float loopTime = 5f;

    private Vector3 startPos;

    IEnumerator Start()
    {
        startPos = transform.position;

        while (currentPos < maxPos)
        {
            currentPos += (1f / loopTime) * Time.unscaledDeltaTime * maxPos;
            transform.position += Vector3.up * (1f / loopTime) * Time.unscaledDeltaTime * maxPos;
            yield return null;
        }

        transform.position = startPos + Vector3.up * maxPos;

        while (currentPos > 0)
        {
            currentPos -= (1f / loopTime) * Time.unscaledDeltaTime * maxPos;
            transform.position -= Vector3.up * (1f / loopTime) * Time.unscaledDeltaTime * maxPos;
            yield return null;
        }

        transform.position = startPos;

        StartCoroutine(Start());
    }
}