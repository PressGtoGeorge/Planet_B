using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FlyIntoSpace : MonoBehaviour
{
    private float speed = 8f;

    public void Fly()
    {
        gameObject.SetActive(true);
        StartCoroutine(Flying());
    }

    private IEnumerator Flying()
    {
        transform.parent = null;

        if (transform.GetChild(0).GetComponent<Character>() != null)
            transform.GetChild(0).GetComponent<Character>().SetMoving(false);

        if (gameObject.GetComponent<RocketAnimation>() != null)
            gameObject.GetComponent<RocketAnimation>().StopAnimation();

        float currentDistance = 0f;
        float goalDistance = 9001f;

        while (currentDistance < goalDistance)
        {
            currentDistance += speed * Time.unscaledDeltaTime;
            transform.position += transform.up * speed * Time.unscaledDeltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}