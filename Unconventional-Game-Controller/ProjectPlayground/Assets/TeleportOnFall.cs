using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportOnFall : MonoBehaviour
{
    // The thresholds for teleporting
    public float yThreshold = -5f;
    public float zThreshold = 100f;

    // The position to teleport the object to
    public Vector3 teleportPosition = new Vector3(2f, 1f, 0f);
    public Vector3 startPosition = new Vector3(2f, 1f, 0f);
    public Vector3 resetRotation = Vector3.zero;

    private Rigidbody rb;

    void Start()
    {
        // Get the Rigidbody component
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the object's position exceeds the thresholds
        if (transform.position.y < yThreshold)
        {
            Teleport();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the object collided with "Level 2 Floor"
        if (collision.gameObject.name == "Level 2 Floor")
        {
            SaveNewResetPosition();
        }

        if (collision.gameObject.name == "Winning Goblet")
        {
            SaveStartPosition();
        }

        // Check if the collided object has OnCollisionTeleport
        OnCollisionTeleport teleportObject = collision.gameObject.GetComponent<OnCollisionTeleport>();

        if (teleportObject != null)
        {
            Debug.Log($"Collision with teleport object: {collision.gameObject.name}. Teleporting...");
            Teleport();
        }
    }

    private void Teleport()
    {
        // Teleport the object to the specified position
        transform.position = teleportPosition;

        // Reset the rotation to the specified value
        transform.rotation = Quaternion.Euler(resetRotation);

        // Reset the velocity if a Rigidbody is attached
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void SaveNewResetPosition()
    {
        // Save the current position as the new teleport position
        teleportPosition = transform.position;

        // Log the new teleport position
        Debug.Log($"New teleport position saved: {teleportPosition}");
    }

    private void SaveStartPosition()
    {
        // Save the current position as the new teleport position
        teleportPosition = startPosition;

    }
}