using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    public float time;

    IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(time);
        Destroy(transform.parent.gameObject);
    }
}