using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtenBackgroundAnimation : MonoBehaviour
{
    private float speed = 0.1f;
    public float distance;

    private Vector3 originalPos;

    // Start is called before the first frame update
    void Start()
    {
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += transform.right * speed * Time.unscaledDeltaTime;
        distance += speed * Time.unscaledDeltaTime;

        if (distance >= 2.4f)
        {
            transform.position = originalPos;
            distance = 0;
            //distance -= 1.2f;
        }
    }
}
