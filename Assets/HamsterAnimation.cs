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

    private float wheelOriginalSpeed;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        wheelOriginalSpeed = wheelRotation.speed;
    }

    private void Update()
    {
        if ((sweating || stressed) && ecosystem.GetCurrentGas() <= 333)
        {
            sweating = false;
            stressed = false;

            animator.SetBool("sweating", false);
            animator.SetBool("stressed", false);

            wheelRotation.speed = wheelOriginalSpeed;
        }
        else if ((sweating == false) && ecosystem.GetCurrentGas() > 333 && ecosystem.GetCurrentGas() <= 666)
        {
            sweating = true;
            stressed = false;

            animator.SetBool("sweating", true);
            animator.SetBool("stressed", false);

            wheelRotation.speed = wheelOriginalSpeed * 2.5f;
        }
        else if (stressed == false && ecosystem.GetCurrentGas() > 666)
        {
            sweating = false;
            stressed = true;

            animator.SetBool("sweating", false);
            animator.SetBool("stressed", true);

            wheelRotation.speed = wheelOriginalSpeed * 2.5f * 2.5f;
        }
    }
}