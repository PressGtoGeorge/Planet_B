using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    private GameObject planet_A;
    private GameObject planet_B;

    private SpaceStation spaceStation_A;
    private SpaceStation spaceStation_B;

    public List<GameObject> enteringPassengers = new List<GameObject>();
    public List<GameObject> exitingPassengers = new List<GameObject>();

    private RocketAnimation animationScript;

    private void Start()
    {
        animationScript = gameObject.GetComponent<RocketAnimation>();

        planet_A = GameObject.FindGameObjectWithTag("Planet_A");
        planet_B = GameObject.FindGameObjectWithTag("Planet_B");

        spaceStation_A = planet_A.GetComponent<PlanetGrid>().gridSpaces[0].GetComponent<GridSpace>().building.GetComponent<SpaceStation>();
        spaceStation_B = planet_B.GetComponent<PlanetGrid>().gridSpaces[0].GetComponent<GridSpace>().building.GetComponent<SpaceStation>();

        StartTravelSchedule();
    }

    public void StartTravelSchedule()
    {
        StartCoroutine(TravelSchedule());
    }

    private IEnumerator TravelSchedule()
    {
        StartCoroutine(ReleaseExitingPassengers());

        yield return new WaitForSeconds(30);

        int waitingPassengers = spaceStation_A.enteringPassengers.Count + spaceStation_B.enteringPassengers.Count;

        if (waitingPassengers == 0 || exitingPassengers.Count > 0)
        {
            StartTravelSchedule();
        }
        else
        {
            AddEnteringPassengers();
            animationScript.StartAnimation();
        }
        yield break;
    }

    private IEnumerator ReleaseExitingPassengers()
    {
        GameObject currentPlanet;

        if (gameObject.GetComponent<RocketAnimation>().OnPlanet_A()) currentPlanet = planet_A;
        else currentPlanet = planet_B;

        foreach (GameObject passenger in enteringPassengers)
        {
            // currentSpaceStation.AddExitingPassenger(passenger);
            exitingPassengers.Add(passenger);
        }

        enteringPassengers.Clear();

        while (exitingPassengers.Count > 0)
        {
            if (currentPlanet.GetComponent<RotatePlanet>().collapsing || currentPlanet.GetComponent<RotatePlanet>().collapsed) yield break;

            exitingPassengers[0].SetActive(true);
            exitingPassengers[0].transform.parent = currentPlanet.transform;
            exitingPassengers[0].transform.localRotation = Quaternion.identity;
            exitingPassengers[0].transform.localPosition = Vector3.zero;

            exitingPassengers[0].transform.GetChild(0).GetComponent<Character>().UpdateCharacterAfterLanding();

            currentPlanet.GetComponent<Population>().characters.Add(exitingPassengers[0].transform.GetChild(0).gameObject);
            if (currentPlanet == planet_B)
            {
                // placeholder ?
                currentPlanet.GetComponent<Population>().UpdateCharacterCounter(exitingPassengers[0].transform.GetChild(0).GetComponent<Character>().tier, 1);
                currentPlanet.GetComponent<Population>().populationComingWithNextRocket--;
            }

            exitingPassengers.RemoveAt(0);

            yield return new WaitForSeconds(6f);
        }

        yield break;
    }

    private void AddEnteringPassengers()
    {
        SpaceStation currentSpaceStation;

        if (gameObject.GetComponent<RocketAnimation>().OnPlanet_A()) currentSpaceStation = spaceStation_A;
        else currentSpaceStation = spaceStation_B;
        
        foreach (GameObject passenger in currentSpaceStation.enteringPassengers)
        {
            enteringPassengers.Add(passenger);
        }

        currentSpaceStation.enteringPassengers.Clear();
    }

}