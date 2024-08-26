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

        // level stars
        private Image starOne;
        private Image starTwo;
        private Image starThree;
        private Image starFour;
        private Image starFive;

        private void Awake()
        {
            cardNameField = transform.Find("NameBox/NameText")?.GetComponent<TextMeshProUGUI>();
            cardRarityField = transform.Find("NameBox/RarityMedal")?.GetComponent<Image>();
            cardLevelField = transform.Find("LevelBox");
            cardFaceField = transform.Find("Background/WeaponIcon")?.GetComponent<Image>();

            if (cardNameField == null)
            {
                throw new System.Exception("TextMeshProUGUI component not found on NameText or invalid hierarchy.");
            }

            cardNameField.text = cardData.cardName;
            cardRarityField.color = cardData.RarityColor;
            cardRarityField.sprite = cardData.RaritySprite;
            cardFaceField.sprite = cardData.cardFace;
            LoadCardLevel();
        }

        private void LoadCardLevel()
        {
            if ((int)cardData.cardLevel >= 1)
            {
                starOne = cardLevelField.Find("StarOne")?.GetComponent<Image>();
                starOne.color = cardData.LevelColor;
                starOne.sprite = cardData.LevelSprite;
            }
            if ((int)cardData.cardLevel >= 2)
            {
                starTwo = cardLevelField.Find("StarTwo")?.GetComponent<Image>();
                starTwo.color = cardData.LevelColor;
                starTwo.sprite = cardData.LevelSprite;
            }
            if ((int)cardData.cardLevel >= 3)
            {
                starThree = cardLevelField.Find("StarThree")?.GetComponent<Image>();
                starThree.color = cardData.LevelColor;
                starThree.sprite = cardData.LevelSprite;
            }
            if ((int)cardData.cardLevel >= 4)
            {
                starFour = cardLevelField.Find("StarFour")?.GetComponent<Image>();
                starFour.color = cardData.LevelColor;
                starFour.sprite = cardData.LevelSprite;
            }
            if ((int)cardData.cardLevel >= 5)
            {
                starFive = cardLevelField.Find("StarFive")?.GetComponent<Image>();
                starFive.color = cardData.LevelColor;
                starFive.sprite = cardData.LevelSprite;
            }
        }
    }
}