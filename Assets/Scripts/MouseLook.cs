using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour {
    public Transform playerBody;
    public float speed = 300f;

    private float rotateX = 0f;

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }
	
    private void Update() {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotateX -= mouseY * speed * Time.deltaTime;
        rotateX = Mathf.Clamp(rotateX, -90f, 90);

        transform.localRotation = Quaternion.Euler(rotateX, 0f, 0f);
        playerBody.Rotate(Vector3.up, mouseX * speed * Time.deltaTime);
    }
}
