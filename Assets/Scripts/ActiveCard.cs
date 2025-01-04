using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ActiveCard : MonoBehaviour
{
    public Card activeCard;
    public static ActiveCard Instance;

    private void Awake()
    {
        Instance = this;
        SetActiveCardValues(activeCard.cardData);
    }

    void Update()
    {
        if (activeCard != null && ShikuSolitaire.Instance.camSnapper.currentCamera == ShikuSolitaire.Instance.camSnapObject && Input.GetKeyDown(KeyCode.Return))
        {
            activeCard.ActivateCard();
        }
    }

    public void SetActiveCardValues(CardData cardData)
    {
        activeCard.Initialize(cardData);
    }
}

