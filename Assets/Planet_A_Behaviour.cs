using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet_A_Behaviour : MonoBehaviour
{
    public GameObject treePrefab;
    public GameObject fieldPrefab;
    public GameObject farmPrefab;
    public GameObject windWheelPrefab;
    public GameObject powerPlantPrefab;
    public GameObject bikeFactoryPrefab;
    public GameObject carFactoryPrefab;

    private List<GameObject> gridSpaces;

    private int[] rightSide = { 1, 1, 2, 2, 3, 4, 6, 6 }; // int[8]
    private int[] leftSide = { 1, 1, 2, 2, 4, 4, 5, 6 }; // int[8]

    private int[] rightSideFinal = { 0, 1, 3, 0, 5, 1, 0, 3 };
    private int[] leftSideFinal = { 0, 1, 3, 0, 5, 1, 0, 3 };

    // 6 trees, 4 fields, 4 wheels, 2 bikes

    private int[] rightOrder;
    private int[] leftOrder;

    private int progression= 0;

    private int[] replacementOrder;

    void Start()
    {
        gridSpaces = gameObject.GetComponent<PlanetGrid>().gridSpaces;
        CreateStartBuildings();
        replacementOrder = PlanetGrid.GetUniqueRandomArray(0, 16, 16);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            // ProgressPlanet();
        }
    }

    private void CreateStartBuildings()
    {
        rightOrder = PlanetGrid.GetUniqueRandomArray(0, 8, 8);

        for (int i = 0; i < rightSide.Length; i++)
        {
            CreateBuilding(rightOrder[i] + 1, rightSide[i]);
        }

        leftOrder = PlanetGrid.GetUniqueRandomArray(0, 8, 8);

        for (int i = 0; i < leftSide.Length; i++)
        {
            CreateBuilding(leftOrder[i] + 1 + 9, leftSide[i]);
        }

        StartCoroutine(LevelUpStartBuildings());
    }

    private IEnumerator LevelUpStartBuildings()
    {
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < rightSide.Length; i++)
        {
            LevelUpBuilding(i + 1);
            LevelUpBuilding(i + 1);
        }

        for (int i = 0; i < leftSide.Length; i++)
        {
            LevelUpBuilding(i + 1 + 9);
            LevelUpBuilding(i + 1 + 9);
        }
    }

    public void ProgressPlanet()
    {
        if (progression > 15) return;

        int progress = replacementOrder[progression];

        if (progress <= 7)
        {
            ReplaceBuilding(progress, false);
        }
        else
        {
            ReplaceBuilding(progress - 8, true);
        }

        progression++;
    }

    private void ReplaceBuilding(int pos, bool left) 
    {
        int position;
        int type;

        if (left)
        {
            position = pos + 9 + 1;
            type = leftSideFinal[pos];
        }
        else
        {
            position = pos + 1;
            type = rightSideFinal[pos];
        }

        DestroyBuilding(position);
        CreateBuilding(position, type);
        StartCoroutine(LevelUpOverTime(position));
    }

    private IEnumerator LevelUpOverTime(int pos)
    {
        yield return new WaitForSeconds(10);
        LevelUpBuilding(pos);
        yield return new WaitForSeconds(10);
        LevelUpBuilding(pos);
    }

    private void CreateBuilding(int pos, int type)
    {
        if (gridSpaces[pos].GetComponent<GridSpace>().building != null) return;

        GameObject prefab = null;

        switch (type)
        {
            case 0:
                prefab = treePrefab;
                break;
            case 1:
                prefab = fieldPrefab;
                break;
            case 2:
                prefab = farmPrefab;
                break;
            case 3:
                prefab = windWheelPrefab;
                break;
            case 4:
                prefab = powerPlantPrefab;
                break;
            case 5:
                prefab = bikeFactoryPrefab;
                break;
            case 6:
                prefab = carFactoryPrefab;
                break;
        }

        GameObject newBuilding = Instantiate(prefab, gridSpaces[pos].transform);
        newBuilding.transform.parent = transform;
        newBuilding.GetComponent<DragAndDrop>().currentGridSpaceIndicator.SetActive(false);
        newBuilding.GetComponent<DragAndDrop>().enabled = false;
        newBuilding.GetComponent<Building>().enabled = true;
        newBuilding.GetComponent<Building>().startOnPlanet = true;

        if (type == 0) newBuilding.GetComponent<Building>().LevelUp(); // tree prefab starts on level -1

        gridSpaces[pos].GetComponent<GridSpace>().occupied = true;
        gridSpaces[pos].GetComponent<GridSpace>().building = newBuilding;
    }

    private void DestroyBuilding(int pos)
    {
        if (gridSpaces[pos].GetComponent<GridSpace>().building.GetComponent<Building>().field)
            gameObject.GetComponent<Ecosystem>().fields.Remove(gridSpaces[pos].GetComponent<GridSpace>().building);

        Destroy(gridSpaces[pos].GetComponent<GridSpace>().building);
        gridSpaces[pos].GetComponent<GridSpace>().building = null;
    }

    private void LevelUpBuilding(int pos)
    {
        if (gridSpaces[pos].GetComponent<GridSpace>().building == null) return;

        Building building = gridSpaces[pos].GetComponent<GridSpace>().building.GetComponent<Building>();

        if (building.level >= 2) return;

        if (building.tree == false && building.windWheel == false)
        {
            building.transform.GetChild(building.level).gameObject.SetActive(false);
            building.LevelUp();
            building.transform.GetChild(building.level).gameObject.SetActive(true);
        }
        else
        {
            building.LevelUp();
            building.transform.GetChild(building.level).gameObject.SetActive(true);
        }
    }
}