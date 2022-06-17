using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGrid : MonoBehaviour
{
    private float planetRadius;

    public int gridSize;
    public float angleBetweenSpaces;

    private float gridSpaceSize = 1f;

    public GameObject gridSpacePrefab;
    public GameObject gridSpaceEdgePrefab;
    public List<GameObject> gridSpaces = new List<GameObject>();

    public GameObject spaceStationPrefab;
    public GameObject rocketPrefab;

    public GameObject housePrefab;

    public GameObject treePrefab;

    // blackmarket variables
    public GameObject blackmarketStandPrefab;
    public int[] standPositions;

    private void Start()
    {
        planetRadius = transform.GetChild(0).localScale.x / 2f;
        CreateGrid();

        GameObject newSpaceStation = Instantiate(spaceStationPrefab, gridSpaces[0].transform);
        newSpaceStation.transform.parent = transform;
        gridSpaces[0].GetComponent<GridSpace>().occupied = true;
        gridSpaces[0].GetComponent<GridSpace>().building = newSpaceStation;

        GameObject newHouse = Instantiate(housePrefab, gridSpaces[gridSize / 2].transform);
        newHouse.transform.parent = transform;
        // newHouse.transform.localScale = Vector3.one;
        gridSpaces[gridSize / 2].GetComponent<GridSpace>().occupied = true;
        gridSpaces[gridSize / 2].GetComponent<GridSpace>().building = newHouse;

        if (gameObject.tag == "Planet_A")
        {
            GameObject newRocket = Instantiate(rocketPrefab, gridSpaces[0].transform);
            newRocket.transform.parent = transform;
        }

        CreateTrees();

        for (int i = 0; i < 3; i++)
        {
            GameObject newBlackmarketStand = Instantiate(blackmarketStandPrefab, gridSpaces[standPositions[i]].transform);
            newBlackmarketStand.transform.parent = transform;

            switch (i)
            {
                case 0:
                    newBlackmarketStand.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
                    break;
                case 1:
                    newBlackmarketStand.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.black;
                    break;
                case 2:
                    newBlackmarketStand.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.blue;
                    break;
            }

            newBlackmarketStand.transform.GetChild(1).position = gridSpaces[standPositions[i] - 3].transform.position;

            if (standPositions[i] + 3 < 18)
            {
                newBlackmarketStand.transform.GetChild(2).position = gridSpaces[standPositions[i] + 3].transform.position;
            }
            else
            {
                newBlackmarketStand.transform.GetChild(2).position = gridSpaces[0].transform.position;
            }
        }
    }

    void CreateGrid()
    {
        angleBetweenSpaces = 360f / gridSize;

        for (int i = 0; i < gridSize; i++)
        {
            GameObject newSpace = Instantiate(gridSpacePrefab, transform);
            newSpace.transform.localPosition = new Vector3(0, planetRadius, 0);
            newSpace.transform.localScale = Vector3.one * gridSpaceSize;
            newSpace.transform.parent = null;

            // create new gridSpaceEdge
            float newAngle = angleBetweenSpaces * 0.5f;

            transform.Rotate(Vector3.forward, newAngle);
            GameObject newEdge = Instantiate(gridSpaceEdgePrefab, transform);
            newEdge.transform.localPosition = new Vector3(0, planetRadius, 0);
            newEdge.transform.parent = null;
            transform.Rotate(Vector3.forward, (-1f) * newAngle);
            newEdge.transform.parent = newSpace.transform;
            // end section

            if (gameObject.tag == "Planet_B")
            {
                gameObject.GetComponent<PlanetVisuals>().gridSpaceEdges.Add(newEdge);
            }

            gridSpaces.Add(newSpace);
            transform.Rotate(Vector3.forward, angleBetweenSpaces);
        }

        transform.rotation = Quaternion.identity;

        foreach (GameObject gridSpace in gridSpaces)
        {
            gridSpace.transform.parent = transform;
        }
    }

    public GameObject NextGridSpace(Vector3 otherPosition)
    {
        GameObject nextGridSpace = null;
        float minDistance = 42069f;

        foreach (GameObject gridSpace in gridSpaces)
        {
            float distance = (gridSpace.transform.position - otherPosition).magnitude;
            if (distance < minDistance)
            {
                minDistance = distance;
                nextGridSpace = gridSpace;
            }
        }
        
        if (nextGridSpace != null)
        {
            return nextGridSpace;
        }
        else
        {
            Debug.LogError("No matching space found.");
            return null;
        }
    }

    private void CreateTrees()
    {
        // int treesOnLeftSide = Random.Range(0, 12);
        // int treesOnRightSide = 11 - treesOnLeftSide;

        int treesOnLeftSide = 2;
        int treesOnRightSide = 2;

        int[] treesLeft = GetUniqueRandomArray(1, (gridSize / 2) - 1, treesOnLeftSide);
        int[] treesRight = GetUniqueRandomArray((gridSize / 2) + 1, gridSize - 1, treesOnRightSide);

        foreach (int i in treesLeft)
        {
            GameObject newTree = Instantiate(treePrefab, gridSpaces[i].transform);
            newTree.transform.parent = transform;
            // newTree.transform.localScale = Vector3.one * 0.6f; // placeholder

            gridSpaces[i].GetComponent<GridSpace>().occupied = true;
            gridSpaces[i].GetComponent<GridSpace>().building = newTree;
        }

        foreach (int i in treesRight)
        {
            GameObject newTree = Instantiate(treePrefab, gridSpaces[i].transform);
            newTree.transform.parent = transform;
            // newTree.transform.localScale = Vector3.one * 0.6f; // placeholder

            gridSpaces[i].GetComponent<GridSpace>().occupied = true;
            gridSpaces[i].GetComponent<GridSpace>().building = newTree;
        }
    }

    // copied from stackoverflow to have less work
    private int[] GetUniqueRandomArray(int min, int max, int count)
    {
        int[] result = new int[count];
        List<int> numbersInOrder = new List<int>();
        for (var x = min; x < max; x++)
        {
            numbersInOrder.Add(x);
        }
        for (var x = 0; x < count; x++)
        {
            var randomIndex = Random.Range(0, numbersInOrder.Count);
            result[x] = numbersInOrder[randomIndex];
            numbersInOrder.RemoveAt(randomIndex);
        }

        return result;
    }

    public Vector3 GetSpaceStationPosition()
    {
        return gridSpaces[0].transform.position;
    }

}