using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour
{
    public GameObject currentGridSpaceIndicator;

    private bool dragging = true;

    private GameObject planet_B;
    private PlanetGrid planetGrid;

    // private float planetRadius;

    private bool levelingUp;

    private void Start()
    {
        planet_B = GameObject.FindGameObjectWithTag("Planet_B");
        planetGrid = planet_B.GetComponent<PlanetGrid>();
        // planetRadius = planet_B.transform.GetChild(0).localScale.x / 2f;
    }

    private void Update()
    {
        if (dragging)
        {
            FollowMouse();
            if (Input.GetMouseButtonUp(0)) ReleaseElement();
        }
    }
    /*
    public void OnMouseDown()
    {
        // startOffset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        transform.parent = null;
        // transform.rotation = Quaternion.identity;

        // update gridspace if necessary
        if (planetGrid.NextGridSpace(transform.position).transform.position == transform.position)
        {
            planetGrid.NextGridSpace(transform.position).GetComponent<GridSpace>().occupied = false;
        }
    }
    
    private void OnMouseUp()
    {
        ReleaseElement();
    }
    
    void OnMouseDrag()
    {
        FollowMouse();
    }
    */
    private void FollowMouse()
    {
        // enable indicator
        currentGridSpaceIndicator.SetActive(true);

        // follow mouseposition
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;

        // mark next grid space
        GameObject currentGridSpace = planetGrid.NextGridSpace(transform.position);

        // see if building on space could be leveled up
        bool occupied = currentGridSpace.GetComponent<GridSpace>().occupied;
        bool sameBuildingType = false;
        bool belowLevelThree = false;
        bool growingTree = false;


        if (occupied)
        {
            sameBuildingType = (currentGridSpace.GetComponent<GridSpace>().building.GetComponent<Building>().buildingName == gameObject.GetComponent<Building>().buildingName);
            belowLevelThree = (currentGridSpace.GetComponent<GridSpace>().building.GetComponent<Building>().level < 2);

            if (currentGridSpace.GetComponent<GridSpace>().building.GetComponent<Building>().tree)
            {
                growingTree = currentGridSpace.GetComponent<GridSpace>().building.GetComponent<Trees>().growing;
            }
        }

        bool fitForLevelUp = sameBuildingType && belowLevelThree && (growingTree == false);
        // end section

        if (occupied == false)
        {
            currentGridSpaceIndicator.transform.position = currentGridSpace.transform.position;
            currentGridSpaceIndicator.transform.localScale = Vector3.one * 1f;

            levelingUp = false;
        }
        else if (fitForLevelUp)
        {
            // mark building for level up
            currentGridSpaceIndicator.transform.position = currentGridSpace.transform.position;
            currentGridSpaceIndicator.transform.localScale = Vector3.one * 1.5f;

            levelingUp = true;
        }
        else
        {
            currentGridSpaceIndicator.SetActive(false);

            levelingUp = false;
        }

        // snap to rotation
        RotateTowardsSurface(currentGridSpace);

    }

    private void ReleaseElement()
    {
        if (dragging) dragging = false;

        GameObject currentGridSpace = planetGrid.NextGridSpace(transform.position);
        bool occupied = currentGridSpace.GetComponent<GridSpace>().occupied;

        if (occupied && levelingUp == false)
        {
            Destroy(gameObject);
            return;
        }
        else if (occupied && levelingUp == true)
        {
            if (currentGridSpace.GetComponent<GridSpace>().building.GetComponent<Building>().tree == false)
            {
                currentGridSpace.GetComponent<GridSpace>().building.GetComponent<Building>().LevelUp();
            }
            else
            {
                currentGridSpace.GetComponent<GridSpace>().building.GetComponent<Trees>().StartGrowth();
            }
            
            Destroy(gameObject);
            return;
        }
        else if (occupied == false && levelingUp == false)
        {
            currentGridSpace.GetComponent<GridSpace>().occupied = true;
        }

        // snap to rotation
        RotateTowardsSurface(currentGridSpace);

        // snap to grid
        transform.position = currentGridSpace.transform.position;
        transform.parent = planet_B.transform;

        // disable indicator
        currentGridSpaceIndicator.SetActive(false);

        // create pointer from gridspace to building
        currentGridSpace.GetComponent<GridSpace>().building = gameObject;

        // creater pointer from building to ecosystem
        if (currentGridSpace.GetComponent<GridSpace>().building.GetComponent<Building>().tree == false) 
            gameObject.GetComponent<Building>().enabled = true;
        else gameObject.GetComponent<Trees>().enabled = true;

        gameObject.GetComponent<Building>().ecosystem = transform.parent.GetComponent<Ecosystem>();
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