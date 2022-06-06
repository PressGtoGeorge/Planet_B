using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public bool logging; // for debug purposes
    public bool movingRight;
    private int directionMultiplier;

    public GameObject emptyGameObject;
    public bool onPlanet_A;
    public bool onSurface = true;

    private GameObject planet_A;
    private GameObject planet_B;

    private float radiusPlanet_A;
    private float radiusPlanet_B;

    public PlanetGrid planetGrid;
    private int gridSize;
    private float distanceBetweenGridSpaces;

    private float speed = 3f;

    public float currentPosition; // tracks current position on globe from 0 to 360 degrees
    public float positionSinceLastGridSpace;

    // variables for consumer behaviour
    private bool wantsFood;
    private bool wantsPower;
    private bool wantsMobility;

    public byte tier;

    private bool goodFood;
    private bool goodPower;
    private bool goodMobility;

    private bool satisfied;

    // speeds for different mobility options
    private float defaultSpeed = 3f;
    private float bikeSpeed = 4f;
    private float carSpeed = 5f;
    private float mountSpeed = 5f;

    // variables for rocket travel
    private bool goingToRocket;

    // visualising needs and time left to fulfill
    public GameObject thoughtBubble;
    public Image coolDownIndicator;

    void Start()
    {
        planet_A = GameObject.FindGameObjectWithTag("Planet_A");
        planet_B = GameObject.FindGameObjectWithTag("Planet_B");

        radiusPlanet_A = planet_A.transform.GetChild(0).localScale.x / 2f;
        radiusPlanet_B = planet_B.transform.GetChild(0).localScale.x / 2f;

        // speed = 3f;
        if (onPlanet_A)
        {
            speed *= radiusPlanet_B / radiusPlanet_A; // assuming planet_B is bigger
        }

        SetupPlanetVariables();
        SetupConsumerVariables();

        if (onSurface == false)
        {
            transform.Rotate(Vector3.forward, 180);
        }

        if (movingRight) directionMultiplier = (-1);
        else directionMultiplier = 1;

        StartCoroutine(CreateNeed());
    }

    private IEnumerator CreateNeed()
    {
        wantsFood = false;
        wantsPower = false;
        wantsMobility = false;

        if (satisfied)
        {
            satisfied = false;
            int time = Random.Range(10, 21);

            yield return new WaitForSeconds(time);
        }

        int random = Random.Range(0, 5);
        int duration;

        if (random <= 1)
        {
            wantsFood = true;
            duration = 60;
        }
        else if (random <= 3)
        {
            wantsPower = true;
            duration = 60;
        }
        else
        {
            wantsMobility = true;
            duration = 90;
        }

        UpdateThoughtBubble();

        // yield return new WaitForSeconds(duration);

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            coolDownIndicator.fillAmount = timer / duration;
            yield return null;
        }

        coolDownIndicator.fillAmount = 0f;
        SetSpeed(defaultSpeed);

        // create new need if satisfied or go to blackmarket/rocket if not
        if (wantsFood || wantsPower || wantsMobility)
        {
            if ((wantsFood && goodFood) || (wantsPower && goodPower) || (wantsMobility && goodMobility))
            {
                thoughtBubble.SetActive(false);
                goingToRocket = true;
            }
            else GoToBlackmarket();
        }
        else
        {
            satisfied = true;
            StartCoroutine(CreateNeed());
        }

        yield break;
    }

    private void UpdateThoughtBubble()
    {
        int color = 0;

        if (wantsFood && goodFood)
        {
            color = 0;
        }
        else if (wantsFood && goodFood == false)
        {
            color = 1;
        }
        else if (wantsPower && goodPower)
        {
            color = 2;
        }
        else if (wantsPower && goodPower == false)
        {
            color = 3;
        }
        else if (wantsMobility && goodMobility)
        {
            color = 4;
        }
        else if (wantsMobility && goodMobility == false)
        {
            color = 5;
        }

        thoughtBubble.transform.GetChild(0).GetComponent<SpriteRenderer>().color = thoughtBubble.GetComponent<ThoughtBubble>().colors[color];
        thoughtBubble.SetActive(true);
    }

    private void GoToRocket(int currentGridSpace)
    {
        if (currentGridSpace != 0) return;

        wantsFood = false;
        wantsPower = false;
        wantsMobility = false;

        onPlanet_A = !onPlanet_A;
        goingToRocket = false;
        planetGrid.gridSpaces[0].GetComponent<GridSpace>().building.GetComponent<SpaceStation>().AddEnteringPassenger(gameObject);
        Debug.Log("Entering rocket. If it works.");
    }

    private void GoToBlackmarket()
    {
        // placeholder function
        GameObject currentPlanet;

        if (onPlanet_A) currentPlanet = planet_A;
        else currentPlanet = planet_B;

        currentPlanet.GetComponent<Ecosystem>().AddGas(4);

        wantsFood = false;
        wantsPower = false;
        wantsMobility = false;

        StartCoroutine(CreateNeed());
    }

    void Update()
    {
        MoveAroundPlanet();
    }

    private void MoveAroundPlanet()
    {
        transform.parent.Rotate(Vector3.forward, speed * Time.deltaTime * directionMultiplier);

        currentPosition += speed * Time.deltaTime * directionMultiplier;
        if (movingRight == false && currentPosition > 360f) currentPosition -= 360f;
        if (movingRight && currentPosition < 0f) currentPosition += 360f;

        positionSinceLastGridSpace += speed * Time.deltaTime;
        if (positionSinceLastGridSpace > distanceBetweenGridSpaces)
        {
            positionSinceLastGridSpace -= distanceBetweenGridSpaces;

            int currentGridSpace = Mathf.RoundToInt(currentPosition / distanceBetweenGridSpaces);
            if (currentGridSpace >= gridSize) currentGridSpace -= gridSize;

            if (logging)
            {
                if (planetGrid.gridSpaces[currentGridSpace].GetComponent<GridSpace>().building == null) return;
                string newName = planetGrid.gridSpaces[currentGridSpace].GetComponent<GridSpace>().building.GetComponent<Building>().buildingName;
                Debug.Log("Passed: " + newName);
            }

            if (goingToRocket) GoToRocket(currentGridSpace);
            Consume(currentGridSpace);
        }
    }

    private void Consume(int currentGridSpace)
    {
        if (onSurface == false || goingToRocket) return;
        GameObject building = planetGrid.gridSpaces[currentGridSpace].GetComponent<GridSpace>().building;
        Building buildingScript;

        if (building == null) return;
        else buildingScript = building.GetComponent<Building>();

        if (wantsFood)
        {
            if (goodFood && buildingScript.field && buildingScript.storedAmount > 0)
            {
                buildingScript.Consume();
                wantsFood = false;
                thoughtBubble.SetActive(false);
            }

            if (goodFood == false && buildingScript.farm && buildingScript.storedAmount > 0)
            {
                buildingScript.Consume();
                wantsFood = false;
                thoughtBubble.SetActive(false);
            }
        }
        else if (wantsPower)
        {
            if (goodPower && (buildingScript.windWheel || buildingScript.powerPlant) && buildingScript.storedAmount > 0)
            {
                buildingScript.Consume();
                wantsPower = false;
                thoughtBubble.SetActive(false);
            }

            if (goodPower == false && (buildingScript.windWheel || buildingScript.powerPlant) && buildingScript.storedAmount > 1)
            {
                buildingScript.Consume();
                buildingScript.Consume();
                wantsPower = false;
                thoughtBubble.SetActive(false);
            }
        }
        else if (wantsMobility)
        {
            if (goodMobility && buildingScript.bikeFactory && buildingScript.storedAmount > 0)
            {
                buildingScript.Consume();
                wantsMobility = false;
                thoughtBubble.SetActive(false);

                SetSpeed(bikeSpeed);
            }

            if (goodMobility == false && buildingScript.carFactory && buildingScript.storedAmount > 0)
            {
                buildingScript.Consume();
                wantsMobility = false;
                thoughtBubble.SetActive(false);

                SetSpeed(carSpeed);
            }
        }
    }

    private void SetupPlanetVariables()
    {
        GameObject currentPlanet;

        if (onPlanet_A) currentPlanet = planet_A;
        else currentPlanet = planet_B;

        GameObject newParent = Instantiate(emptyGameObject, currentPlanet.transform);
        transform.parent = newParent.transform;
        newParent.AddComponent<FlyIntoSpace>();

        planetGrid = currentPlanet.GetComponent<PlanetGrid>();
        gridSize = planetGrid.gridSize;
        distanceBetweenGridSpaces = planetGrid.angleBetweenSpaces;
    }

    // after leaving rocket
    public void UpdateCharacterAfterLanding()
    {
        GameObject currentPlanet;

        if (onPlanet_A) currentPlanet = planet_A;
        else currentPlanet = planet_B;

        planetGrid = currentPlanet.GetComponent<PlanetGrid>();
        gridSize = planetGrid.gridSize;
        distanceBetweenGridSpaces = planetGrid.angleBetweenSpaces;

        currentPosition = 0f;

        StartCoroutine(CreateNeed());

        // next part is necessary because of unity bug? or i am overlooking something
        transform.localRotation = Quaternion.identity;
        transform.localPosition = Vector3.up * radiusPlanet_A; // because both have same radius
    }

    private void SetupConsumerVariables()
    {
        if (tier == 1)
        {
            goodFood = true;
            goodPower = true;
            goodMobility = true;
        }
        else if (tier == 2)
        {
            int random = Random.Range(0, 3);

            switch (random)
            {
                case 0:
                    goodFood = true;
                    goodPower = true;
                    break;
                case 1:
                    goodFood = true;
                    goodMobility = true;
                    break;
                case 2:
                    goodPower = true;
                    goodMobility = true;
                    break;
            }
        }
        else if (tier == 3)
        {
            int random = Random.Range(0, 3);

            switch (random)
            {
                case 0:
                    goodFood = true;
                    break;
                case 1:
                    goodPower = true;
                    break;
                case 2:
                    goodMobility = true;
                    break;
            }
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
}