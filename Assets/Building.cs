using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    // type of building
    public bool tree;
    public bool field;
    public bool farm;
    public bool windWheel;
    public bool powerPlant;
    public bool bikeFactory;
    public bool carFactory;

    public bool house;
    public bool spaceStation;

    public string buildingName;

    // production variables
    public GameObject productPrefab;
    
    public int level;

    public float[] productionTimeOnLevel;

    public float[] gasPerProductOnLevel;

    [HideInInspector] public int storedAmount;
    private int maximumStorage;

    private Stack<GameObject> storedProducts = new Stack<GameObject>();

    public Ecosystem ecosystem;
    public bool startOnPlanet;
    private float buildingCost = 12f;

    void Start()
    {
        maximumStorage = 4;

        if (startOnPlanet) ecosystem = transform.parent.GetComponent<Ecosystem>();

        if ((house || spaceStation) == false)
        {
            ecosystem.AddGas(buildingCost); // for building the building
            Produce();

            StartCoroutine(Production());
        }
        
        if (field) ecosystem.fields.Add(gameObject);

        // Debug.Log("Building build.");
    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Consuming products for test.");
            Consume();
        }
        */
    }

    private IEnumerator Production()
    {
        yield return new WaitForSeconds(productionTimeOnLevel[level]);
        
        Produce();
        StartCoroutine(Production());

        yield break;
    }

    public void Produce()
    {
        if (storedAmount >= maximumStorage) return;

        if (productPrefab != null && farm == false)
        {
            InstantiateProduct();
        }
        else if (productPrefab != null && farm == true)
        {
            int availableCrops = 0;

            // check total available crops
            foreach (GameObject otherField in ecosystem.fields)
            {
                availableCrops += otherField.GetComponent<Building>().storedAmount;
            }

            // use plants for animal production if possible

            if (availableCrops < 2) return;

            int usedCrops = 0;

            foreach (GameObject otherField in ecosystem.fields)
            {
                if (otherField.GetComponent<Building>().storedAmount >= 1 && usedCrops == 0)
                {
                    for (int i = 0; i < otherField.GetComponent<Building>().storedAmount; i++)
                    {
                        otherField.GetComponent<Building>().Consume();
                        usedCrops++;
                    }
                    
                }
                else if (otherField.GetComponent<Building>().storedAmount >= 1 && usedCrops == 1)
                {
                    otherField.GetComponent<Building>().Consume();
                    usedCrops++;
                }
            }

            InstantiateProduct();
        }

        ecosystem.AddGas(gasPerProductOnLevel[level]);
    }

    private void InstantiateProduct()
    {
        GameObject newProduct = Instantiate(productPrefab, transform);
        newProduct.transform.localPosition += Vector3.up * storedAmount * 0.25f;

        storedProducts.Push(newProduct);

        storedAmount++;
    }

    public void Consume()
    {
        if (storedAmount <= 0) return;
        GameObject product = storedProducts.Pop();
        Destroy(product);
        storedAmount--;
    }

    public void LevelUp()
    {
        if (level < 2) level++;

        // placeholder
        transform.localScale += Vector3.up * 0.3f;
    }
}