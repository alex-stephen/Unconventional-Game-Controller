using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateModel : MonoBehaviour
{
    // Rotation speed
    public float rotationSpeed = 100f;

    void Update()
    {
        // Get horizontal input (A/D or Left/Right arrow keys)
        float horizontalInput = Input.GetAxis("Horizontal");

        // Rotate the model around the Y-axis
        transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
    }
}
