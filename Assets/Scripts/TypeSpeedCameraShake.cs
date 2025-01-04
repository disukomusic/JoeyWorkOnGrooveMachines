using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeSpeedCameraShake : MonoBehaviour
{
    public float shakeIntensity = 0.05f; // Reduced base shake intensity for subtle shake
    public float shakeDecay = 2.0f;     // Faster decay to make the shake end quicker
    public float shakeIncreaseSpeed = 0.1f; // Slower shake intensity increase for more control
    public float maxShakeIntensity = 0.3f; // Max shake intensity (lower than before)
    private Vector3 originalPosition;
    private float currentShakeIntensity = 0f;
    private float typingTimer = 0f;
    private bool isTyping = false;

    void Start()
    {
        originalPosition = transform.position; // Store the original camera position
    }

    void Update()
    {
        // Check if the player is typing (any key press)
        if (Input.anyKeyDown)
        {
            isTyping = true;
            typingTimer = 0f; // Reset the timer each time a key is pressed
            currentShakeIntensity = Mathf.Min(currentShakeIntensity + shakeIncreaseSpeed, maxShakeIntensity); // Increase shake intensity
        }
        else
        {
            typingTimer += Time.deltaTime;
        }

        if (isTyping)
        {
            // Apply subtle shake with lower intensity and randomness
            Vector3 shake = new Vector3(
                Random.Range(-currentShakeIntensity, currentShakeIntensity),
                Random.Range(-currentShakeIntensity, currentShakeIntensity),
                Random.Range(-currentShakeIntensity, currentShakeIntensity)
            );

            transform.position = originalPosition + shake; // Apply shake to the camera

            // Decay the shake intensity smoothly over time
            currentShakeIntensity = Mathf.Lerp(currentShakeIntensity, 0f, shakeDecay * Time.deltaTime);
        }
        else if (typingTimer > 0.2f) // Check if no keys have been pressed for a short time
        {
            // Smoothly return to the original position
            transform.position = Vector3.Lerp(transform.position, originalPosition, shakeDecay * Time.deltaTime);

            // If shake intensity is low enough, stop typing detection
            if (currentShakeIntensity < 0.01f)
            {
                isTyping = false;
            }
        }
    }
}
