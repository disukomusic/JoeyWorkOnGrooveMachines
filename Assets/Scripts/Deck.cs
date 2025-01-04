using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class Deck : MonoBehaviour
{
    public List<CardData> deck = new List<CardData>();
    private float shuffleTimerCurrent;
    private bool _isShuffling;
    public Card cardObject;

    public AudioSource shuffleSound;
    public AudioSource drawCardSound;
    public Animation cardAnimation;
    public GameObject shuffleOverlay;

    public void StartShuffle()
    {
        shuffleSound.Play();
        _isShuffling = true;
        shuffleOverlay.SetActive(true);
    }

    public void StopShuffle()
    {
        shuffleSound.Stop();
        _isShuffling = false;
        shuffleOverlay.SetActive(false);
    }

    public void DrawCard()
    {
        if (!_isShuffling && GameManager.instance.isGamePlaying && cardObject.cardUsedOverlay.activeInHierarchy)
        {
            FindObjectOfType<ShikuSolitaire>().ShuffleDeck();
            cardObject.cardUsedOverlay.SetActive(false);
            if (deck.Count > 0)
            {
                if(GameManager.instance.Money >= 50f)
                {
                    CardData drawnCard = deck[Random.Range(0, deck.Count)];
                    ActiveCard.Instance.SetActiveCardValues(drawnCard);
                    GameManager.instance.UpdateMoney(-50f);
                
                    deck.Remove(drawnCard);
                    Debug.Log($"Draw card: {drawnCard.cardName}");
                    drawCardSound.Play();
                }
                else
                {
                    Debug.Log("Not enough money...");
                }

            }
        }
        else
        {
            Debug.Log("Could not draw card");
        }
    }
}