using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class BugSpawner : MonoBehaviour
{
    public GameObject bugPrefab;
    public float spawnInterval = 1f;  // Time in seconds between spawns
    public float spawnRateIncrease = 0.1f;  // How much to reduce spawn interval over time
    [SerializeField] private TMP_Text bugCountText;
    [SerializeField] private GameObject bugPopup;
    [SerializeField] private HackerTyper _typer;

    public bool autoSquash = false;

    private float timer;
    private RectTransform canvasRect;
    private List<GameObject> activeBugs = new List<GameObject>();  // List to store active bugs
    private bool isPopupActive = false;  // Track if the popup is already active

    // Start is called before the first frame update
    void Start()
    {
        canvasRect = GetComponent<RectTransform>();
        timer = spawnInterval;  // Set initial timer based on spawnInterval
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && !_typer.IDECrashed && GameManager.instance.isGamePlaying)
        {
            SpawnBug();
            timer = spawnInterval;  // Reset the timer
            spawnInterval -= spawnRateIncrease;  // Decrease the spawn interval to spawn more bugs over time
            if (spawnInterval < 1f) spawnInterval = 1f;  // Limit the spawn rate to a minimum of 0.1 seconds
        }
    }

    public void SpawnBug()
    {
        // Randomize the position within the canvas
        float randomX = Random.Range(0, canvasRect.rect.width);
        float randomY = Random.Range(0, canvasRect.rect.height);
        Vector3 randomPosition = new Vector3(randomX, randomY, 0);

        // Instantiate the bug at the random position within the canvas
        GameObject bug = Instantiate(bugPrefab, randomPosition, Quaternion.identity);

        // Set the canvas for the bug's behavior
        BugBehavior bugBehavior = bug.GetComponent<BugBehavior>();
        bugBehavior.canvasRect = canvasRect;
        bugBehavior.bugRect = bug.GetComponent<RectTransform>();

        // Make the bug a child of the canvas
        bug.transform.SetParent(canvasRect, false);  // The second argument ensures local position is preserved

        // Add the bug to the active bugs list
        activeBugs.Add(bug);
        bugCountText.text = (activeBugs.Count).ToString();

        // Check for too many bugs
        if (GetBugCount() > 19)
        {
            if (!isPopupActive)  // Only trigger if the popup is not already active
            {
                bugPopup.SetActive(true);
                _typer.ResetActiveMethodProgress();
                isPopupActive = true;  // Set the flag to prevent repeated activation
            }
        }
        else
        {
            if (isPopupActive)  // Reset only when bug count drops below threshold
            {
                bugPopup.SetActive(false);
                isPopupActive = false;  // Reset the flag
            }
        }
    }

    // Method to remove a bug from the active list (called by BugBehavior when a bug is squashed)
    public void RemoveBug(GameObject bug)
    {
        activeBugs.Remove(bug);
        bugCountText.text = (activeBugs.Count).ToString();
        GameManager.instance.UpdateBugScore(1);
    }

    public int GetBugCount()
    {
        return (activeBugs.Count);
    }

    public void RemoveAllBugs()
    {
        for (int i = activeBugs.Count - 1; i >= 0; i--)
        {
            RemoveBug(activeBugs[i]);
        }
    }
}
