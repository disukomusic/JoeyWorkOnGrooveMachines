using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraSnapper : MonoBehaviour
{
    public GameObject[] cameraPositions; 
    public GameObject[] monitorObjects;
    public GameObject currentCamera;
    public float transitionDuration = 1.0f; 
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    
    private GameObject _nextCamera;
    private Vector3 _targetPosition;
    private Quaternion _targetRotation;
    private Vector3 _startPosition;
    private Quaternion _startRotation;
    private float _transitionTimer = 0f;
    private bool _isTransitioning = false;

    private void Start()
    {
        StartTransition(cameraPositions[3]);
        cinemachineVirtualCamera.LookAt = monitorObjects[1].transform;
    }

    public void Update()
    {
        if (GameManager.instance.isGamePlaying)
        {
            
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                StartTransition(cameraPositions[0]);
                cinemachineVirtualCamera.LookAt = monitorObjects[0].transform;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                StartTransition(cameraPositions[1]);
                cinemachineVirtualCamera.LookAt = monitorObjects[1].transform;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                StartTransition(cameraPositions[2]);
                cinemachineVirtualCamera.LookAt = monitorObjects[2].transform;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                StartTransition(cameraPositions[3]);
                cinemachineVirtualCamera.LookAt = monitorObjects[1].transform;
            }

        }
        
        if (_isTransitioning)
        {
            _transitionTimer += Time.deltaTime;
            float t = Mathf.Clamp01(_transitionTimer / transitionDuration); 
            t = Mathf.SmoothStep(0f, 1f, t); 

            transform.position = Vector3.Lerp(_startPosition, _targetPosition, t);
            if (t >= 1f)
            {
                _isTransitioning = false;
            }
        }
    }

    private void StartTransition(GameObject target)
    {
        currentCamera = target;
        _startPosition = transform.position;
        _targetPosition = target.transform.position;

        _transitionTimer = 0f;
        _isTransitioning = true;
    }

    public void OnNightEnd()
    {
        cinemachineVirtualCamera.LookAt = monitorObjects[1].transform;
        StartTransition(cameraPositions[3]);
    }
}
