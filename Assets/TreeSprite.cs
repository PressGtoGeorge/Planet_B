using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSprite : MonoBehaviour
{
    public List<Sprite> sappling = new List<Sprite>();
    public List<Sprite> growing = new List<Sprite>();
    public List<Sprite> fullyGrown = new List<Sprite>();

    private List<SpriteRenderer> spriteRenderer = new List<SpriteRenderer>();

    // public int[] type = new int[3];
    public int nextType;

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.Add(transform.GetChild(i).GetComponent<SpriteRenderer>());
        }

        if (gameObject.GetComponent<Trees>().startsOnPlanet)
        {
            int random = Random.Range(0, fullyGrown.Count);
            spriteRenderer[0].sprite = fullyGrown[random];
        }
    }

}
