using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlanet : MonoBehaviour
{
    private float speed = 2.5f;

    public bool collapsing;
    public bool collapsed;

    private int gridSize;

    public bool rotating = false;

    private float yearDuration = 120f;
    private float timePassedSinceLastYear;
    public int yearsPassed = 0;
    public int charactersPassed = 0;

    private void Start()
    {
        gridSize = gameObject.GetComponent<PlanetGrid>().gridSize;
    }

    void Update()
    {
        if (collapsed == false && rotating) transform.Rotate(Vector3.forward, speed * Time.deltaTime);

        timePassedSinceLastYear += Time.deltaTime;
        if (timePassedSinceLastYear >= yearDuration)
        {
            timePassedSinceLastYear -= yearDuration;
            yearsPassed++;
            // Debug.Log("Year: " + yearsPassed);
        }

        // placeholder
        if (Input.GetKeyDown(KeyCode.L) && collapsing == false)
        {
            StartCoroutine(Collapse());
        }
    }

    private IEnumerator Collapse()
    {
        collapsing = true;

        // start rotating with acceleration

        float currentSpeed = 0;
        float goalSpeed = 360f;

        float acceleration = 120f;

        while (currentSpeed < goalSpeed)
        {
            currentSpeed += acceleration * Time.deltaTime;
            transform.Rotate(Vector3.forward, currentSpeed * Time.deltaTime);
            yield return null;
        }

        // make objects on planet fly into space
        float timer = 0f;
        float goalTime = 0.1f;

        while (transform.childCount > (gridSize + 1))
        {
            timer += Time.deltaTime;
            if (timer >= goalTime)
            {
                timer = 0f;
                int random = Random.Range(gridSize + 1, transform.childCount);

                Transform element = transform.GetChild(random);

                if (element.GetComponent<FlyIntoSpace>() != null) element.GetComponent<FlyIntoSpace>().Fly();
            }

            transform.Rotate(Vector3.forward, goalSpeed * Time.deltaTime);
            yield return null;
        }

        // decelerate
        currentSpeed = 360f;
        goalSpeed = 0f;

        while (currentSpeed > goalSpeed)
        {
            currentSpeed -= acceleration * Time.deltaTime;
            transform.Rotate(Vector3.forward, currentSpeed * Time.deltaTime);
            yield return null;
        }

        collapsing = false;
        collapsed = true;
    }
}
