using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamMover : MonoBehaviour
{
    // Distance to move on the X-axis
    public float moveDistance = 9f;

    // Speed of oscillation
    public float moveSpeed = 4f;

    // Internal variables
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool movingToTarget = true;

    void Start()
    {
        // Store the initial position
        startPosition = transform.position;

        // Calculate the target position (9 units to the negative X-axis)
        targetPosition = new Vector3(startPosition.x - moveDistance, startPosition.y, startPosition.z);
    }

    void Update()
    {
        // Determine the target position to move toward
        Vector3 destination = movingToTarget ? targetPosition : startPosition;

        // Move toward the destination
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);

        // Check if the beam has reached the destination
        if (Vector3.Distance(transform.position, destination) < 0.01f)
        {
            // Switch direction
            movingToTarget = !movingToTarget;
        }
    }
}
