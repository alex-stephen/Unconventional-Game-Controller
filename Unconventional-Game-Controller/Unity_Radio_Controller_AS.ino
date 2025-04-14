#include <Wire.h>

// Pin Definitions
const int OUT_PIN = A11;
const int IN_PIN = A13;
const int POT_PIN = A3;
const int DEBOUNCE_PIN = A5;

// Constants for Capacitance Measurement
const float IN_STRAY_CAP_TO_GND = 24.48;  // Stray capacitance to ground
const float IN_CAP_TO_GND = IN_STRAY_CAP_TO_GND;
const float R_PULLUP = 34.8;              // Pull-up resistor value
const int MAX_ADC_VALUE = 1023;           // Maximum ADC value

// Smoothing Parameters
const int WINDOW_SIZE = 30;               // Circular buffer size
int samples[WINDOW_SIZE] = {0};           // Circular buffer for smoothing
int currentIndex = 0;                     // Current buffer index
int total = 0;                            // Total of all buffer values
int average = 0;                          // Smoothed average

void setup()
{
    // Initialize Serial Communication
    Serial.begin(9600);

    // Configure Pins
    pinMode(OUT_PIN, OUTPUT);
    pinMode(IN_PIN, OUTPUT);
    pinMode(POT_PIN, INPUT);

    // Initialize the smoothing buffer
    for (int i = 0; i < WINDOW_SIZE; i++)
    {
        samples[i] = 0;
    }
}

void loop()
{
    // Stabilize Analog Reads
    analogRead(DEBOUNCE_PIN);
    analogRead(DEBOUNCE_PIN);

    // Measure Capacitance
    pinMode(IN_PIN, INPUT);            // Set IN_PIN as input
    digitalWrite(OUT_PIN, HIGH);       // Charge the capacitor
    int adcValue = analogRead(IN_PIN); // Read ADC value
    digitalWrite(OUT_PIN, LOW);        // Discharge the capacitor
    pinMode(IN_PIN, OUTPUT);           // Set IN_PIN back to output

    // Calculate Capacitance
    float capacitance = (float)adcValue * IN_CAP_TO_GND / (float)(MAX_ADC_VALUE - adcValue);
    delay(10); // Critical delay for proper measurement

    // Read Potentiometer Value
    int potValue = analogRead(POT_PIN);

    // Smooth the Capacitance Value
    total -= samples[currentIndex];          // Remove the oldest sample
    samples[currentIndex] = capacitance;     // Add the new sample
    total += samples[currentIndex];          // Update the total
    currentIndex = (currentIndex + 1) % WINDOW_SIZE; // Move to the next buffer index
    average = total / WINDOW_SIZE;           // Calculate the smoothed average

    // Map Values to Desired Ranges
    int potMappedValue = map(potValue, 0, MAX_ADC_VALUE, 0, 8);           // Potentiometer: 0-8
    int capacitanceMappedValue = map(average, 30, 110, -180, 180);       // Capacitance: -180 to 180


    // Send Values via Serial
    Serial.print(capacitanceMappedValue);
    Serial.print(",");
    Serial.println(potMappedValue);

    // Delay for Unity reading
    delay(50);
}