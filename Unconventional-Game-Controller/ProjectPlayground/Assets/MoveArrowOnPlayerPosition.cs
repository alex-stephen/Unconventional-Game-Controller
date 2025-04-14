using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveArrowOnPlayerPosition : MonoBehaviour
{
    // Reference to the player object (Capsule)
    public GameObject player;

    // Reference to the Arrow object
    public GameObject arrow;

    // Speed at which the Arrow moves
    public float moveSpeed = 5f;

    public float yThreshold = -5f;

    // Flag to control Arrow movement
    private bool shouldMoveArrow = false;

    // Arrow's starting position
    private Vector3 arrowStartPosition;

    void Start()
    {
        // Ensure arrow does not move at runtime
        shouldMoveArrow = false;

        // Validate references
        if (player == null || arrow == null)
        {
            Debug.LogError("Player or Arrow reference is missing. Please assign them in the Inspector.");
        }

        // Store the arrow's initial position
        arrowStartPosition = arrow.transform.position;
    }

    void Update()
    {
        // Validate references again
        if (player == null || arrow == null) return;

        // Explicitly check the player's Z position
        if (player.transform.localPosition.z > 7f)
        {
            shouldMoveArrow = true;
        }

        // Move the Arrow only if the condition is explicitly met
        if (shouldMoveArrow)
        {
            MoveArrow();
        }
        if (player.transform.position.y < yThreshold) {
            // Reset the arrow's position to its start position
            arrow.transform.position = arrowStartPosition;
            // Stop the arrow from moving
            shouldMoveArrow = false;
        }
    }

    void MoveArrow()
    {
        // Move the Arrow in the negative Z-axis
        arrow.transform.position += Vector3.back * moveSpeed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with the object named "ArrowHead"
        if (collision.gameObject.name == "ArrowHead" || 
            collision.gameObject.name == "Beam_R1 (1)")
        {
            Debug.Log("Collision with ArrowHead detected. Resetting arrow position.");
            
            // Reset the arrow's position to its start position
            arrow.transform.position = arrowStartPosition;

            // Stop the arrow from moving
            shouldMoveArrow = false;
        }
    }
}