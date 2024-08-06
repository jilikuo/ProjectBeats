using TMPro;
using UnityEngine;

public class GameVersion : MonoBehaviour
{
    public TextMeshProUGUI versionText;

    void Start()
    {
        if (versionText == null)
        {
            versionText = GameObject.Find("GameVersion").gameObject.GetComponent<TextMeshProUGUI>();
        }
        string version = Application.version;

        versionText.text = "Version: " + version;
    }
}