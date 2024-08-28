using Jili.StatSystem;
using Jili.StatSystem.AttackSystem;
using Jili.StatSystem.CardSystem;
using TMPro;
using UnityEditor;
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
    public CardCategory cardCategory;
    public StatType statType;
    public AttributeType attributeType;
    public float value;
    public Sprite cardFace;
    public MonoScript cardObject;

    public Sprite RaritySprite { get; private set; }
    public Sprite LevelSprite { get; private set;}
    public Color RarityColor { get; private set; }    
    public Color LevelColor { get; private set; }


    private void OnEnable()
    {
        // Load the GUI atlas and figure out the rarity sprite
        // also loads the level Star.
        Addressables.LoadAssetAsync<Sprite[]>(guiAtlasAddress).Completed += OnSpriteLoaded;

        // Set the rarity color based on the rarity
        // Should be temporary, once we have assets for each rarity,
        // we should use the sprite instead
        CalculateRarityColor();
        CalculateLevelColor();
    }

    private void OnSpriteLoaded(AsyncOperationHandle<Sprite[]> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Sprite[] guiTable = handle.Result;
            RaritySprite = GetRaritySprite(guiTable);
            LevelSprite = GetLevelStarSprite(guiTable);
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
                return "GUI_6";
            case CardRarity.Common:
                return "GUI_6";
            case CardRarity.Rare:
                return "GUI_5";
            case CardRarity.Legendary:
                return "GUI_5";
            case CardRarity.Mythic:
                return "GUI_4";
            case CardRarity.Godly:
                return "GUI_4";
            default:
                return "GUI_5";
        }
    }

    private void CalculateRarityColor()
    {
        switch (cardRarity)
        {
            case CardRarity.Inferior:
                RarityColor = new(0.4f, 0.4f, 0.4f, 1f);
                break;
            default:
                RarityColor = new(1f, 1f, 1f, 1f);
                break;
        }
    }

    private Sprite GetLevelStarSprite(Sprite[] sprites)
    {
        string spriteName = GetSpriteNameBasedOnLevel();

        foreach (var sprite in sprites)
        {
            if (sprite.name == spriteName)
            {
                return sprite;
            }
        }

        throw new System.Exception("Level star sprite not found for " + cardName + " card");
    }

    private string GetSpriteNameBasedOnLevel()
    {
        switch (cardLevel)
        {
            case CardLevel.Max:
                return "GUI_45";
            default:
                return "GUI_24";
        }
    }

    private void CalculateLevelColor()
    {
        switch (cardLevel)
        {
            case CardLevel.Max:
                LevelColor = new(0.5f, 0f, 0.6f, 1f);
                break;
            default:
                LevelColor = new(1f, 1f, 1f, 1f);
                break;
        }
    }
}
