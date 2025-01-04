using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugSquasher : MonoBehaviour
{
    [SerializeField] private CameraSnapper cameraSnapper;
    [SerializeField] private GameObject camSnapObject;
    void Update()
    {
        if (Input.GetMouseButton(0) && cameraSnapper.currentCamera == camSnapObject)
        {
            CheckBugClick();
        }

    }

    private void CheckBugClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) 
        {
            if (hit.collider.CompareTag("Bug"))
            {
                Debug.DrawRay(ray.origin, ray.direction, Color.red, 1f, false);
                GameManager.instance.UpdateMoney(2f);
                hit.collider.GetComponent<BugBehavior>().SquashBug();
            }
        }
    }
}
