using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Card", order = 0)]
public class CardData : ScriptableObject
{
    public string cardName;
    public Sprite cardSprite;
}