using UnityEngine;
using System.Threading.Tasks;
public class LevelUpMenu : MonoBehaviour
{
    public GameObject levelUpMenu;
    public EntityStats statsData;
    public GameObject statItemPrefab;
    public Transform contentPanel;
    private bool isLeveling = false;

    private void Awake()
    {
        if (levelUpMenu != null)
        {
            levelUpMenu.SetActive(false);
        }
        if (levelUpMenu == null)
        {
            levelUpMenu = GameObject.Find("LevelUpMenu");
            levelUpMenu.SetActive(false);
        }
    }

    void Start()
    {
        PopulateStats();
    }

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.I)) && isLeveling == false)
        {
            ShowLevelUpMenu();
        }
        else if ((Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.I)) && isLeveling == true)
        {
            CloseLevelUpMenu();
        }
    }

    public async void ShowLevelUpMenu()
    {
        if (isLeveling)
            return;

        isLeveling = true;
        levelUpMenu.SetActive(true);

        // Pausa o tempo do jogo
        Time.timeScale = 0;

        // Aguarda até que o jogador feche o menu
        await WaitForLevelUpClose();

        // Retorna o tempo do jogo ao normal
        Time.timeScale = 1;
    }

    private async Task WaitForLevelUpClose()
    {
        // Aguarda enquanto a janela de level-up está ativa
        while (isLeveling)
        {
            await Task.Yield();
        }
    }

    public void CloseLevelUpMenu()
    {
        isLeveling = false;
        levelUpMenu.SetActive(false);
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