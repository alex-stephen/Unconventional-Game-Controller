using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamMoverPositive : MonoBehaviour
{
    // Distance to move on the X-axis
    public float moveDistance = 2f;

    // Speed of oscillation
    public float moveSpeed = 2f;

    // Internal variables
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool movingToTarget = true;

    void Start()
    {
        // Store the initial position
        startPosition = transform.position;

        // Calculate the target position (9 units to the positive X-axis)
        targetPosition = new Vector3(startPosition.x + moveDistance, startPosition.y, startPosition.z);
    }

    void Update()
    {
        // Determine the destination
        Vector3 destination = movingToTarget ? targetPosition : startPosition;

        // Move the beam toward the destination
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

        // Check if the beam has reached the destination
        if (Vector3.Distance(transform.position, destination) < 0.01f)
        {
            // Switch direction
            movingToTarget = !movingToTarget;
        }
    }
}
