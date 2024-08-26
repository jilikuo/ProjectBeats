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
            cardRarityField.sprite = cardData.RaritySprite;
        }
    }
}