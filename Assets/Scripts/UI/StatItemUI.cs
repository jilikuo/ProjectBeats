using UnityEngine;
using TMPro;

public class StatItemUI : MonoBehaviour
{
    public TextMeshProUGUI statNameText;
    public TextMeshProUGUI statValueText;
    private int statValue;

    public void SetStat(string name, int value)
    {
        statNameText.text = name;
        statValue = value;
        UpdateStatValueText();
    }

    public void IncreaseStat()
    {
        statValue++;
        UpdateStatValueText();
    }

    public void DecreaseStat()
    {
        if (statValue > 0)
        {
            statValue--;
            UpdateStatValueText();
        }
    }

    private void UpdateStatValueText()
    {
        statValueText.text = statValue.ToString();
    }
}