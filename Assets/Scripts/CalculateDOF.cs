using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CalculateDOF : MonoBehaviour
{
    [Header("Settings")]
    public LayerMask focusLayerMask = ~0; // Default to all layers
    public float maxFocusDistance = 50f; // Maximum raycast distance
    
    private Volume globalVolume;
    private DepthOfField depthOfField;
    private Camera mainCamera;

    void Start()
    {
        // Get the main camera
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera not found!");
            return;
        }

        // Find the global volume in the scene
        Volume[] volumes = FindObjectsOfType<Volume>();
        foreach (var volume in volumes)
        {
            if (volume.isGlobal)
            {
                globalVolume = volume;
                break;
            }
        }

        if (globalVolume == null)
        {
            Debug.LogError("Global Volume not found in the scene!");
            return;
        }

        // Try to get the Depth of Field component from the global volume
        if (!globalVolume.profile.TryGet(out depthOfField))
        {
            Debug.LogError("Depth of Field component not found in the Global Volume Profile!");
        }
    }

    void Update()
    {
        if (depthOfField == null || mainCamera == null) return;

        // Perform a raycast to determine the distance to the object in front
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxFocusDistance, focusLayerMask))
        {
            // Update the focal distance to the distance of the object hit
            depthOfField.focusDistance.value = hit.distance;
        }
        else
        {
            // Set to maximum distance if no object is hit
            depthOfField.focusDistance.value = maxFocusDistance;
        }
    }
}