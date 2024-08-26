using Jili.StatSystem.CardSystem;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "New Card", menuName = "Card/New Card Data")]
public class ScriptableCardData : ScriptableObject
{
    private readonly string guiAtlasAddress = "Assets/Images/Sprites/GUI.png";

    public string cardName;
    public CardRarity cardRarity;
    public CardLevel cardLevel;
    public Sprite cardFace;

    public Sprite RaritySprite { get; private set; }

    private void OnEnable()
    {
        Addressables.LoadAssetAsync<Sprite[]>(guiAtlasAddress).Completed += OnSpriteLoaded;

    }

    private void OnSpriteLoaded(AsyncOperationHandle<Sprite[]> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Sprite[] guiTable = handle.Result;
            RaritySprite = GetRaritySprite(guiTable);
        }
        else
        {
            Debug.LogError("Failed to load sprite.");
        }
    }

    private Sprite GetRaritySprite(Sprite[] sprites)
    {
        string spriteName = GetSpriteNameBasedOnRarity();

        foreach (var sprite in sprites)
        {
            if (sprite.name == spriteName)
            {
                return sprite;
            }
        }

        throw new System.Exception("Rarity sprite not found for " + cardName + " card");
    }

    private string GetSpriteNameBasedOnRarity()
    {
        switch (cardRarity)
        {
            case CardRarity.Inferior:
                return "GUI_1";
            case CardRarity.Common:
                return "GUI_2";
            case CardRarity.Rare:
                return "GUI_3";
            case CardRarity.Legendary:
                return "GUI_4";
            case CardRarity.Mythic:
                return "GUI_5";
            case CardRarity.Godly:
                return "GUI_6";
            default:
                return "GUI_1";
        }
    }
}
