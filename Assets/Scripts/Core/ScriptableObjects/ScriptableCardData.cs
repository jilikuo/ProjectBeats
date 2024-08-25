using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/New Card Data")]
public class ScriptableCardData : ScriptableObject
{
    public string cardName;
    public int cardRarity;
    public int cardLevel;
    public Sprite cardFace;
}
