using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ecosystem : MonoBehaviour
{
    private float currentTemp;
    private float startTemp = 37.5f;

    private float currentGas;
    public int startGas;
    private int endGas = 1000;

    public List<GameObject> fields = new List<GameObject>();

    public Text gasAmountText;

    [Range(0, 1000)] public float test;

    void Start()
    {
        currentTemp = startTemp;
        currentGas = startGas;
    }

    public void AddGas(float amount)
    {
        currentGas += amount;
        currentGas = Mathf.Clamp(currentGas, (-1f) * Mathf.Infinity, endGas);

        currentTemp = startTemp + 0.0035f * currentGas;
        // Debug.Log(currentGas);
        if (gasAmountText != null) gasAmountText.text = currentGas.ToString();
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

}