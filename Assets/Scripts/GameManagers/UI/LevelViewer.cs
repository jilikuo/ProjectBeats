using TMPro;
using UnityEngine;
using Jili.StatSystem.LevelSystem;

public class LevelViewer : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    public PlayerLevel levelSystem;
    public string labelName = "LevelLabel";
    public string playerTag = "Player";
    public string levelUpWarning = "LevelUpWarning";
    private string text = string.Empty;

    private Transform levelUpWaningLabel;
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

        levelUpWaningLabel = levelText.transform.Find("LevelUpWarning");
        text = ("Level: " + levelSystem.ReadLevel());
        levelText.text = text;
    }

    private void Update()
    {
        text = ("Level: " + levelSystem.ReadLevel());
        levelText.text = text;
        if (levelSystem.ReadFreeAttPoints() > 0)
        {
            levelUpWaningLabel.gameObject.SetActive(true);
        }
        else
        {
            levelUpWaningLabel.gameObject.SetActive(false);
        }
    }
}