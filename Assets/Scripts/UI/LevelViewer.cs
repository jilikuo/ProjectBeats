using TMPro;
using UnityEngine;

public class LevelViewer : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public EntityStats playerStats;
    public string labelName = "LevelLabel";
    public string playerTag = "Player";
    private string text = string.Empty;

    void Start()
    {
        if (levelText == null)
        {
            levelText = GameObject.Find(labelName).gameObject.GetComponent<TextMeshProUGUI>();
        }
        if (playerStats == null)
        {
            playerStats = GameObject.FindGameObjectWithTag(playerTag).gameObject.GetComponent<EntityStats>();
        }
        text = ("Level: " + playerStats.ReadLevel());
        levelText.text = text;
    }

    private void Update()
    {
        text = ("Level: " + playerStats.ReadLevel());
        levelText.text = text;
    }
}