using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStation : MonoBehaviour
{
    public List<GameObject> enteringPassengers = new List<GameObject>();

    public void AddEnteringPassenger(GameObject passenger)
    {
        GameObject parent = passenger.transform.parent.gameObject;

        enteringPassengers.Add(parent);
        parent.SetActive(false);
    }

}