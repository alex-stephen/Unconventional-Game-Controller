using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionTeleport : MonoBehaviour
{
    // Tag to identify the player
    public string playerTag = "Player";

    private void Start()
    {
        // Ensure the object with this script has a collider
        if (!GetComponent<Collider>())
        {
            Debug.LogWarning("No Collider attached to the object with OnCollisionTeleport. Add a Collider component.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
    
    if (collision.gameObject.name == "Winning Goblet") {
        
        // Continue teleport logic
        TeleportOnFall startScript = GetComponent<TeleportOnFall>();
        if (startScript != null)
        {
            Debug.Log("TeleportOnFall script found. Teleporting...");
            transform.position = startScript.startPosition;
            transform.rotation = Quaternion.Euler(startScript.resetRotation);

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    } else if (collision.gameObject.CompareTag(playerTag))
    {
        Debug.Log("Collision with player detected.");
        

        // Continue teleport logic
        TeleportOnFall teleportScript = GetComponent<TeleportOnFall>();
        if (teleportScript != null)
        {
            Debug.Log("TeleportOnFall script found. Teleporting...");
            transform.position = teleportScript.teleportPosition;
            transform.rotation = Quaternion.Euler(teleportScript.resetRotation);

            Rigidbody rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
        else
        {
            Debug.LogWarning("No TeleportOnFall script found.");
        }
    }
    }
}