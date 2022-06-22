using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopPlanetRotation : MonoBehaviour
{
    private bool rotating;

    private GameObject planet_A;
    private GameObject planet_B;

    private void Start()
    {
        planet_A = GameObject.FindGameObjectWithTag("Planet_A");
        planet_B = GameObject.FindGameObjectWithTag("Planet_B");
    }

    private void OnMouseDown()
    {
        rotating = !rotating;
        planet_A.GetComponent<RotatePlanet>().rotating = rotating;
        planet_B.GetComponent<RotatePlanet>().rotating = rotating;
    }
}