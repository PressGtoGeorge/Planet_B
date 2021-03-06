using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ecosystem : MonoBehaviour
{
    private float currentTemp;
    private float startTemp = 37.5f;

    public float currentGas;
    public int startGas;
    private int endGas = 1000;

    public List<GameObject> fields = new List<GameObject>();

    public Text gasAmountText;

    [Range(0, 1000)] public float test;

    private Planet_A_Behaviour planet_A_Behaviour;
    private float planetProgress = 0;
    private float changeProgressAfter = 50;

    private RotatePlanet rotatePlanet;

    private Ecosystem planet_B_ecosystem;

    void Start()
    {
        currentTemp = startTemp;
        currentGas = startGas;

        if (gameObject.tag == "Planet_B") planet_A_Behaviour = GameObject.FindGameObjectWithTag("Planet_A").GetComponent<Planet_A_Behaviour>();

        rotatePlanet = gameObject.GetComponent<RotatePlanet>();

        planet_B_ecosystem = GameObject.FindGameObjectWithTag("Planet_B").GetComponent<Ecosystem>();
    }

    public void AddGas(float amount)
    {
        if (rotatePlanet.collapsed || rotatePlanet.collapsing) return;

        currentGas += amount;
        currentGas = Mathf.Clamp(currentGas, (-1f) * Mathf.Infinity, endGas);

        currentTemp = startTemp + 0.0035f * currentGas;
        // Debug.Log(currentGas);
        if (gasAmountText != null) gasAmountText.text = currentGas.ToString();

        planetProgress += amount;
        if (planet_A_Behaviour != null) CheckPlanetProgress();
    }

    private void LateUpdate()
    {
        if (gameObject.tag == "Planet_B") return;
        currentGas = 1000 - planet_B_ecosystem.GetCurrentGas();
    }

    private void Update0() // for testing, remove 0
    {
        currentGas = test;

        if (Input.GetKeyDown(KeyCode.W))
        {
            test += 30;
            AddGas(30);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            test -= 30;
            AddGas(-30);
        }
    }

    public float GetCurrentGas()
    {
        return currentGas;
    }

    private void CheckPlanetProgress()
    {
        if (planetProgress > changeProgressAfter)
        {
            planetProgress -= changeProgressAfter;
            planet_A_Behaviour.ProgressPlanet();
        }
    }

}