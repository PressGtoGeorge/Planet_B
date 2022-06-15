using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    // general purpose rotation script for animation (clouds, wheels etc.)
    public float speed;
    public bool unscaledTime;

    void Update()
    {
        if (unscaledTime) transform.Rotate(Vector3.forward, speed * Time.unscaledDeltaTime);
        else transform.Rotate(Vector3.forward, speed * Time.deltaTime);
    }
}