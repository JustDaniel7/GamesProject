using System;
using static UnityEngine.Quaternion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour

{
    private CharacterController characterController;
    private Animator animator;
    public float jumpSpeed;

    [SerializeField] private float ySpeed;
    [SerializeField] private float forwardMoveSpeed = 7;
    [SerializeField] private float backwardMoveSpeed = 3;
    [SerializeField] private float turnSpeed = 5;
    private float originalStepOffset;
    
    private void Awake() {
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
        animator = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        
        var movement = new Vector3(horizontal,0,vertical);
        float magnitude = Mathf.Clamp01(movement.magnitude) * forwardMoveSpeed;

        Vector3 velocity = movement * magnitude;
        velocity.y = ySpeed;

        characterController.Move(velocity * Time.deltaTime);
        animator.SetFloat("speed",vertical);

        transform.Rotate(Vector3.up, horizontal * turnSpeed * Time.deltaTime);

        if (vertical != 0)
        {
            float moveSpeedToUse = vertical > 0 ? forwardMoveSpeed : backwardMoveSpeed;
            characterController.SimpleMove(transform.forward * moveSpeedToUse * vertical);
        }

        ySpeed += Physics.gravity.y * Time.deltaTime;
        if (characterController.isGrounded)
        {
            characterController.stepOffset = originalStepOffset;
            ySpeed = -0.5f;
            if(Input.GetButtonDown("Jump")) ySpeed = jumpSpeed;
        }
        else {
            characterController.stepOffset = 0;
        }
        
        
        if (movement.magnitude > 0) {
            Quaternion newDirection = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, newDirection, Time.deltaTime * turnSpeed);
        }
        
    }
}
