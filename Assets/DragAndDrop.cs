using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour
{
    public GameObject currentGridSpaceIndicator;

    public List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    private bool dragging = true;

    private GameObject planet_B;
    private PlanetGrid planetGrid;

    private bool levelingUp;

    private GameObject ui_background;
    private bool over_UI;

    private Building buildingScript;

    private GameObject lastGridSpace;
    private Sprite originalIndicatorSprite;

    // for windwheels
    private Sprite originalIndicatorSpriteWheel;
    private Vector3[] windWheelOffset = new Vector3[3];

    public AudioSource dropSource;

    private void Start()
    {
        planet_B = GameObject.FindGameObjectWithTag("Planet_B");
        planetGrid = planet_B.GetComponent<PlanetGrid>();

        ui_background = GameObject.FindGameObjectWithTag("UI_Background");

        buildingScript = gameObject.GetComponent<Building>();

        if (buildingScript.tree)
        {
            int random = Random.Range(0, 4);

            Sprite grownSprite = gameObject.GetComponent<TreeSprite>().fullyGrown[random];
            Sprite sapplingSprite = gameObject.GetComponent<TreeSprite>().sappling[random / 2];

            spriteRenderers[0].sprite = grownSprite;
            currentGridSpaceIndicator.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = sapplingSprite;

            gameObject.GetComponent<TreeSprite>().nextType = random;
        }

        originalIndicatorSprite = currentGridSpaceIndicator.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;

        if (buildingScript.windWheel)
        {
            originalIndicatorSpriteWheel = currentGridSpaceIndicator.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite;
            
            windWheelOffset[0] = transform.GetChild(0).GetChild(0).localPosition;
            windWheelOffset[1] = transform.GetChild(1).GetChild(0).localPosition;
            windWheelOffset[2] = transform.GetChild(2).GetChild(0).localPosition;
        }

        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.sortingOrder += 10000;
        }

        dropSource = gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (dragging)
        {
            FollowMouse();
            if (Input.GetMouseButtonUp(0)) ReleaseElement();
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
        bool occupied = currentGridSpace.GetComponent<GridSpace>().occupied;

        GameObject currentBuilding = null;
        if (occupied) currentBuilding = currentGridSpace.GetComponent<GridSpace>().building;

        // see if mouse is over UI background
        over_UI = ui_background.GetComponent<Collider2D>().OverlapPoint(mousePosition);

        // see if building on space could be leveled up
        bool sameBuildingType = false;
        bool belowLevelThree = false;
        bool growingTree = false;

        if (occupied)
        {
            sameBuildingType = (currentBuilding.GetComponent<Building>().buildingName == gameObject.GetComponent<Building>().buildingName);
            belowLevelThree = (currentBuilding.GetComponent<Building>().level < 2);

            if (currentBuilding.GetComponent<Building>().tree)
            {
                growingTree = currentBuilding.GetComponent<Trees>().growing;
            }
        }

        bool fitForLevelUp = sameBuildingType && belowLevelThree && (growingTree == false);
        // end section

        if (occupied == false && over_UI == false)
        {
            currentGridSpaceIndicator.transform.position = currentGridSpace.transform.position;
            // currentGridSpaceIndicator.transform.localScale = Vector3.one * 1f; // placeholder

            if (buildingScript.windWheel == true)
            {
                currentGridSpaceIndicator.transform.localPosition += Vector3.right * (0.22f);
                currentGridSpaceIndicator.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = originalIndicatorSprite;
                currentGridSpaceIndicator.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = originalIndicatorSpriteWheel;

                currentGridSpaceIndicator.transform.GetChild(0).GetChild(0).localPosition = windWheelOffset[0];
            }

            if (gameObject.GetComponent<Building>().tree == false && gameObject.GetComponent<Building>().windWheel == false)
            {
                currentGridSpaceIndicator.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = originalIndicatorSprite;
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = originalIndicatorSprite;
            }

            foreach (SpriteRenderer renderer in spriteRenderers)
            {
                renderer.color = Color.white;
            }

            levelingUp = false;
        }
        else if (fitForLevelUp && over_UI == false)
        {
            // mark building for level up

            if (buildingScript.tree || buildingScript.windWheel)
            {
                int level = currentBuilding.GetComponent<Building>().level;

                if (level == 0)
                {
                    currentGridSpaceIndicator.transform.position = currentBuilding.transform.GetChild(1).position;
                }
                else
                {
                    currentGridSpaceIndicator.transform.position = currentBuilding.transform.GetChild(2).position;
                }

                if (buildingScript.windWheel)
                {
                    currentGridSpaceIndicator.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = currentBuilding.transform.GetChild(level + 1).GetComponent<SpriteRenderer>().sprite;
                    currentGridSpaceIndicator.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = currentBuilding.transform.GetChild(level + 1).GetChild(0).GetComponent<SpriteRenderer>().sprite;
                    
                    currentGridSpaceIndicator.transform.GetChild(0).GetChild(0).localPosition = windWheelOffset[level + 1];
                }
            }
            else
            {
                currentGridSpaceIndicator.transform.position = currentGridSpace.transform.position;

                int level = currentBuilding.GetComponent<Building>().level;

                currentBuilding.transform.GetChild(level).GetComponent<SpriteRenderer>().enabled = false;

                Sprite upgradedSprite = currentBuilding.transform.GetChild(level + 1).GetComponent<SpriteRenderer>().sprite;
                currentGridSpaceIndicator.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = upgradedSprite;
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = upgradedSprite;
            }
            // currentGridSpaceIndicator.transform.localScale = Vector3.one * 1.5f; //placeholder

            foreach (SpriteRenderer renderer in spriteRenderers)
            {
                renderer.color = Color.white;
            }

            levelingUp = true;
        }
        else // if drag and drop fails
        {
            currentGridSpaceIndicator.SetActive(false);

            if (buildingScript.tree == false)
            {
                currentGridSpaceIndicator.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = originalIndicatorSprite;
                transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = originalIndicatorSprite;

                if (buildingScript.windWheel)
                {
                    currentGridSpaceIndicator.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = originalIndicatorSpriteWheel;
                    currentGridSpaceIndicator.transform.GetChild(0).GetChild(0).localPosition = windWheelOffset[0];
                }
            }

            foreach (SpriteRenderer renderer in spriteRenderers)
            {
                renderer.color = Color.red;
            }

            levelingUp = false;
        }

        // snap to rotation
        RotateTowardsSurface(currentGridSpace);

        // make buildings reappear if levelup was denied
        if (lastGridSpace != null && lastGridSpace != currentGridSpace)
        {
            int lastLevel = lastGridSpace.GetComponent<GridSpace>().building.GetComponent<Building>().level;
            lastGridSpace.GetComponent<GridSpace>().building.transform.GetChild(lastLevel).GetComponent<SpriteRenderer>().enabled = true;
        }

        bool tree = false;
        bool windWheel = false;

        if (occupied)
        {
            tree = currentBuilding.GetComponent<Building>().tree;
            windWheel = currentBuilding.GetComponent<Building>().windWheel;
        }
        if (currentBuilding != null && tree == false && windWheel == false) // && lastGridSpace != currentGridSpace) 
            lastGridSpace = currentGridSpace;
        else 
            lastGridSpace = null;
    }

    private void ReleaseElement()
    {
        if (dragging) dragging = false;

        foreach (SpriteRenderer renderer in spriteRenderers)
        {
            renderer.sortingOrder -= 10000;
        }

        GameObject currentGridSpace = planetGrid.NextGridSpace(transform.position);
        bool occupied = currentGridSpace.GetComponent<GridSpace>().occupied;

        GameObject currentBuilding = null;
        if (occupied) currentBuilding = currentGridSpace.GetComponent<GridSpace>().building;

        if ((occupied && levelingUp == false) || over_UI)
        {
            Destroy(gameObject);
            return;
        }
        else if ((occupied && levelingUp == true) && over_UI == false)
        {
            currentBuilding.GetComponent<AudioSource>().Play();

            if (currentBuilding.GetComponent<Building>().tree)
            {
                currentBuilding.GetComponent<TreeSprite>().nextType = gameObject.GetComponent<TreeSprite>().nextType;
                currentBuilding.GetComponent<Trees>().StartGrowth();
            }
            else // if (currentBuilding.GetComponent<Building>().windWheel)
            {
                int level = currentBuilding.GetComponent<Building>().level + 1;

                currentBuilding.transform.GetChild(level).gameObject.SetActive(true);
                currentBuilding.GetComponent<Building>().LevelUp();
            }

            Destroy(gameObject);
            return;
        }
        else if (occupied == false && levelingUp == false && over_UI == false)
        {
            dropSource.Play();

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

        Vector3 targetDir = planet_B.transform.position - currentGridSpace.transform.position;
        // Vector3 targetDir = planet_B.transform.position - gameObject.transform.position;

        targetDir = targetDir.normalized;

        Quaternion qDir = new Quaternion();
        qDir.SetLookRotation(Vector3.forward, (-1) * targetDir);
        gameObject.transform.rotation = qDir;
    }
}