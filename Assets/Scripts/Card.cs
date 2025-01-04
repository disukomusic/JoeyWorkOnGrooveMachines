using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Card : MonoBehaviour
{
    public CardData cardData;
    public Sprite cardBackSprite;
    public Sprite cardFrontSprite;
    public SpriteRenderer cardSpriteRenderer;
    public GameObject cardUsedOverlay;
    public AudioSource cardAlignmentSound;
    public AudioSource cardFlipSound;
    public AudioClip goodCardSound;
    public AudioClip badCardSound;
    public AudioClip crashSound;
    public Animation cardAnimation;
    public bool wasCardUsed;
    public bool usedOverlayAnimating;

    private Dictionary<string, Action> cardActions;

    private void Awake()
    {
        cardSpriteRenderer = GetComponent<SpriteRenderer>();
        wasCardUsed = false;
        InitializeCardActions();
    }

    private void InitializeCardActions()
    {
        cardActions = new Dictionary<string, Action>
        {
            { "BugSquasher", BugSquasherCard },
            { "BugSwarm", BugSwarmCard },
            { "TypeFaster1", () => TypeFasterCard(2) },
            { "TypeFaster2", () => TypeFasterCard(4) },
            { "TypeFaster3", () => TypeFasterCard(8) },
            { "TypeSlower1", () => TypeSlowerCard(2) },
            { "TypeSlower2", () => TypeSlowerCard(3) },
            { "TypeSlower3", () => TypeSlowerCard(4) },
            { "MultiplierX2", () => MultiplierCard(2) },
            { "MultiplierX3", () => MultiplierCard(3) },
            { "MultiplierX4", () => MultiplierCard(4) },
            { "MultiplierX8", () => MultiplierCard(8) },
            { "Score-200", () => ScoreCard(-200) },
            { "Score-300", () => ScoreCard(-300) },
            { "Score-400", () => ScoreCard(-400) },
            { "Score-800", () => ScoreCard(-800) },
            { "Score+200", () => ScoreCard(200) },
            { "Score+300", () => ScoreCard(300) },
            { "Score+400", () => ScoreCard(400) },
            { "Score+1K", () => ScoreCard(1000) },
            { "Score+5K", () => ScoreCard(5000) },
            { "Score+10K", () => ScoreCard(10000) },
            { "Money-10", () => MoneyCard(-10) },
            { "Money-15", () => MoneyCard(-15) },
            { "Money-20", () => MoneyCard(-20) },
            { "Money-30", () => MoneyCard(-30) },
            { "Money-50", () => MoneyCard(-50) },
            { "Money-100", () => MoneyCard(-100) },
            { "Money+20", () => MoneyCard(20) },
            { "Money+40", () => MoneyCard(40) },
            { "Money+60", () => MoneyCard(60) },
            { "Money+100", () => MoneyCard(100) },
            { "Money+200", () => MoneyCard(200) },
            { "IDECrash", IDECrashCard },
            { "HyperTyper", HyperTyper },
            { "AutoCode", AutoCode },
        };
    }

    public void Initialize(CardData newCardData)
    {
        wasCardUsed = false;
        HideUsedOverlay();
        cardData = newCardData;
        cardFrontSprite = newCardData.cardSprite;
        cardSpriteRenderer.sprite = cardBackSprite;
    }

    public void ActivateCard()
    {
        cardFlipSound.Play();
        if (!wasCardUsed)
        {
            cardAnimation.Rewind();
            cardAnimation.Play();
            cardSpriteRenderer.sprite = cardData.cardSprite;

            if (cardActions.ContainsKey(cardData.cardName))
            {
                cardActions[cardData.cardName].Invoke();
            }
            else
            {
                Debug.Log("Unknown card name.");
                ShowUsedOverlay();
            }

            wasCardUsed = true;
        }
        else
        {
            Debug.Log("Card was already activated.");
        }
    }

    public void ShowUsedOverlay() =>cardUsedOverlay.SetActive(true);

    public void HideUsedOverlay() => cardUsedOverlay.SetActive(false);

    // Card actions
    private void BugSquasherCard()
    {
        cardAlignmentSound.PlayOneShot(goodCardSound);
        StartCoroutine(BugSquasherRoutine());
    }

    private IEnumerator BugSquasherRoutine()
    {
        var bugSpawner = FindObjectOfType<BugSpawner>();
        bugSpawner.autoSquash = true;
        yield return new WaitForSeconds(20f);
        ShowUsedOverlay();
        bugSpawner.autoSquash = false;
    }

    private void BugSwarmCard()
    {
        var bugSpawner = FindObjectOfType<BugSpawner>();
        for (int i = 0; i < 10; i++)
        {
            bugSpawner.SpawnBug();
        }
        ShowUsedOverlay();
    }

    private void TypeFasterCard(int multiplier)
    {
        cardAlignmentSound.PlayOneShot(goodCardSound);
        StartCoroutine(TypeFasterCardRoutine(multiplier));
    }

    private IEnumerator TypeFasterCardRoutine(int multiplier)
    {
        var hackerTyper = FindObjectOfType<HackerTyper>();
        int oldChunkSize = hackerTyper.chunkSize;
        hackerTyper.chunkSize *= multiplier;
        yield return new WaitForSeconds(10f);
        ShowUsedOverlay();
        hackerTyper.chunkSize = oldChunkSize;
    }

    private void TypeSlowerCard(int multiplier)
    {
        cardAlignmentSound.PlayOneShot(badCardSound);
        StartCoroutine(TypeSlowerCardRoutine(multiplier));
    }

    private IEnumerator TypeSlowerCardRoutine(int multiplier)
    {
        var hackerTyper = FindObjectOfType<HackerTyper>();
        int oldChunkSize = hackerTyper.chunkSize;
        hackerTyper.chunkSize /= multiplier;
        yield return new WaitForSeconds(10f);
        ShowUsedOverlay();
        hackerTyper.chunkSize = oldChunkSize;
    }

    private void MultiplierCard(int multiplier)
    {
        cardAlignmentSound.PlayOneShot(goodCardSound);
        StartCoroutine(MultiplierCardRoutine(multiplier));
    }

    private IEnumerator MultiplierCardRoutine(int multiplier)
    {
        var hackerTyper = FindObjectOfType<HackerTyper>();
        int oldScoreMultiplier = hackerTyper.scoreMultipler;
        hackerTyper.scoreMultipler = multiplier;
        yield return new WaitForSeconds(20f);
        hackerTyper.scoreMultipler = oldScoreMultiplier;
        ShowUsedOverlay();
    }

    private void ScoreCard(int score)
    {
        cardAlignmentSound.PlayOneShot(score < 0 ? badCardSound : goodCardSound);
        GameManager.instance.UpdateHaxorScore(score);
        ShowUsedOverlay();
    }

    private void MoneyCard(float money)
    {
        cardAlignmentSound.PlayOneShot(money < 0 ? badCardSound : goodCardSound);
        GameManager.instance.UpdateMoney(money);
        ShowUsedOverlay();
    }

    private void IDECrashCard()
    {
        cardAlignmentSound.PlayOneShot(crashSound);
        StartCoroutine(IDECrashRoutine());
    }

    private IEnumerator IDECrashRoutine()
    {
        var hackerTyper = FindObjectOfType<HackerTyper>();
        var bugSpawner = FindObjectOfType<BugSpawner>();
        bugSpawner.RemoveAllBugs();
        yield return new WaitForSeconds(2f);
        hackerTyper.IDECrashed = true;
        hackerTyper.StartIDECrash();
        yield return new WaitForSeconds(Random.Range(10f, 20f));
        ShowUsedOverlay();
        hackerTyper.IDECrashed = false;
        hackerTyper.StopIDECrash();
    }

    void HyperTyper()
    {
        StartCoroutine(HyperTyperRoutine());
    }

    private IEnumerator HyperTyperRoutine()
    {
        var hackerTyper = FindObjectOfType<HackerTyper>();
        hackerTyper.isHyperTyping = true;
        yield return new WaitForSeconds(20f);
        ShowUsedOverlay();
        hackerTyper.isHyperTyping = false;
    }

    void AutoCode()
    {
        StartCoroutine(AutoCodeRoutine());
    }
    
    private IEnumerator AutoCodeRoutine()
    {
        var hackerTyper = FindObjectOfType<HackerTyper>();
        float t = 0;

        while (t < 20f)
        {
            hackerTyper.autoCodeInput = true;
            float firstWait = Random.Range(0.01f,0.05f);
            yield return new WaitForSeconds(firstWait);
            hackerTyper.autoCodeInput = false;
            float secondWait = Random.Range(0.05f, 0.25f);
            yield return new WaitForSeconds(secondWait);

            t += firstWait + secondWait; 
        }
        ShowUsedOverlay();
    }

}

