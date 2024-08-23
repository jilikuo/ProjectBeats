using UnityEngine;
using TMPro;
using System;
using Jili.StatSystem;
using Attribute = Jili.StatSystem.Attribute;
using Jili.StatSystem.EntityTree;
using Jili.StatSystem.LevelSystem;

public class StatItemUI : MonoBehaviour
{
    public TextMeshProUGUI statNameText;
    public TextMeshProUGUI statValueText;

    private int statValue;
    private Color originalColor;
    private Color increaseColor = Color.green;
    private int temporaryAdd;
    private LevelUpMenu levelUpMenu;
    private int availablePoints;
    private Attribute attribute;
    private AttributeModifier modifier;
    private PlayerIdentity playerIdentity;
    private PlayerLevel levelSystem;

    private void Awake()
    {
        levelUpMenu = GameObject.Find("UI Manager").GetComponent<LevelUpMenu>();
        playerIdentity = GameObject.Find("Player").GetComponent<PlayerIdentity>();
        levelSystem = playerIdentity.GetComponent<PlayerLevel>();
        availablePoints = levelUpMenu.tempAttPoints;
    }

    private void Start()
    {
        if (statNameText == null)
        {
            throw new Exception("O Stat Item não localizou o campo 'statNameText', verifique o prefab");
        }

        if (statValueText == null)
        {
            throw new Exception("O Stat Item não localizou o campo 'statValueText', verifique o prefab");
        }
    }

    private void Update()
    {
        availablePoints = levelUpMenu.tempAttPoints;
        UpdateValueView();
    }

    public void SetStat(Attribute att)
    {
        if (att == null)
        {
            throw new ArgumentNullException("Attribute not found");
        }

        attribute = att;

        if (attribute == null)
        {
            throw new Exception("Attribute not found in playerIdentity.attList");
        }
        modifier = attribute.SetAndReadLevelModifier();
        originalColor = statValueText.color;
        statNameText.text = attribute.ReadAttName();
        statValue = attribute.ReadBaseValue();
        temporaryAdd = 0;
    }

    public void UpdateStatValue()
    {
        statValue = attribute.ReadBaseValue();
    }

    public void TempIncrease()
    {
        if (availablePoints > 0)
        {
            temporaryAdd++;
            levelUpMenu.TempUseSinglePoint();
        }
        UpdateValueView();
    }

    public bool ApplyChanges()
    {

        if (temporaryAdd > 0)
        {
            modifier.IncreaseModifier(temporaryAdd);
            attribute.ReloadLevelModifier(modifier);
            levelSystem.SpendAttPoints(temporaryAdd);
            return true;
        }
        if (temporaryAdd < 0)
        {

            // PLACEHOLDER FOR REDUCING ARGUMENT LOGIC
            throw new ArgumentOutOfRangeException("NÃO É POSSÍVEL DIMINUIR UM ATRIBUTO");
        }
        return false;
    }

    public void TempDecrease()
    {
        if (temporaryAdd > 0)
        {
            temporaryAdd--;
            levelUpMenu.TempUndoSinglePoint();
        }
        UpdateValueView();
    }

    public int CheckTempValue()
    {
        return temporaryAdd;
    }

    public void ResetTempValue()
    {
        temporaryAdd = 0;
    }

    public void UpdateValueView()
    {
        UpdateStatValue();
        if (temporaryAdd < 0)
        {
            statValueText.text = (statValue.ToString() + " (" + temporaryAdd + ")");
            statValueText.color = Color.red;
        }
        if (temporaryAdd == 0)
        {
            statValueText.text = statValue.ToString();
            statValueText.color = originalColor;
        }
        if (temporaryAdd >= 1)
        {
            statValueText.text = (statValue.ToString() + " (+" + temporaryAdd + ")");
            statValueText.color = increaseColor;
        }
    }
} 