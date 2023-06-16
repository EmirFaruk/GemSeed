using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;
    
    PlayerInput playerInput;
    Animator animator;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        transform.position += Vector3.forward * playerInput.inputVector.z * playerInput.speed * Time.deltaTime;
        transform.position += Vector3.right * playerInput.inputVector.x * playerInput.speed * Time.deltaTime;

        animator.SetBool("walking", playerInput.inputVector.magnitude > 0);        
    }


    [SerializeField] CameraBounds cameraBounds;
    [Serializable] struct CameraBounds
    {
        public float horizontalMin, horizontalMax, verticalMin, verticalMax;
    }

    /*void RunningControl()
    {
        if (playerInput.inputVector.magnitude > 0)
        {
            if (!animator.GetBool("running"))
            {
                this.Wait(1f, () =>
                {
                    if (playerInput.inputVector.magnitude > .6f)
                    {
                        //animator.SetBool("walking", false);
                        animator.SetBool("running", true);
                    }
                });
            }
            else
            {
                animator.SetBool("running", playerInput.inputVector.magnitude > .6f);
                animator.SetBool("walking", playerInput.inputVector.magnitude < .6f);
            }
        }        
    }*/
}
