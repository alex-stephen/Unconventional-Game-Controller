using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    // Movement speed
    public float moveSpeed = 5f;

    // Rotation speed
    public float rotationSpeed = 100f;

    // Orientation Transform
    public Transform orientation;

    // Model Transform
    public Transform model; // Add a reference to the model's Transform

    // Update is called once per frame
    void Update()
    {
        // Get input for forward/backward movement (W/S)
        float verticalInput = Input.GetAxis("Vertical");

        // Get input for rotation (A/D)
        float horizontalInput = Input.GetAxis("Horizontal");

        // Move the object forward and backward relative to orientation
        Vector3 forwardMovement = orientation.forward * verticalInput;
        transform.Translate(forwardMovement * moveSpeed * Time.deltaTime, Space.World);

        // Rotate the orientation transform
        orientation.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);

        // Align the model's rotation with the orientation
        AlignModelWithOrientation();
    }

    void AlignModelWithOrientation()
    {
        // Set the model's rotation to match the orientation's Y-axis rotation
        model.rotation = Quaternion.Euler(0, orientation.eulerAngles.y, 0);
    }
}