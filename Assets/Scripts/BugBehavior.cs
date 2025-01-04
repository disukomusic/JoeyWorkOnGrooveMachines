using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BugBehavior : MonoBehaviour
{
    public RectTransform canvasRect; 
    public RectTransform bugRect;    
    public float speed = 100f;        
    public float moveInterval = 1f;  
    public GameObject deathEffect;

    private Vector2 direction;
    private float moveTimer;
    private BugSpawner bugSpawner;  

    void Awake()
    {
        if (bugRect == null)
        {
            bugRect = GetComponent<RectTransform>();
        }

        if (canvasRect == null)
        {
            canvasRect = GetComponentInParent<RectTransform>();
        }

        bugSpawner = FindObjectOfType<BugSpawner>();
        if (bugSpawner.autoSquash)
        {
            StartCoroutine(SquashBugWithDelay());
        }
        ChangeDirection();
    }

    void Update()
    {
        bugRect.anchoredPosition += direction * speed * Time.deltaTime;

        ClampToCanvas();

        moveTimer += Time.deltaTime;
        if (moveTimer >= moveInterval)
        {
            ChangeDirection();
            moveTimer = 0f;
        }
    }

    void ChangeDirection()
    {
        direction = Random.insideUnitCircle.normalized;
    }

    void ClampToCanvas()
    {
        // Get the size of the canvas and bug
        Vector2 canvasSize = canvasRect.sizeDelta;
        Vector2 bugSize = bugRect.sizeDelta;

        // Clamp the bug's position to stay within the canvas bounds
        Vector2 position = bugRect.anchoredPosition;
        position.x = Mathf.Clamp(position.x, -canvasSize.x / 2 + bugSize.x / 2, canvasSize.x / 2 - bugSize.x / 2);
        position.y = Mathf.Clamp(position.y, -canvasSize.y / 2 + bugSize.y / 2, canvasSize.y / 2 - bugSize.y / 2);
        bugRect.anchoredPosition = position;
    }

    public void SquashBug()
    {
        Vector2 splatPosition = bugRect.anchoredPosition; 
        GameObject splat = Instantiate(deathEffect, canvasRect, false);
        RectTransform splatRect = splat.GetComponent<RectTransform>();
        
        splatRect.anchoredPosition = splatPosition;

        // Notify the BugSpawner to remove the bug from the active list
        if (bugSpawner != null)
        {
            bugSpawner.RemoveBug(gameObject);
        }

        // Destroy the bug
        Destroy(gameObject);
    }

    IEnumerator SquashBugWithDelay()
    {
        yield return new WaitForSeconds(1f);
        SquashBug();
    }


}
