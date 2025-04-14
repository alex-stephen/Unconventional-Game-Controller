using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;

public class Flattener : MonoBehaviour
{
    // Distance to move along the X-axis
    public float moveDistance = 6f;

    // Speed for moving outward
    public float outwardSpeed = 10f;

    // Speed for returning to the original position
    public float returnSpeed = 5f;

    // Delay before movement starts
    public float startDelay = 2f;

    // Internal variables
    private Vector3 startPosition;
    private Vector3 targetPosition;

    // Global clock to synchronize movement
    private static float globalClock = 0f;
    private static bool clockInitialized = false;

    // Total duration for a complete outward and return cycle
    private float outwardDuration;
    private float returnDuration;
    private float cycleDuration;

    void Start()
    {
        // Initialize the global clock only once
        if (!clockInitialized)
        {
            globalClock = -startDelay;
            clockInitialized = true;
        }

        // Store the initial position
        startPosition = transform.position;

        // Calculate the target position (move left)
        targetPosition = new Vector3(startPosition.x + moveDistance, startPosition.y, startPosition.z);

        // Calculate durations for outward and return movements
        outwardDuration = moveDistance / outwardSpeed;
        returnDuration = moveDistance / returnSpeed;

        // Total duration for one complete cycle
        cycleDuration = outwardDuration + returnDuration;
    }

    void Update()
    {
        // Update the global clock
        globalClock += Time.deltaTime;

        // Wait for the start delay
        if (globalClock < 0f)
        {
            return;
        }

        // Calculate the phase of the movement based on the clock
        float elapsedTime = (globalClock - startDelay) % cycleDuration;

        if (elapsedTime < outwardDuration)
        {
            // Moving outward
            float progress = elapsedTime / outwardDuration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, progress);
        }
        else
        {
            // Returning to start
            float progress = (elapsedTime - outwardDuration) / returnDuration;
            transform.position = Vector3.Lerp(targetPosition, startPosition, progress);
        }
    }
}