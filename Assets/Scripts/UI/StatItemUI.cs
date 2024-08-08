using UnityEngine;
using TMPro;

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
    private EntityStats playerStats;

    private void Start()
    {
        levelUpMenu = GameObject.Find("UI Manager").GetComponent<LevelUpMenu>();
        availablePoints = levelUpMenu.tempAttPoints;
        playerStats = GameObject.FindWithTag("Player").gameObject.GetComponent<EntityStats>();
    }

    private void Update()
    {
        availablePoints = levelUpMenu.tempAttPoints;
        UpdateValueView();
    }

    public void SetStat(string name, int value)
    {
        originalColor = statValueText.color;
        statNameText.text = name;
        statValue = value;
        temporaryAdd = 0;
    }

    public void UpdateStatValue()
    {
        statValue = playerStats.ReadAttByName(statNameText.text).Value;
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