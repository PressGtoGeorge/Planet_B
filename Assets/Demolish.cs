using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demolish : MonoBehaviour
{
    public GameObject currentGridSpaceIndicator;

    private bool dragging = true;

    private GameObject planet_B;
    private PlanetGrid planetGrid;

    private void Start()
    {
        planet_B = GameObject.FindGameObjectWithTag("Planet_B");
        planetGrid = planet_B.GetComponent<PlanetGrid>();
    }

    private void Update()
    {
        if (dragging)
        {
            FollowMouse();
            if (Input.GetMouseButtonUp(0)) DestroyElement();
        }
    }

    private void FollowMouse()
    {
        // enable indicator
        currentGridSpaceIndicator.SetActive(true);

        // follow mouseposition
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;

        // mark next grid space
        GameObject currentGridSpace = planetGrid.NextGridSpace(transform.position);
        bool house = (currentGridSpace.GetComponent<GridSpace>().building != null && currentGridSpace.GetComponent<GridSpace>().building.GetComponent<Building>().house);
        bool spaceStation = (currentGridSpace.GetComponent<GridSpace>().building != null && currentGridSpace.GetComponent<GridSpace>().building.GetComponent<Building>().spaceStation);

        // snap to rotation
        RotateTowardsSurface(currentGridSpace);

        if (currentGridSpace.GetComponent<GridSpace>().occupied == true && house == false && spaceStation == false)
        {
            // für später, wenn art da ist
            // currentGridSpaceIndicator.GetComponent<SpriteRenderer>().sprite = currentGridSpace.GetComponent<GridSpace>().building.GetComponent<SpriteRenderer>().sprite; 
            
            currentGridSpaceIndicator.transform.position = currentGridSpace.transform.position;
        }
        else
        {
            currentGridSpaceIndicator.SetActive(false);
        }
    }

    private void DestroyElement()
    {
        if (dragging) dragging = false;

        GameObject currentGridSpace = planetGrid.NextGridSpace(transform.position);
        bool house = (currentGridSpace.GetComponent<GridSpace>().building != null && currentGridSpace.GetComponent<GridSpace>().building.GetComponent<Building>().house);
        bool spaceStation = (currentGridSpace.GetComponent<GridSpace>().building != null && currentGridSpace.GetComponent<GridSpace>().building.GetComponent<Building>().spaceStation);

        if (currentGridSpace.GetComponent<GridSpace>().occupied == false || house == true || spaceStation == true)
        {
            Destroy(gameObject);
            return;
        }

        currentGridSpace.GetComponent<GridSpace>().occupied = false;

        // delete field from ecosystem list
        if (currentGridSpace.GetComponent<GridSpace>().building.GetComponent<Building>().field)
            planetGrid.gameObject.GetComponent<Ecosystem>().fields.Remove(currentGridSpace.GetComponent<GridSpace>().building);

        // add gas for destruction
        if (currentGridSpace.GetComponent<GridSpace>().building.GetComponent<Building>().tree)
        {
            planetGrid.gameObject.GetComponent<Ecosystem>().AddGas(12);
        }
        else
        {
            planetGrid.gameObject.GetComponent<Ecosystem>().AddGas(1);
        }

        // destroy pointer from gridspace to building
        Destroy(currentGridSpace.GetComponent<GridSpace>().building);
        currentGridSpace.GetComponent<GridSpace>().building = null;
        currentGridSpace.GetComponent<GridSpace>().occupied = false;

        Destroy(gameObject);
    }

    private void RotateTowardsSurface(GameObject currentGridSpace)
    {
        if (currentGridSpace == null) return;

        Vector3 targetDir = planet_B.transform.position - currentGridSpace.transform.position; // gameObject.transform.position;
        targetDir = targetDir.normalized;

        Quaternion qDir = new Quaternion();
        qDir.SetLookRotation(Vector3.forward, (-1) * targetDir);
        gameObject.transform.rotation = qDir;
    }
}