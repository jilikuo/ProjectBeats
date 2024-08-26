using Jili.StatSystem.CardSystem;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/New Card Data")]
public class ScriptableCardData : ScriptableObject
{
    public string cardName;
    public CardRarity cardRarity;
    public CardLevel cardLevel;
    public Sprite cardFace;
}
