using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables
    [SerializeField] private float pcSpeed, mobileSpeed;
    [SerializeField] private float rotationDuration;
    private float smoothVelocity;

    //Character Controller Component
    private CharacterController controller;
    private float speed = 5f;    

    //Input
    private PlayerInput playerInput;

    //Animator Component
    private Animator animator;
    #endregion


    private void Awake()
    {
        //Input
        playerInput = GetComponent<PlayerInput>();
        //Character Controller Component
        controller = GetComponent<CharacterController>();
        //Animator Component
        animator = GetComponentInChildren<Animator>();
        //Speed
        speed = playerInput.inputMode == PlayerInput.InputMode.Mobile ? mobileSpeed : pcSpeed;
    }

    private void FixedUpdate()
    {
        Vector3 moveDirection = playerInput.inputVector.normalized;

        animator.SetBool("walking", playerInput.inputVector.magnitude > 0);
        controller.Move(moveDirection * speed * Time.deltaTime);

        if (moveDirection != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVelocity, rotationDuration);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

}