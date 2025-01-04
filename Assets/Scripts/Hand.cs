using UnityEngine;

public class Hand : MonoBehaviour
{
    public Card[] handCards = new Card[3]; // Array of 3 hand slots

    public void AddCardToHand(Card card)
    {
        for (int i = 0; i < handCards.Length; i++)
        {
            if (handCards[i] == null)
            {
                handCards[i] = card;
                return;
            }
        }
    }

    public void SwapCard(int slotIndex, Card card)
    {
        if (slotIndex >= 0 && slotIndex < handCards.Length)
        {
            handCards[slotIndex] = card;
        }
    }
}