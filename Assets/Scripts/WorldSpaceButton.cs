using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class WorldSpaceButton : MonoBehaviour
{
    [SerializeField] private CameraSnapper cameraSnapper;
    [SerializeField] private GameObject camSnapObject;
    [SerializeField] private string buttonTag;

    [SerializeField] private UnityEvent onButtonClick;
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && cameraSnapper.currentCamera == camSnapObject)
        {
            CheckButtonClick();
        }

    }

    private void CheckButtonClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) 
        {
            if (hit.collider.CompareTag(buttonTag))
            {
                Debug.DrawRay(ray.origin, ray.direction, Color.red, 1f, false);
                onButtonClick.Invoke();
            }
        }
    }
    
    
}
