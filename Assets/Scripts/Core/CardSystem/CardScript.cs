using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Jili.StatSystem.CardSystem
{

    public class CardScript : MonoBehaviour
    {
        // data entry variables
        public ScriptableCardData cardData;

        // data display fields
        private TextMeshProUGUI cardNameField;
        private Transform cardLevelField;
        private Image cardRarityField;
        private Image cardFaceField;

        private void Awake()
        {
            cardNameField = transform.Find("NameBox/NameText")?.GetComponent<TextMeshProUGUI>();
            cardRarityField = transform.Find("NameBox/RarityMedal")?.GetComponent<Image>();

            if (cardNameField == null)
            {
                throw new System.Exception("TextMeshProUGUI component not found on NameText or invalid hierarchy.");
            }

            cardNameField.text = cardData.cardName;
            cardRarityField.sprite = GetRaritySprite();
        }

        private Sprite GetRaritySprite()
        {
            switch (cardData.cardRarity)
            {
                case CardRarity.Inferior:
                    return Resources.Load<Sprite>("Assets/Images/Sprites/GUI.png/GUI_1");
                case CardRarity.Common:
                    return Resources.Load<Sprite>("Sprites/Rarity/Common");
                case CardRarity.Rare:
                    return Resources.Load<Sprite>("Sprites/Rarity/Rare");
                case CardRarity.Legendary:
                    return Resources.Load<Sprite>("Sprites/Rarity/Legendary");
                case CardRarity.Mythic:
                    return Resources.Load<Sprite>("Sprites/Rarity/Mythic");
                case CardRarity.Godly:
                    return Resources.Load<Sprite>("Sprites/Rarity/Godly");
                default:
                    return Resources.Load<Sprite>("Sprites/Rarity/Inferior");
            }
        }
    }
}