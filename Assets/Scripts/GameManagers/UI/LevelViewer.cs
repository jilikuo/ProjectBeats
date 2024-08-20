using TMPro;
using UnityEngine;
using Jili.StatSystem.LevelSystem;

public class LevelViewer : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public PlayerLevel levelSystem;
    public string labelName = "LevelLabel";
    public string playerTag = "Player";
    private string text = string.Empty;

    void Start()
    {
        if (levelText == null)
        {
            levelText = GameObject.Find(labelName).gameObject.GetComponent<TextMeshProUGUI>();
        }
        if (levelSystem == null)
        {
            levelSystem = GameObject.FindGameObjectWithTag(playerTag).gameObject.GetComponent<PlayerLevel>();
        }
        text = ("Level: " + levelSystem.ReadLevel());
        levelText.text = text;
    }

    private void Update()
    {
        text = ("Level: " + levelSystem.ReadLevel());
        levelText.text = text;
    }
}