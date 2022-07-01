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

    void Start()
    {
        gridSpaces = gameObject.GetComponent<PlanetGrid>().gridSpaces;

        CreateBuilding(5, 1);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            CreateBuilding(7, 0);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            DestroyBuilding(7);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            LevelUpBuilding(7);
        }
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

        if (type == 0) newBuilding.GetComponent<Building>().LevelUp();

        gridSpaces[pos].GetComponent<GridSpace>().occupied = true;
        gridSpaces[pos].GetComponent<GridSpace>().building = newBuilding;
    }

    private void DestroyBuilding(int pos)
    {
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