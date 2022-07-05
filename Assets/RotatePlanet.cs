using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlanet : MonoBehaviour
{
    private bool scaledTime = false;
    private float deltaTime;
    private float speed = 2f;

    public bool collapsing;
    public bool collapsed;

    private int gridSize;

    public bool rotating;

    private float yearDuration = 120f;
    private float timePassedSinceLastYear;
    public int yearsPassed = 0;
    public int charactersPassed = 0;

    private Ecosystem ecosystem;
    private GameObject rocket;

    private AudioSource collapseSource;

    [Range(0, 1)] public float minVolume;
    [Range(0, 1)] public float maxVolume;

    [Range(-3, 3)] public float minPitch;
    [Range(-3, 3)] public float maxPitch;

    private void Start()
    {
        gridSize = gameObject.GetComponent<PlanetGrid>().gridSize;
        if (gameObject.tag == "Planet_B") collapseSource = gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        ecosystem = gameObject.GetComponent<Ecosystem>();

        SetDeltaTime();

        if (collapsed == false && rotating) transform.Rotate(Vector3.forward, speed * deltaTime);

        timePassedSinceLastYear += Time.deltaTime; // always unscaled
        if (timePassedSinceLastYear >= yearDuration)
        {
            timePassedSinceLastYear -= yearDuration;
            yearsPassed++;
            // Debug.Log("Year: " + yearsPassed);
        }

        if (ecosystem.GetCurrentGas() >= 1000 && collapsing == false && collapsed == false)
        {
            StartCoroutine(Collapse());
        }

        // placeholder
        if (Input.GetKeyDown(KeyCode.L) && collapsing == false)
        {
            // StartCoroutine(Collapse());
        }

        StartCoroutine(GetRocket());
    }

    private IEnumerator GetRocket()
    {
        yield return new WaitForEndOfFrame();
        rocket = GameObject.FindGameObjectWithTag("Rocket");
    }

    private IEnumerator Collapse()
    {
        if (gameObject.tag == "Planet_A") yield break;

        GameState.gameOver = true;
        collapsing = true;

        collapseSource.Play();

        // make rocket fly into outer space
        rocket.GetComponent<FlyIntoSpace>().Fly();

        // start rotating with acceleration

        float currentSpeed = 0;
        float goalSpeed = 360f;

        float acceleration = 40f;

        while (currentSpeed < goalSpeed)
        {
            currentSpeed += acceleration * Time.unscaledDeltaTime; // always unscaled
            transform.Rotate(Vector3.forward, currentSpeed * Time.unscaledDeltaTime); // always unscaled

            collapseSource.volume = minVolume + (currentSpeed / goalSpeed) * (maxVolume - minVolume);
            collapseSource.pitch = minPitch + (currentSpeed / goalSpeed) * (maxPitch - minPitch);

            yield return null;
        }

        collapseSource.volume = maxVolume;
        collapseSource.pitch = maxPitch;

        // make objects on planet fly into space
        float timer = 0f;
        float goalTime = 0.2f;

        while (transform.childCount > (gridSize + 1))
        {
            timer += deltaTime;
            if (timer >= goalTime)
            {
                timer = 0f;
                int random = Random.Range(gridSize + 1, transform.childCount);

                Transform element = transform.GetChild(random);

                if (element.GetComponent<FlyIntoSpace>() != null) element.GetComponent<FlyIntoSpace>().Fly();
            }

            transform.Rotate(Vector3.forward, goalSpeed * Time.unscaledDeltaTime); // always unscaled
            yield return null;
        }

        // decelerate
        currentSpeed = 360f;
        goalSpeed = 0f;

        while (currentSpeed > goalSpeed)
        {
            currentSpeed -= acceleration * deltaTime;
            transform.Rotate(Vector3.forward, currentSpeed * Time.unscaledDeltaTime); // always unscaled

            collapseSource.volume = (currentSpeed / 360f) * maxVolume;
            collapseSource.pitch = minPitch + (currentSpeed / 360f) * (maxPitch - minPitch);

            yield return null;
        }

        collapseSource.volume = 0;
        collapseSource.pitch = minPitch;
        collapseSource.Stop();

        collapsing = false;
        collapsed = true;
    }

    private void SetDeltaTime()
    {
        if (scaledTime)
        {
            deltaTime = Time.deltaTime;
        }
        else
        {
            deltaTime = Time.unscaledDeltaTime;
        }
    }
}