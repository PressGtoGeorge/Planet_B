using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketAnimation : MonoBehaviour
{
    public GameObject emptyGameObject; // for the instantiation of an empty parent

    private GameObject planet_A;
    private GameObject planet_B;

    private float safeHeightPlanet_A = 2f;
    private float safeHeightPlanet_B = 2f;

    private float radiusPlanet_A;
    private float radiusPlanet_B;

    private float distanceBetweenPlanetCores;

    private float defaultSpeed = 2f; // 0.8f;
    private float driftSpeed = 0.6f;
    private bool startingOnPlanet_A = true;

    private float lastDotProduct = 0f; // for determining switch point

    private bool flying;

    public bool OnPlanet_A()
    {
        if (startingOnPlanet_A) return true;
        else return false;
    }

    private void Start()
    {
        planet_A = GameObject.FindGameObjectWithTag("Planet_A");
        planet_B = GameObject.FindGameObjectWithTag("Planet_B");

        radiusPlanet_A = planet_A.transform.GetChild(0).localScale.x / 2f;
        radiusPlanet_B = planet_B.transform.GetChild(0).localScale.x / 2f;

        distanceBetweenPlanetCores = Mathf.Abs((planet_A.transform.position - planet_B.transform.position).magnitude);
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartAnimation();
        }
        */
    }

    public void StartAnimation()
    {
        if (flying) return;
        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        flying = true;

        // check if flight is possible (not necessary if size is predetermined by design)
        if (distanceBetweenPlanetCores < radiusPlanet_A + safeHeightPlanet_A + radiusPlanet_B + safeHeightPlanet_B)
        {
            Debug.Log("No route found.");
            yield break;
        }

        float speed = defaultSpeed;

        // first part of animation: get into safe hight and turn 90 degrees
        GameObject currentPlanet;

        float currentHeight = 0f;
        float goalHeight;
        float planetRadius;
        
        if (startingOnPlanet_A)
        {
            currentPlanet = planet_A;
            goalHeight = safeHeightPlanet_A;
            planetRadius = radiusPlanet_A;
        }
        else
        {
            currentPlanet = planet_B;
            goalHeight = safeHeightPlanet_B;
            planetRadius = radiusPlanet_B;
        }

        GameObject newParent = Instantiate(emptyGameObject, currentPlanet.transform);
        newParent.transform.parent = null;
        transform.parent = newParent.transform;

        while (currentHeight < goalHeight)
        {
            transform.parent.Rotate(Vector3.forward, 30f * speed * (Mathf.Sqrt(currentHeight / goalHeight)) * Time.deltaTime);

            transform.Rotate(Vector3.forward, 90f * (speed / goalHeight) * Time.deltaTime);

            transform.localPosition += Vector3.up * Time.deltaTime * speed;
            // currentHight += speed * Time.deltaTime;
            currentHeight = (currentPlanet.transform.position - transform.position).magnitude - planetRadius;

            yield return null;
        }

        // transform.parent = startPlanet.transform;


        // fix position
        Vector3 tempDir = (transform.position - currentPlanet.transform.position).normalized;

        currentHeight = goalHeight;
        transform.position = currentPlanet.transform.position + tempDir * (goalHeight + planetRadius);

        // fix rotation
        Quaternion qDir = new Quaternion();
        qDir.SetLookRotation(Vector3.forward, tempDir);
        transform.rotation = qDir;
        transform.Rotate(Vector3.forward, 90f);

        // Debug.Log("In orbit.\nAnimation 1 finished.");

        // second part of animation: rotate with rising radius around planet until reaching the middle between both planets
        // newParent.transform.parent = null;
        goalHeight = (distanceBetweenPlanetCores - radiusPlanet_A - radiusPlanet_B) / 2f;

        while (currentHeight < goalHeight)
        {
            currentHeight += speed * driftSpeed * Time.deltaTime;
            transform.localPosition += Vector3.up * Time.deltaTime * speed * driftSpeed;

            transform.parent.Rotate(Vector3.forward, 30f * (speed) * Time.deltaTime);

            yield return null;
        }

        // fix position
        tempDir = (transform.position - currentPlanet.transform.position).normalized;
        transform.position = currentPlanet.transform.position + tempDir * (goalHeight + planetRadius);

        // Debug.Log("Reached target height.\nAnimation 2 finished.");

        // wait for moment to turn onto the other planet
        Vector3 vectorBetweenPlanets = (planet_A.transform.position - planet_B.transform.position); // points from b to a

        if (startingOnPlanet_A == false) vectorBetweenPlanets *= (-1f);

        while (true)
        {
            transform.parent.Rotate(Vector3.forward, 30f * (speed) * Time.deltaTime);

            float dotProduct = Vector3.Dot(vectorBetweenPlanets, transform.up);
            if (dotProduct >= 0 && lastDotProduct < 0)
            {
                lastDotProduct = 0f;

                // Debug.Log("Switch point passed.\nAnimation 3 finished.");
                break;
            }
            else
            {
                lastDotProduct = dotProduct;
            }

            yield return null;
        }

        // fix position
        transform.position = currentPlanet.transform.position + (-1f) * vectorBetweenPlanets.normalized * (goalHeight + planetRadius);

        // fix rotation
        qDir = new Quaternion();
        qDir.SetLookRotation(Vector3.forward, (-1f) * vectorBetweenPlanets);
        transform.rotation = qDir;
        transform.Rotate(Vector3.forward, 90f);

        // check currentheight for both planets
        // Debug.Log((transform.position - planet_A.transform.position).magnitude - radiusPlanet_A);
        // Debug.Log((transform.position - planet_B.transform.position).magnitude - radiusPlanet_B);

        // start rotating around other planet and get closer to safe hight
        if (startingOnPlanet_A)
        {
            transform.parent = null;
            // newParent.transform.parent = planet_B.transform;
            newParent.transform.position = planet_B.transform.position;
            transform.parent = newParent.transform;

            currentPlanet = planet_B;
            goalHeight = safeHeightPlanet_B;
            planetRadius = radiusPlanet_B;
        }
        else
        {
            transform.parent = null;
            // newParent.transform.parent = planet_A.transform;
            newParent.transform.position = planet_A.transform.position;
            transform.parent = newParent.transform;

            currentPlanet = planet_A;
            goalHeight = safeHeightPlanet_A;
            planetRadius = radiusPlanet_A;
        }

        // currentHeight = (distanceBetweenPlanetCores - radiusPlanet_A - radiusPlanet_B) / 2f - planetRadius;

        while (currentHeight > goalHeight)
        {
            currentHeight += speed * (-1f) * driftSpeed * Time.deltaTime;
            transform.localPosition += Vector3.up * Time.deltaTime * speed * driftSpeed;

            transform.parent.Rotate(Vector3.forward, (-1f) * 30f * (speed) * Time.deltaTime);
            yield return null;
        }

        // fix position
        tempDir = (transform.position - currentPlanet.transform.position).normalized;
        transform.position = currentPlanet.transform.position + tempDir * (goalHeight + planetRadius);

        // Debug.Log("Reached target height.\nAnimation 4 finished.");

        // look for position of space station
        Vector3 spaceStationPosition;

        while (true)
        {
            transform.parent.Rotate(Vector3.forward, (-1f) * 30f * (speed) * Time.deltaTime);

            spaceStationPosition = currentPlanet.GetComponent<PlanetGrid>().GetSpaceStationPosition();
            Vector3 vectorBetweenShipAndStation = (transform.position - spaceStationPosition).normalized;

            float dotProduct = Vector3.Dot(vectorBetweenShipAndStation, transform.up);
            if (dotProduct >= 0 && lastDotProduct < 0)
            {
                lastDotProduct = 0f;

                // Debug.Log("Found space station.\nAnimation 5 finished.");
                break;
            }
            else
            {
                lastDotProduct = dotProduct;
            }

            yield return null;
        }

        // fix position
        spaceStationPosition = currentPlanet.GetComponent<PlanetGrid>().GetSpaceStationPosition();
        transform.position = currentPlanet.transform.position + (spaceStationPosition - currentPlanet.transform.position).normalized * (planetRadius + goalHeight);

        // move a bit forward to create space for final circle
        if (currentPlanet.GetComponent<RotatePlanet>().collapsing == false)
            newParent.transform.parent = currentPlanet.transform;

        float currentPos = 0f;
        float lastCircleRadius = 0.4f * goalHeight;

        while (currentPos < lastCircleRadius)
        {
            transform.position += transform.up * speed * Time.deltaTime * 2f;
            currentPos += speed * Time.deltaTime * 2f;

            yield return null;
        }

        // fix position
        transform.position += (transform.up) * (lastCircleRadius - currentPos);

        // Debug.Log("Prepared final circle.\nAnimation 6 finished.");

        // do final circle until above space station
        transform.parent = null;
        newParent.transform.position = transform.position + transform.right * (-1f) * lastCircleRadius;
        transform.parent = newParent.transform;

        float currentAngle = 0f;
        float goalAngle = 270f;

        while (currentAngle < goalAngle)
        {
            transform.parent.Rotate(Vector3.forward, (3f) * 30f * (speed) * Time.deltaTime);
            currentAngle += speed * (3f) * 30f * Time.deltaTime;
            yield return null;
        }

        // fix rotation
        // transform.parent.Rotate(Vector3.forward, goalAngle - currentAngle); // change this
        spaceStationPosition = currentPlanet.GetComponent<PlanetGrid>().GetSpaceStationPosition();
        Vector3 vectorBetweenSpaceStationAndCore = (spaceStationPosition - currentPlanet.transform.position);

        qDir = new Quaternion();
        qDir.SetLookRotation(Vector3.forward, (-1f) * vectorBetweenSpaceStationAndCore);
        transform.rotation = qDir;

        // fix position
        if (currentPlanet.GetComponent<RotatePlanet>().collapsing == false && currentPlanet.GetComponent<RotatePlanet>().collapsed == false)
        {
            spaceStationPosition = currentPlanet.GetComponent<PlanetGrid>().GetSpaceStationPosition();
            transform.position = currentPlanet.transform.position + (spaceStationPosition - currentPlanet.transform.position).normalized * (planetRadius + goalHeight + lastCircleRadius);
        }
        // Debug.Log("Over space station.\nAnimation 7 finished.");

        // land rocket on space station
        float undergroundAmount = 0.5f;
        currentPos = 0f;
        float goalPos = goalHeight + lastCircleRadius + undergroundAmount;

        while (currentPos < goalPos)
        {
            transform.position += transform.up * speed * Time.deltaTime;
            currentPos += speed * Time.deltaTime;

            yield return null;
        }

        // fix position
        transform.position += transform.up * (goalPos - currentPos);
        
        if (currentPlanet.GetComponent<RotatePlanet>().collapsing == false && currentPlanet.GetComponent<RotatePlanet>().collapsed == false)
        {
            // rotate 180
            transform.Rotate(Vector3.forward, 180);

            // Debug.Log("Rocket in space station.\nAnimation 8 finished.");

            // redeploy rocket on space station


            currentPos = 0f;

            while (currentPos < undergroundAmount)
            {
                transform.position += transform.up * speed * Time.deltaTime;
                currentPos += speed * Time.deltaTime;

                yield return null;
            }

            // fix position
            transform.position = currentPlanet.GetComponent<PlanetGrid>().GetSpaceStationPosition();
        }

        // set variables
        transform.parent = currentPlanet.transform;
        Destroy(newParent);

        startingOnPlanet_A = !startingOnPlanet_A;
        flying = false;

        // Debug.Log("Rocket ready for new take off.\nAnimation 9 finished.");

        if (currentPlanet.GetComponent<RotatePlanet>().collapsed == false) gameObject.GetComponent<Rocket>().StartTravelSchedule();

        yield break;
    }
}