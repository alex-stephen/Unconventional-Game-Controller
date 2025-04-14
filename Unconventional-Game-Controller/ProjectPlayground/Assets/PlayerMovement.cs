using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Settings")]
    public float moveSpeed = 0f;      // Current move speed
    public float maxSpeed = 3f;      // Maximum speed (adjusted to slow down)
    public float speedScale = 0.5f;  // Scale factor for fine-tuning speed
    public float groundDrag;
    public float playerHeight;
    public LayerMask whatIsGround;
    public Transform orientation;
    public Transform playerModel;

    [Header("Rotation Smoothing")]
    public float rotationSmoothing = 0.2f; // Smoothing factor (smaller = slower rotation)

    [Header("Serial Communication")]
    public string portName = "COM3";
    public int baudRate = 9600;

    private Rigidbody rb;
    private SerialPort serialPort;

    private float targetRotationValue = 0f; // Target rotation value from Arduino (-180 to 180)
    private float currentRotationValue = 0f; // Smoothed rotation value
    private int speedValue = 0;              // Speed value received from Unity (0 to 6)

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        try
        {
            serialPort = new SerialPort(portName, baudRate);
            serialPort.ReadTimeout = 10;
            serialPort.Open();
            Debug.Log("Serial port opened successfully.");
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"Could not open serial port: {e.Message}");
        }
    }

    void Update()
    {
        ReadArduinoInput();
        SmoothRotation();
        RotatePlayer();
        HandleSpeed();
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void ReadArduinoInput()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                while (serialPort.BytesToRead > 0) // Avoid buffer backlog
                {
                    string data = serialPort.ReadLine();
                    string[] values = data.Split(','); // Expecting format: "rotation,speed"
                    if (values.Length == 2)
                    {
                        if (float.TryParse(values[0], out float parsedRotation))
                        {
                            targetRotationValue = Mathf.Clamp(parsedRotation, -180f, 180f);
                        }
                        if (int.TryParse(values[1], out int parsedSpeed))
                        {
                            speedValue = Mathf.Clamp(parsedSpeed, 0, 6); // Speed value must be between 0 and 6
                        }
                        Debug.Log($"Target Rotation: {targetRotationValue}, Speed Value: {speedValue}");
                    }
                }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Error reading serial data: {e.Message}");
            }
        }
    }

    void SmoothRotation()
    {
        // Gradually interpolate currentRotationValue towards targetRotationValue
        currentRotationValue = Mathf.Lerp(currentRotationValue, targetRotationValue, rotationSmoothing);
    }

    void RotatePlayer()
    {
        // Apply the smoothed rotation to the orientation transform
        orientation.rotation = Quaternion.Euler(0, currentRotationValue, 0);

        // Ensure the player model matches the orientation
        AlignPlayerModel();
    }

    void AlignPlayerModel()
    {
        // Align the player model's rotation with the orientation's rotation
        playerModel.rotation = orientation.rotation;
    }

    void HandleSpeed()
    {
        // Set moveSpeed based on the speed value and apply scaling
        moveSpeed = Mathf.Lerp(0f, maxSpeed, speedValue / 6f) * speedScale;
    }

    void MovePlayer()
    {
        if (moveSpeed > 0)
        {
            Vector3 movementDirection = orientation.forward;
            rb.AddForce(movementDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else
        {
            rb.velocity = new Vector3(0f, rb.velocity.y, 0f); // Stop horizontal movement if moveSpeed is 0
        }
    }

    private void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("Serial port closed.");
        }
    }
}