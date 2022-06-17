using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpaceEdge : MonoBehaviour
{
    public List<Sprite> dandiSprites = new List<Sprite>();
    public List<Sprite> thornSprites = new List<Sprite>();

    private SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        int random = Random.Range(0, dandiSprites.Count);

        spriteRenderer.sprite = dandiSprites[random];
    }
}
