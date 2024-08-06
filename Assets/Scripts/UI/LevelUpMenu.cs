using UnityEngine;
using UnityEngine.UI;

public class LevelUpMenu : MonoBehaviour
{
    public EntityStats statsData;
    public GameObject statItemPrefab;
    public Transform contentPanel;

    void Start()
    {
        PopulateStats();
    }

    void PopulateStats()
    {
        for (int i = 0; i < statsData.attNames.Length; i++)
        {
            GameObject newItem = Instantiate(statItemPrefab, contentPanel);
            StatItemUI itemUI = newItem.GetComponent<StatItemUI>();
            itemUI.SetStat(statsData.attNames[i], Mathf.FloorToInt(statsData.attValues[i]));
        }
    } 
}