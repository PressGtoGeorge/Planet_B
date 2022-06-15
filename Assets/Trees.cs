using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trees : MonoBehaviour
{
    private int growthDuration = 240;
    private float timer = 0f;

    public bool growing;
    public GameObject growingIndicator;

    public bool startsOnPlanet;

    private float durationByWater = 30f;

    private void Start()
    {
        if (startsOnPlanet == false) StartGrowth();
    }

    public void StartGrowth()
    {
        StartCoroutine(GrowTree());
    }

    private IEnumerator GrowTree()
    {
        growing = true;
        // growingIndicator.SetActive(true); // placeholder
        gameObject.GetComponent<Building>().ecosystem.AddGas(4f);

        int level = gameObject.GetComponent<Building>().level + 1;
        int type = gameObject.GetComponent<TreeSprite>().nextType;

        GameObject newTree = transform.GetChild(level).gameObject;
        newTree.SetActive(true);

        Sprite sapplingSprite = gameObject.GetComponent<TreeSprite>().sappling[type / 2];
        newTree.GetComponent<SpriteRenderer>().sprite = sapplingSprite;

        // yield return new WaitForSeconds(duration);

        float halfTime = growthDuration / 2f;

        while (timer < growthDuration)
        {
            timer += Time.deltaTime;
            if (timer >= halfTime) 
            {
                newTree.GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<TreeSprite>().growing[type];
                halfTime = 0;
            }
            yield return null;
        }

        timer = 0f;

        newTree.GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<TreeSprite>().fullyGrown[type];

        if (gameObject.GetComponent<Building>().enabled == false)
        {
            gameObject.GetComponent<Building>().level++; // drag-and-drop-trees start on level -1
            gameObject.GetComponent<Building>().enabled = true;
            // transform.localScale += Vector3.up * 0.3f; // placeholder
        }
        else
        {
            gameObject.GetComponent<Building>().LevelUp();
        }

        growing = false;
        // growingIndicator.SetActive(false); // placeholder

        yield break;
    }

    public void SpeedUpGrowth()
    {
        if (growing) timer += durationByWater;
    }

}