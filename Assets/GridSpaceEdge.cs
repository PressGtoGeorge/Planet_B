using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpaceEdge : MonoBehaviour
{
    public List<Sprite> sprites = new List<Sprite>();
    private SpriteRenderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        renderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        int random = Random.Range(0, sprites.Count);

        renderer.sprite = sprites[random];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
