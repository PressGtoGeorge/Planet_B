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

    public float radiusPlanet_A;
    public float radiusPlanet_B;

    public float surfacePathDepth;
    public float blackmarketPathDepth;

    public List<SpriteRenderer> characterRenderer = new List<SpriteRenderer>();

    public PlanetGrid planetGrid;
    private int gridSize;
    private float distanceBetweenGridSpaces;

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
    private int lowEndSatisfactionDuration = 10;
    private int highEndSatisfactionDuration = 31;

    // speeds for different mobility options
    private bool moving = true;
    private float speed;

    private float defaultSpeed = 3f;
    private float bikeSpeed = 4f;
    private float carSpeed = 5f;
    private float mountSpeed = 5f;

    private int vehicleDuration = 180;
    private Coroutine resetSpeed;

    // variables for rocket travel
    public bool goingToRocket;

    private bool goingToBlackmarket;
    private int blackmarketStandPosition;
    private int blackmarketGasPerProduct = 18;

    public bool goingHome; // true if character disappears next time they move by the house

    // visualising needs and time left to fulfill
    public GameObject thoughtBubble;
    public Image coolDownIndicator;

    // variables for spawning new characters
    public float[] newNpcsPerPass;
    private List<int[]> spawnChancesAfterYears = new List<int[]>();

    public int[] spawnChanceAfter_0;
    public int[] spawnChanceAfter_1;
    public int[] spawnChanceAfter_2;
    public int[] spawnChanceAfter_3;


    // variables for replacing characters that leave with rocket
    private List<int[]> replacementCharacterTierChances = new List<int[]>();

    public int[] replacementCharacterTierChance_1;
    public int[] replacementCharacterTierChance_2;
    public int[] replacementCharacterTierChance_3;

    public bool stationary; // true if a character does never use the rocket

    private int characterNumber; // for sorting every character into their own layer to avoid conflicts

    public Animator animator;

    public SpriteMask spriteMask; // for blackmartket entry/exit animation
    public SpriteMask spriteMask_1;
    private BlackmarketAnimation blackmarketAnimation;

    public int startingSpace;

    void Start()
    {
        planet_A = GameObject.FindGameObjectWithTag("Planet_A");
        planet_B = GameObject.FindGameObjectWithTag("Planet_B");

        radiusPlanet_A = planet_A.transform.GetChild(0).localScale.x / 2f;
        radiusPlanet_B = planet_B.transform.GetChild(0).localScale.x / 2f;

        surfacePathDepth = 0.225f;
        blackmarketPathDepth = 0.925f;

        speed = defaultSpeed;

        spawnChancesAfterYears.Add(spawnChanceAfter_0);
        spawnChancesAfterYears.Add(spawnChanceAfter_1);
        spawnChancesAfterYears.Add(spawnChanceAfter_2);
        spawnChancesAfterYears.Add(spawnChanceAfter_3);

        replacementCharacterTierChances.Add(replacementCharacterTierChance_1);
        replacementCharacterTierChances.Add(replacementCharacterTierChance_2);
        replacementCharacterTierChances.Add(replacementCharacterTierChance_3);

        SetupPlanetVariables();
        SetupConsumerVariables();
        /*
        if (onSurface == false)
        {
            transform.Rotate(Vector3.forward, 180);
        }
        */
        if (movingRight) directionMultiplier = (-1);
        else directionMultiplier = 1;

        if (goingToRocket == false) StartCoroutine(CreateNeed());
        else thoughtBubble.SetActive(false);

        // place character and thoughbubble in seperate sortingOrder to avoid conflicts
        GameState.totalCharacters++;
        if (GameState.totalCharacters >= 100) GameState.totalCharacters = 0;
        characterNumber = GameState.totalCharacters;

        characterRenderer.Add(transform.GetChild(0).GetComponent<SpriteRenderer>());
        characterRenderer.Add(transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>());
        characterRenderer.Add(transform.GetChild(0).GetChild(1).GetComponent<SpriteRenderer>());

        foreach (SpriteRenderer renderer in characterRenderer)
        {
            renderer.sortingOrder = 510 + characterNumber;
            renderer.flipX = true;
        }

        // setup sprite mask for blackmarket animation
        spriteMask.frontSortingOrder = 510 + characterNumber;
        spriteMask.backSortingOrder = 509 + characterNumber;

        spriteMask_1.frontSortingOrder = 510 + characterNumber;
        spriteMask_1.backSortingOrder = 509 + characterNumber;

        MoveToSurface();

        SetThoughtBubbleSortOrder();
        SetupAnimation();

        PositionCharacterOnPlanet_A();
    }

    void Update()
    {
        MoveAroundPlanet();

        if (Input.GetKeyDown(KeyCode.K))
        {
            // animator.SetBool("mount", true);
        }
    }

    private void MoveAroundPlanet()
    {
        if (moving == false) return;

        transform.parent.Rotate(Vector3.forward, speed * Time.deltaTime * directionMultiplier);

        currentPosition += speed * Time.deltaTime * directionMultiplier;
        if (movingRight == false && currentPosition > 360f) currentPosition -= 360f;
        if (movingRight && currentPosition < 0f) currentPosition += 360f;

        int currentGridSpace = Mathf.RoundToInt(currentPosition / distanceBetweenGridSpaces);
        if (currentGridSpace >= gridSize) currentGridSpace -= gridSize;

        positionSinceLastGridSpace += speed * Time.deltaTime;
        if (positionSinceLastGridSpace > distanceBetweenGridSpaces)
        {
            positionSinceLastGridSpace -= distanceBetweenGridSpaces;

            // old position of block

            if (logging)
            {
                // Debug.Log(currentGridSpace);
                if (planetGrid.gridSpaces[currentGridSpace].GetComponent<GridSpace>().building == null) return;
                string newName = planetGrid.gridSpaces[currentGridSpace].GetComponent<GridSpace>().building.GetComponent<Building>().buildingName;
                Debug.Log("Passed: " + newName);
            }

            if (goingHome && onSurface)
            {
                GoHome(currentGridSpace);
                return;
            }

            if (tier == 1) WaterTree(currentGridSpace);
            if (goingToRocket) GoToRocket(currentGridSpace);
            if (goingToBlackmarket) GoToBlackmarket(currentGridSpace);
            SpawnNewCharacters(currentGridSpace);
        }

        Consume(currentGridSpace);
    }

    private void GoHome(int currentGridSpace)
    {
        if (currentGridSpace != gridSize * 0.5f || onPlanet_A == false) return;
        planet_A.GetComponent<Population>().characters.Remove(gameObject);
        Destroy(gameObject);
    }

    private IEnumerator CreateNeed()
    {
        // yield break;
        wantsFood = false;
        wantsPower = false;
        wantsMobility = false;

        if (satisfied)
        {
            satisfied = false;
            int time = Random.Range(lowEndSatisfactionDuration, highEndSatisfactionDuration);

            yield return new WaitForSeconds(time);
        }

        int random;
        if (speed == defaultSpeed)
        {
            random = Random.Range(0, 5);
        }
        else
        {
            random = Random.Range(0, 4); // to avoid somebody with vehicle to want a new one
        }

        int duration;

        if (random <= 1)
        {
            wantsFood = true;
            duration = 61;
        }
        else if (random <= 3)
        {
            wantsPower = true;
            duration = 61;
        }
        else
        {
            wantsMobility = true;
            duration = 91;
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

        // create new need if satisfied or go to blackmarket/rocket if not
        if (wantsFood || wantsPower || wantsMobility)
        {
            if ((wantsFood && goodFood) || (wantsPower && goodPower) || (wantsMobility && goodMobility))
            {
                if (onPlanet_A)
                {
                    StartCoroutine(CreateNeed());
                    planet_A.GetComponent<Ecosystem>().AddGas(blackmarketGasPerProduct * 0.5f);

                    yield break;
                }

                // thoughtBubble.SetActive(false);
                goingToRocket = true;

                UpdateThoughtBubble();

                if (onPlanet_A == false) CreateReplacementCharacters();
            }
            else
            {
                // thoughtBubble.SetActive(false);
                goingToBlackmarket = true;
                UpdateThoughtBubble();
                FindBlackmarketStand();
            }
        }
        else
        {
            satisfied = true;
            StartCoroutine(CreateNeed());
        }

        yield break;
    }

    private void FindBlackmarketStand()
    {
        if (wantsFood)
        {
            blackmarketStandPosition = 0;
        }
        else if (wantsPower)
        {
            blackmarketStandPosition = 6;
        }
        else if (wantsMobility)
        {
            blackmarketStandPosition = 12;
        }
    }

    private void UpdateThoughtBubble()
    {
        if (onPlanet_A)
        {
            thoughtBubble.SetActive(false);
            return;
        }

        int type = 0;

        if (goingToRocket)
        {
            type = 6;
        }
        else if (goingToBlackmarket)
        {
            type = 7;
        }
        else if (wantsFood && goodFood)
        {
            type = 0;
        }
        else if (wantsFood && goodFood == false)
        {
            type = 1;
        }
        else if (wantsPower && goodPower)
        {
            type = 2;
        }
        else if (wantsPower && goodPower == false)
        {
            type = 3;
        }
        else if (wantsMobility && goodMobility)
        {
            type = 4;
        }
        else if (wantsMobility && goodMobility == false)
        {
            type = 5;
        }

        // thoughtBubble.transform.GetChild(0).GetComponent<SpriteRenderer>().color = thoughtBubble.GetComponent<ThoughtBubble>().colors[color];
        thoughtBubble.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = thoughtBubble.GetComponent<ThoughtBubble>().symbols[type];
        thoughtBubble.SetActive(true);
    }

    private void GoToRocket(int currentGridSpace)
    {
        if (currentGridSpace != 0 || stationary) return;

        wantsFood = false;
        wantsPower = false;
        wantsMobility = false;

        if (onPlanet_A == false) stationary = true;
        goingToRocket = false;
        planetGrid.gridSpaces[0].GetComponent<GridSpace>().building.GetComponent<SpaceStation>().AddEnteringPassenger(gameObject);

        planetGrid.gameObject.GetComponent<Population>().characters.Remove(gameObject);
        if (onPlanet_A == false) planetGrid.gameObject.GetComponent<Population>().UpdateCharacterCounter(tier, -1); // placeholder ?

        onPlanet_A = !onPlanet_A;
        // Debug.Log("Entering rocket.");
    }

    private void CreateReplacementCharacters()
    {
        // return; // placeholder

        if (planet_B.GetComponent<RotatePlanet>().collapsing || planet_B.GetComponent<RotatePlanet>().collapsed) return;

        Population planet_A_Population = planet_A.GetComponent<Population>();
        Population planet_B_Population = planet_B.GetComponent<Population>();

        planet_A_Population.RemoveCharacterPlanet_A();

        if (planet_B_Population.characters.Count < planet_B_Population.maxPopulation - planet_B_Population.populationComingWithNextRocket - 1)
        {
            // create two semi-random replacements

            // Debug.Log("create 2");

            int[] replacementChances = replacementCharacterTierChances[tier - 1];

            for (int i = 0; i < 2; i++) // placeholder, kinda
            {
                int random = Random.Range(0, 101);

                if (random < replacementChances[0])
                {
                    planet_A_Population.CreateOnPlanet_A_Tier_1();
                }
                else if (random < replacementChances[0] + replacementChances[1])
                {
                    planet_A_Population.CreateOnPlanet_A_Tier_2();
                }
                else if (random < replacementChances[0] + replacementChances[1] + replacementChances[2])
                {
                    planet_A_Population.CreateOnPlanet_A_Tier_3();
                }
                else // if (random < replacementChances[0] + replacementChances[1] + replacementChances[2] + replacementChances[3])
                {
                    planet_A_Population.CreateOnPlanet_A_Tier_4();
                }
            }
            planet_B_Population.populationComingWithNextRocket += 2;
        }
        else
        {
            // Debug.Log("create 1");

            // create replacement that is 1 tier worse
            switch (tier)
            {
                case 1:
                    planet_A_Population.CreateOnPlanet_A_Tier_2();
                        break;
                case 2:
                    planet_A_Population.CreateOnPlanet_A_Tier_3();
                    break;
                case 3:
                    planet_A_Population.CreateOnPlanet_A_Tier_4();
                    break;
            }

            planet_B_Population.populationComingWithNextRocket += 1;
        }
    }

    private void GoToBlackmarket(int currentGridSpace)
    {
        if (onSurface)
        {
            GameObject currentPlanet;

            if (onPlanet_A) currentPlanet = planet_A;
            else currentPlanet = planet_B;

            // check when char is over part that fits need
            if ((currentGridSpace > blackmarketStandPosition && (currentGridSpace < blackmarketStandPosition + 6 || currentGridSpace == 0)) && goingHome == false)
            {
                MoveToBlackmarket();

                currentPlanet.GetComponent<Ecosystem>().AddGas(blackmarketGasPerProduct);

                // check if riding hamster
                if (wantsMobility)
                {
                    SetSpeed(mountSpeed);
                    // animator.SetBool("mount", true);
                    resetSpeed = StartCoroutine(ResetVehicleAfter(vehicleDuration));
                }

                wantsFood = false;
                wantsPower = false;
                wantsMobility = false;
            }
            else
            {
                currentPlanet.GetComponent<Ecosystem>().AddGas(1.5f);
            }
        }
        else
        {
            // check if character reached end of blackmarket section
            if (currentGridSpace == blackmarketStandPosition)
            {
                MoveToSurface();

                goingToBlackmarket = false;
                StartCoroutine(CreateNeed());
            }
        }
    }

    private void SpawnNewCharacters(int currentGridSpace)
    {
        // return; // placeholder
        if (currentGridSpace != (gridSize / 2) || goingToRocket /*|| goingToBlackmarket*/ || onPlanet_A || planet_B.GetComponent<RotatePlanet>().collapsed || planet_B.GetComponent<RotatePlanet>().collapsing) return;

        RotatePlanet planetScript = planet_B.GetComponent<RotatePlanet>();
        
        int year = planetScript.yearsPassed;
        if (year > 3) year = 3;
        planetScript.charactersPassed++;

        if (planetScript.charactersPassed >= (1 / newNpcsPerPass[year]))
        {
            planetScript.charactersPassed = 0;
            planet_B.GetComponent<Population>().CreateCharacterWithChances(spawnChancesAfterYears[year]);
        }
    }

    private void Consume(int currentGridSpace)
    {
        if (onSurface == false || goingToRocket || goingToBlackmarket) return;
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
                resetSpeed = StartCoroutine(ResetVehicleAfter(vehicleDuration));
                animator.SetBool("bike", true);
            }

            if (goodMobility == false && buildingScript.carFactory && buildingScript.storedAmount > 0)
            {
                buildingScript.Consume();
                wantsMobility = false;
                thoughtBubble.SetActive(false);

                SetSpeed(carSpeed);
                resetSpeed = StartCoroutine(ResetVehicleAfter(vehicleDuration));
                animator.SetBool("car", true);

                characterRenderer[1].enabled = true;
                characterRenderer[2].enabled = true;

            }
        }
    }

    private void WaterTree(int currentGridSpace)
    {
        if (onSurface == false || goingToRocket) return;
        GameObject building = planetGrid.gridSpaces[currentGridSpace].GetComponent<GridSpace>().building;
        Trees treeScript;

        if (building == null || building.GetComponent<Building>().tree == false) return;
        else treeScript = building.GetComponent<Trees>();

        if (treeScript.growing)
        {
            treeScript.SpeedUpGrowth();
            StartCoroutine(WaterAnimation());
        }
    }

    private IEnumerator WaterAnimation()
    {
        moving = false;
        animator.SetBool("watering", true);
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("watering", false);
        if (GameState.gameOver == false || onPlanet_A) moving = true;
    }

    private IEnumerator ResetVehicleAfter(int duration)
    {
        yield return new WaitForSeconds(duration);

        animator.SetBool("bike", false);
        animator.SetBool("mount", false);
        animator.SetBool("car", false);

        characterRenderer[1].enabled = false;
        characterRenderer[2].enabled = false;

        SetSpeed(defaultSpeed);
        yield break;
    }

    private void SetupPlanetVariables()
    {
        GameObject currentPlanet;

        if (onPlanet_A) currentPlanet = planet_A;
        else currentPlanet = planet_B;

        GameObject newParent = Instantiate(emptyGameObject, currentPlanet.transform);
        newParent.transform.Rotate(Vector3.forward, currentPosition);
        transform.parent = newParent.transform;
        newParent.AddComponent<FlyIntoSpace>();

        MoveToSurface();

        planetGrid = currentPlanet.GetComponent<PlanetGrid>();
        gridSize = planetGrid.gridSize;
        distanceBetweenGridSpaces = planetGrid.angleBetweenSpaces;
    }

    private void MoveToSurface()
    {
        if (onSurface)
        {
            // transform.Rotate(Vector3.forward, 180f);
            transform.localPosition = Vector3.up * (radiusPlanet_A - surfacePathDepth);
        }
        else
        {
            blackmarketAnimation.StartExitAnimation();
        }

        if (onPlanet_A == false) thoughtBubble.SetActive(true);
        onSurface = true;
    }

    private void MoveToBlackmarket()
    {
        // transform.Rotate(Vector3.forward, 180f);
        // transform.localPosition = Vector3.up * (radiusPlanet_A - blackmarketPathDepth);

        if (wantsMobility) blackmarketAnimation.getsMount = true;
        blackmarketAnimation.StartEnterAnimation();
        /*
        foreach (SpriteRenderer renderer in characterRenderer)
        {
            renderer.sortingOrder = 210 + characterNumber;
            renderer.flipX = false;
        }

        // setup sprite mask for blackmarket animation
        spriteMask.frontSortingOrder = 210 + characterNumber;
        spriteMask.backSortingOrder = 209 + characterNumber;
        
        thoughtBubble.SetActive(false);
        */
        onSurface = false;
    }


    public void UpdateCharacterAfterLanding() // after leaving rocket
    {
        GameObject currentPlanet;

        if (onPlanet_A) currentPlanet = planet_A;
        else currentPlanet = planet_B;

        planetGrid = currentPlanet.GetComponent<PlanetGrid>();
        gridSize = planetGrid.gridSize;
        distanceBetweenGridSpaces = planetGrid.angleBetweenSpaces;

        currentPosition = 0f;

        if (resetSpeed != null) StopCoroutine(resetSpeed);
        SetSpeed(defaultSpeed);

        StartCoroutine(CreateNeed());

        // next part is necessary because of unity bug? or i am overlooking something
        transform.localRotation = Quaternion.identity;

        MoveToSurface();
        SetupAnimation();
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

    private void SetThoughtBubbleSortOrder()
    {
        SpriteRenderer thoughBubbleBackground = transform.GetChild(1).GetComponent<SpriteRenderer>();
        SpriteRenderer thoughBubbleSymbol = transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>();
        Canvas percentCanvas = transform.GetChild(1).GetChild(1).GetComponent<Canvas>();

        thoughBubbleBackground.sortingOrder += characterNumber * 10;
        thoughBubbleSymbol.sortingOrder += characterNumber * 10;
        percentCanvas.sortingOrder += characterNumber * 10;
    }

    private void SetupAnimation()
    {
        blackmarketAnimation = gameObject.GetComponent<BlackmarketAnimation>();

        animator = transform.GetChild(0).GetComponent<Animator>();

        switch (tier)
        {
            case 2:
                animator.SetBool("tier2", true);
                break;
            case 3:
                animator.SetBool("tier3", true);
                break;
            case 4:
                animator.SetBool("tier4", true);
                break;
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void SetMoving(bool move)
    {
        moving = move;
    }

    private void PositionCharacterOnPlanet_A()
    {
        if (startingSpace == 0) return;
        currentPosition = startingSpace * distanceBetweenGridSpaces;
        transform.parent.Rotate(Vector3.forward, currentPosition);

        transform.localRotation = Quaternion.identity;
    }
}