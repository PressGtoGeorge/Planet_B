using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPlanet : MonoBehaviour
{
    private bool onPlanet_A = false;

    private GameObject planet_A;
    private GameObject planet_B;

    private bool moving;
    private float speed = 20f;

    private AudioSource switchSource;

    private void Start()
    {
        planet_A = GameObject.FindGameObjectWithTag("Planet_A");
        planet_B = GameObject.FindGameObjectWithTag("Planet_B");

        switchSource = gameObject.GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        if (moving == false) StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        moving = true;

        switchSource.Play();

        Vector3 goalPos;
        Vector3 startPos;

        if (onPlanet_A) 
        {
            startPos = planet_A.transform.position;
            goalPos = planet_B.transform.position;
        }
        else 
        {
            startPos = planet_B.transform.position;
            goalPos = planet_A.transform.position;
        }

        float currentDistance = 0f;
        float goalDistance = (planet_A.transform.position - planet_B.transform.position).magnitude;

        while (currentDistance < goalDistance)
        {
            transform.parent.position += (goalPos - startPos).normalized * speed * Time.unscaledDeltaTime;
            currentDistance += speed * Time.unscaledDeltaTime;

            yield return null;
        }

        // fix position
        transform.parent.position += (goalPos - startPos).normalized * (goalDistance - currentDistance); // setting position directly screws up clear flags ???
        onPlanet_A = !onPlanet_A;

        yield return new WaitForEndOfFrame(); // this seems to fix a rare bug that stops the button from working?

        moving = false;
    }
}