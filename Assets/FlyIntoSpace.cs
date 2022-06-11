using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FlyIntoSpace : MonoBehaviour
{
    private float speed = 10f;

    public void Fly()
    {
        gameObject.SetActive(true);
        StartCoroutine(Flying());
    }

    private IEnumerator Flying()
    {
        transform.parent = null;

        float currentDistance = 0f;
        float goalDistance = 9001f;

        while (currentDistance < goalDistance)
        {
            currentDistance += speed * Time.deltaTime;
            transform.position += transform.up * speed * Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}