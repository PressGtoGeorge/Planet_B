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
        growingIndicator.SetActive(true);
        gameObject.GetComponent<Building>().ecosystem.AddGas(12f);

        // yield return new WaitForSeconds(duration);

        while (timer < growthDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0f;

        if (gameObject.GetComponent<Building>().enabled == false)
        {
            gameObject.GetComponent<Building>().enabled = true;
            transform.localScale += Vector3.up * 0.3f; // placeholder
        }
        else
        {
            gameObject.GetComponent<Building>().LevelUp();
        }

        growing = false;
        growingIndicator.SetActive(false);

        yield break;
    }

    public void SpeedUpGrowth()
    {
        if (growing) timer += durationByWater;
    }

}