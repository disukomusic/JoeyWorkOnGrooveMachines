using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlashColor : MonoBehaviour
{
    [SerializeField] private Image panelImage; // Reference to the panel's Image component.
    [SerializeField] private float flashDuration = 0.2f; // Duration of the flash in seconds.

    public Color color1;
    public Color color2;
    private Coroutine _flashCoroutine;

    private void Start()
    {
        FlashOpacityEffect();
    }

    public void FlashOpacityEffect()
    {
        if (panelImage == null)
        {
            Debug.LogError("Panel Image is not assigned!");
            return;
        }

        if (_flashCoroutine != null)
        {
            StopCoroutine(_flashCoroutine);
        }

        _flashCoroutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        panelImage.color = color2;
        yield return new WaitForSeconds(flashDuration / 2f);

        panelImage.color = color1;
        yield return new WaitForSeconds(flashDuration / 2f);
    }
}