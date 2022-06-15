using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterAnimation : MonoBehaviour
{
    private Animator animator;
    public Ecosystem ecosystem;
    public Rotate wheelRotation;

    private bool sweating;
    private bool stressed;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (sweating == false && ecosystem.GetCurrentGas() > 333)
        {
            sweating = true;
            animator.SetBool("sweating", true);
            wheelRotation.speed *= 2.5f;
        }
        else if (stressed == false && ecosystem.GetCurrentGas() > 666)
        {
            stressed = true;
            animator.SetBool("stressed", true);
            wheelRotation.speed *= 2.5f;
        }
    }
}