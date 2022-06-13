using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ecosystem : MonoBehaviour
{
    private float currentTemp;
    private float startTemp = 37.5f;

    private float currentGas;
    private int startGas = 0;
    private int endGas = 1000;

    public List<GameObject> fields = new List<GameObject>();

    public Text gasAmountText;

    void Start()
    {
        currentTemp = startTemp;
    }

    public void AddGas(float amount)
    {
        currentGas += amount;
        // currentGas = (int)Mathf.Clamp(currentGas, startGas, endGas);

        currentTemp = startTemp + 0.0035f * currentGas;
        // Debug.Log(currentGas);
        if (gasAmountText != null) gasAmountText.text = currentGas.ToString();
    }

    public float GetCurrentGas()
    {
        return currentGas;
    }

}