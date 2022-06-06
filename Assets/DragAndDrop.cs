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
        if (currentGridSpace.GetComponent<GridSpace>().occupied == false)
        {
            currentGridSpaceIndicator.transform.position = currentGridSpace.transform.position;
            currentGridSpaceIndicator.transform.localScale = Vector3.one * 1f;

            levelingUp = false;
        }
        else if (currentGridSpace.GetComponent<GridSpace>().building.GetComponent<Building>().name == gameObject.GetComponent<Building>().name && currentGridSpace.GetComponent<GridSpace>().building.GetComponent<Building>().level < 2)
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

        if (currentGridSpace.GetComponent<GridSpace>().occupied && levelingUp == false)
        {
            Destroy(gameObject);
            return;
        }
        else if (currentGridSpace.GetComponent<GridSpace>().occupied && levelingUp == true)
        {
            currentGridSpace.GetComponent<GridSpace>().building.GetComponent<Building>().LevelUp();
            Destroy(gameObject);
            return;
        }
        else if (currentGridSpace.GetComponent<GridSpace>().occupied == false && levelingUp == false)
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
        gameObject.GetComponent<Building>().enabled = true;
        gameObject.GetComponent<Building>().ecosystem = transform.parent.GetComponent<Ecosystem>();

        // snap to planet radius
        // gameObject.transform.position = planet_a.transform.position - targetDir * planetRadius;
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