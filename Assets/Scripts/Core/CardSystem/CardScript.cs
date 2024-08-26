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
        private TextMeshProUGUI cardRarityField;
        private TextMeshProUGUI cardLevelField;
        private Image cardFaceField;

        private void Awake()
        {
            TextMeshProUGUI[] textBoxes = this.GetComponentsInChildren<TextMeshProUGUI>(true);
            foreach (var box in textBoxes)
            {
                Debug.Log($"Checking TextMeshProUGUI with name: {box.name}");
                if (box.name == "NameBox")
                {
                    cardNameField = box.GetComponent<TextMeshProUGUI>();
                    break;
                }
            }

            if (cardNameField == null)
            {
                throw new System.Exception("NameBox not found, verify prefab/cardNameField finding logic");
            }

            cardNameField.text = cardData.cardName;
        }
    }
}