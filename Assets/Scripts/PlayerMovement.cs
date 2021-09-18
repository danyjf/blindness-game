using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float speed = 10f;
    public Transform groundChecker;
    public LayerMask groundLayer;
    public float gravity = -15f;
    public float jumpHeight = 3f;

    private CharacterController controller;
    private Vector3 velocity;
    private float groundRadius = 0.4f;
    private bool isGrounded;
    private float jumpForce;

    private void Start() {
        controller = transform.GetComponent<CharacterController>();
        jumpForce = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }
	
    private void Update() {
        isGrounded = Physics.CheckSphere(groundChecker.position, groundRadius, groundLayer);

        if(isGrounded && velocity.y < 0f)
            velocity.y = -2f;

        if(Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = jumpForce;

        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");

        Vector3 move = Vector3.Normalize(transform.right * xInput + transform.forward * zInput);
        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
