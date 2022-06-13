using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Population : MonoBehaviour
{
    public GameObject characterPrefab;

    private bool onPlanet_A;
    public List<GameObject> characters = new List<GameObject>();

    public List<Color> tierColors = new List<Color>();

    public List<Text> counters = new List<Text>();
    public int[] amountOfCharactersOfTier = new int[4];

    private int planet_B_startPopulation = 4;
    public int maxPopulation;

    public int populationComingWithNextRocket = 0;

    void Start()
    {
        if (gameObject.tag == "Planet_A")
        {
            onPlanet_A = true;
        }
        else
        {
            onPlanet_A = false;
        }

        /*
        for (int i = 0; i < 1; i++)
        {
            StartCoroutine(CreateTestCharacter());
        }
        */
        StartCoroutine(CreateStartCharacters());
    }

    private IEnumerator CreateStartCharacters()
    {
        // yield break; // placeholder

        if (onPlanet_A) yield break;

        // yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(6f);

        int amount = 0;

        while (amount < planet_B_startPopulation)
        {
            int random = Random.Range(0, 101);

            if (random <= 75)
            {
                CreateOnPlanet_B_Tier_1();
            }
            else
            {
                CreateOnPlanet_B_Tier_2();
            }

            amount++;
            yield return new WaitForSeconds(30f);
        }
    }

    public void CreateCharacterWithChances(int[] chances)
    {
        if (characters.Count >= maxPopulation - populationComingWithNextRocket) return;
        StartCoroutine(CreateCharacterWithChances_Co(chances));
    }

    private IEnumerator CreateCharacterWithChances_Co(int[] chances)
    {
        // determin waiting time so that the char spawns roughly in the middle of surrounding characters
        float waitingTime;
        float housePosition = 180f;

        float nextPosition = 42069f;

        // find character that is closest in front of the house
        foreach (GameObject character in characters)
        {
            Character characterScript = character.GetComponent<Character>();
            float characterPosition = characterScript.currentPosition;

            if (characterPosition > housePosition && characterPosition < nextPosition) nextPosition = characterPosition;
        }

        // Debug.Log(nextPosition);

        if (nextPosition == 42069f) nextPosition = 360f;

        waitingTime = (nextPosition - housePosition) / 6f;

        yield return new WaitForSeconds(waitingTime);
        
        int random = Random.Range(0, 101);

        if (random <= chances[0])
        {
            CreateOnPlanet_B_Tier_1();
        }
        else if (random <= chances[0] + chances[1])
        {
            CreateOnPlanet_B_Tier_2();
        }
        else if (random <= chances[0] + chances[1] + chances[2])
        {
            CreateOnPlanet_B_Tier_3();
        }
        else
        {
            CreateOnPlanet_B_Tier_4();
        }

        yield break;
    }

    public void CreateCharacterAtHouse(byte tier, bool planet_A)
    {
        if (tier < 1 || tier > 4) return;
        GameObject newChar = Instantiate(characterPrefab, transform);
        Character newCharScript = newChar.GetComponent<Character>();

        newCharScript.onPlanet_A = planet_A;
        newCharScript.tier = tier;
        newCharScript.movingRight = true;

        newChar.transform.localPosition += Vector3.up * (-1f) * transform.GetChild(0).localScale.x / 2f;

        float angle = 180f;
        newChar.transform.Rotate(Vector3.forward, angle);

        newCharScript.currentPosition = angle;
        newCharScript.positionSinceLastGridSpace = angle % gameObject.GetComponent<PlanetGrid>().angleBetweenSpaces;
        if (planet_A) newCharScript.goingToRocket = true;

        characters.Add(newChar);

        // placeholder
        newChar.transform.GetChild(0).GetComponent<SpriteRenderer>().color = tierColors[tier - 1];
        // newChar.GetComponent<Character>().logging = true;
    }

    public void CreateOnPlanet_B_Tier_1()
    {
        CreateCharacterAtHouse(1, false);
        UpdateCharacterCounter(1, 1);
    }

    public void CreateOnPlanet_B_Tier_2()
    {
        CreateCharacterAtHouse(2, false);
        UpdateCharacterCounter(2, 1);
    }

    public void CreateOnPlanet_B_Tier_3()
    {
        CreateCharacterAtHouse(3, false);
        UpdateCharacterCounter(3, 1);
    }

    public void CreateOnPlanet_B_Tier_4()
    {
        CreateCharacterAtHouse(4, false);
        UpdateCharacterCounter(4, 1);
    }

    public void UpdateCharacterCounter(int tier, int amount)
    {
        amountOfCharactersOfTier[tier - 1] += amount;
        counters[tier - 1].text = amountOfCharactersOfTier[tier - 1].ToString();
    }

    public void CreateOnPlanet_A_Tier_1()
    {
        CreateCharacterAtHouse(1, true);
    }

    public void CreateOnPlanet_A_Tier_2()
    {
        CreateCharacterAtHouse(2, true);
    }

    public void CreateOnPlanet_A_Tier_3()
    {
        CreateCharacterAtHouse(3, true);
    }

    public void CreateOnPlanet_A_Tier_4()
    {
        CreateCharacterAtHouse(4, true);
    }

    private IEnumerator CreateTestCharacter()
    {
        yield return new WaitForEndOfFrame();
        GameObject newChar = Instantiate(characterPrefab, transform);

        if (onPlanet_A)
        {
            newChar.GetComponent<Character>().onPlanet_A = true;
            newChar.GetComponent<Character>().onSurface = true;
            newChar.GetComponent<Character>().logging = false;

            newChar.GetComponent<Character>().tier = 1;

            // create on random point for testing
            Vector3 direction = new Vector3();
            direction.x = Random.Range(-10, 10);
            if (direction.x == 0) direction.x = 1;
            direction.y = Random.Range(-10, 10);
            direction = direction.normalized;

            newChar.transform.localPosition += direction * transform.GetChild(0).localScale.x / 2f;

            float angle;

            if (direction.x < 0)
            {
                angle = Vector3.Angle(Vector3.up, direction);
            }
            else
            {
                angle = 360f - Vector3.Angle(Vector3.up, direction);
            }

            newChar.transform.Rotate(Vector3.forward, angle);

            // Debug.Log(angle);

            newChar.GetComponent<Character>().currentPosition = angle;
            newChar.GetComponent<Character>().positionSinceLastGridSpace = angle % gameObject.GetComponent<PlanetGrid>().angleBetweenSpaces;
        }
        else
        {
            int i =  Random.Range(0, 2);

            if (i == 0)
            {
                newChar.GetComponent<Character>().movingRight = true;
                newChar.GetComponent<Character>().logging = true;
            }

            newChar.GetComponent<Character>().tier = 1;


            newChar.transform.localPosition += Vector3.up * transform.GetChild(0).localScale.x / 2f;
            newChar.GetComponent<Character>().onPlanet_A = false;
            newChar.GetComponent<Character>().onSurface = true;
        }

        characters.Add(newChar);
    }
}