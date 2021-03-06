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

    public GameObject emptyGameObject;

    void Start()
    {
        maximumStorage = 4;

        if (startOnPlanet) ecosystem = transform.parent.GetComponent<Ecosystem>();

        if ((house || spaceStation) == false)
        {
            if (tree == false) ecosystem.AddGas(buildingCost); // for building the building
            if (startOnPlanet == false) Produce();
            if (powerPlant) Produce();

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
            if (availableCrops < 1) return;

            for (int i = 0; i < 1; i++)
            {
                GameObject fieldWithMostCrops = null;
                int currentMostCrops = 0;

                foreach (GameObject otherField in ecosystem.fields)
                {
                    if (otherField.GetComponent<Building>().storedAmount > currentMostCrops)
                    {
                        currentMostCrops = otherField.GetComponent<Building>().storedAmount;
                        fieldWithMostCrops = otherField;
                    }
                }

                if (fieldWithMostCrops != null) fieldWithMostCrops.GetComponent<Building>().Consume();
            }

            InstantiateProduct();
        }

        ecosystem.AddGas(gasPerProductOnLevel[level]);
    }

    private void InstantiateProduct()
    {
        GameObject emptyParent = Instantiate(emptyGameObject, transform);
        GameObject newProduct = Instantiate(productPrefab, transform);

        emptyParent.transform.localScale = new Vector3(1f / transform.localScale.x, 1f / transform.localScale.y, 1f / transform.localScale.z);
        emptyParent.transform.position = ecosystem.transform.position;

        newProduct.transform.localScale = new Vector3(1f / transform.localScale.x, 1f / transform.localScale.y, 1f / transform.localScale.z) * 0.6f;

        emptyParent.transform.parent = null;
        newProduct.transform.parent = emptyParent.transform;

        float rotate = -1.5f + storedAmount;

        float angle;

        if (bikeFactory || carFactory) angle = -4f;
        else angle = -3f;

        emptyParent.transform.Rotate(Vector3.forward, rotate * angle);
        newProduct.transform.localPosition += Vector3.up * -0.5f;

        newProduct.transform.parent = transform;
        newProduct.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder -= storedAmount;

        Destroy(emptyParent);
        // newProduct.transform.localPosition += Vector3.up * -0.03f + Vector3.up * storedAmount * -0.06f;

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
        if (level > 2) return;
        
        level++;

        if (tree == false)
        {
            // ecosystem.AddGas(buildingCost);
        }

        Produce();

        // placeholder
        // transform.localScale += Vector3.up * 0.3f;
    }
}