using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParentAfterAnimation : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            // Debug.Log("Boom");
            Destroy(transform.parent.gameObject);
        }
    }
}