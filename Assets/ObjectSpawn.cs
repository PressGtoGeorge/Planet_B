using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawn : MonoBehaviour
{
    public GameObject prefab;

    private AudioSource dragSource;

    private void Start()
    {
        dragSource = transform.parent.GetComponent<AudioSource>();
    }

    private void OnMouseDown()
    {
        if (GameState.switching || GameState.switched || GameState.gameOver) return;

        GameObject newObject = Instantiate(prefab, transform);
        newObject.transform.parent = null;

        dragSource.Play();
    }
}