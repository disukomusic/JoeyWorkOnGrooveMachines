using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaxorMeter : MonoBehaviour
{
    [SerializeField] private HackerTyper typer;

    private RectTransform _rectTransform;

    [SerializeField] private float minWidth = 0f;  // Minimum width for the meter
    [SerializeField] private float maxWidth = 256f; // Maximum width for the meter

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void UpdateHaxorMeterWidth()
    {
        if (_rectTransform == null || typer == null) return;

        float completion = typer.TypingCompletionPercentage;

        Vector2 size = _rectTransform.sizeDelta;
        size.x = Mathf.Lerp(minWidth, maxWidth, completion);
        _rectTransform.sizeDelta = size;
    }
}