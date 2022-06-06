using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawn : MonoBehaviour
{
    public GameObject prefab;

    private void OnMouseDown()
    {
        GameObject newTree = Instantiate(prefab, transform);
        newTree.transform.parent = null;
    }

}
