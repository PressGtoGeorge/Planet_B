using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public List<string> text = new List<string>();
    private static int index;
    private static List<int> seenTutorials;

    public GameObject setMarker;
    private static GameObject marker;

    void Start()
    {
        index = 0;
        seenTutorials = new List<int>();
        marker = setMarker;

        text.Add("Welcome the first citizens on Planet B!\nFulfil their wishes by placing the\ncorresponding building ahead of them!");
        text.Add("Mind CO2 emissions when goods are produced!\nBuild as less as you can and plant\ntrees to keep the climate balanced!");
        text.Add("Looks like you left one of your citizens unsatisfied!\nThey will travel back to Earth to find a better life\nthere! But don’t worry, other people from Earth will\ncome to Planet B as a replacement!");
        text.Add("Looks like you left one of your citizens unsatisfied!\nThey will seek for a similar product at the Black\nMarket to still their need! Looks like they are selling\nhamsters for all matters there – true climate killers!") ;
    }

    public static int GetIndex()
    {
        return index;
    }

    public static void SetIndex(int newValue)
    {
        if (seenTutorials.Contains(newValue) || Settings.stopTutorial) return;
        index = newValue;

        marker.SetActive(true);
    }

    public void UpdateSeenTutorials()
    {
        seenTutorials.Add(index);
    }

}