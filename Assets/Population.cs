using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Population : MonoBehaviour
{
    public GameObject characterPrefab;

    private bool onPlanet_A;
    private List<GameObject> characters = new List<GameObject>();

    public List<Color> tierColors = new List<Color>();

    public List<Text> counters = new List<Text>();
    private int[] numbers = new int[4];

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

        for (int i = 0; i < 1; i++)
        {
            // StartCoroutine(CreateTestCharacter());
        }
    }

    public void CreateCharacter(byte tier, bool planet_A)
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

        newChar.GetComponent<Character>().currentPosition = angle;
        newChar.GetComponent<Character>().positionSinceLastGridSpace = angle % gameObject.GetComponent<PlanetGrid>().angleBetweenSpaces;

        newChar.transform.GetChild(0).GetComponent<SpriteRenderer>().color = tierColors[tier - 1];
    }

    public void Create1()
    {
        CreateCharacter(1, false);
        numbers[0]++;
        counters[0].text = numbers[0].ToString();
    }

    public void Create2()
    {
        CreateCharacter(2, false);
        numbers[1]++;
        counters[1].text = numbers[1].ToString();
    }

    public void Create3()
    {
        CreateCharacter(3, false);
        numbers[2]++;
        counters[2].text = numbers[2].ToString();
    }

    public void Create4()
    {
        CreateCharacter(4, false);
        numbers[3]++;
        counters[3].text = numbers[3].ToString();
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