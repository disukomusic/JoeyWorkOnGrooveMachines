using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ClockManager : MonoBehaviour
{
    public int hour; 
    public bool isAM; 
    public TMP_Text hourText;
    public UnityEvent onNightFinished;
    public UnityEvent onHourComplete;

    private bool showColon = true;

    void Start()
    {
        hour = 0; // Start at midnight
        isAM = true; 
        UpdateHourText();
        StartCoroutine(ClockCycle());
        StartCoroutine(BlinkColon());
    }

    IEnumerator ClockCycle()
    {
        while (hour < 8 || (hour == 8 && isAM))
        {
            yield return new WaitForSeconds(60f);
            IncrementHour();
            UpdateHourText();
        }

        onNightFinished.Invoke(); // Trigger event when night ends
    }

    IEnumerator BlinkColon()
    {
        while (true)
        {
            showColon = !showColon; // Toggle colon visibility
            UpdateHourText();
            yield return new WaitForSeconds(1f);
        }
    }

    void IncrementHour()
    {
        onHourComplete.Invoke();
        GameManager.instance.HourlyPayout(); // Transfer HaxorScore to Money
        hour++;
        if (hour > 12)
        {
            hour = 1; 
            isAM = !isAM; // Switch AM/PM at 12
        }
    }

    void UpdateHourText()
    {
        string amPm = isAM ? "am" : "pm";
        string colon = showColon ? ":" : " ";
        if (hour == 0 && isAM)
        {
            hourText.text = $"12{colon}00{amPm}";
        }
        else
        {
            hourText.text = $"{hour}{colon}00{amPm}";
        }
        
    }
}