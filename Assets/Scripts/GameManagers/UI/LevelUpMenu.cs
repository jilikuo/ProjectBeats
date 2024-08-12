using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.Collections.Generic;
public class LevelUpMenu : MonoBehaviour
{
    public GameObject levelUpMenu;
    public PlayerIdentity playerStats;
    public GameObject statItemPrefab;
    public Transform contentPanel;
    private bool isLeveling = false;
    private int heldAttPoints;
    public int tempAttPoints;
    private List<StatItemUI> statItems = new List<StatItemUI>();

    private void Awake()
    {
        heldAttPoints = 0;
        tempAttPoints = 0;
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
       // PopulateStats();
    }

    private void Update()
    {
        //CheckForAttributePoints();
        if ((Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.I)) && isLeveling == false)
        {
            ShowLevelUpMenu();
        }
        else if ((Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.I)) && isLeveling == true)
        {
            CloseLevelUpMenu();
        }
    }

     /* void CheckForAttributePoints()
    {
        if ((heldAttPoints + tempAttPoints) == statsData.freeAttPoints)
        {
            return;
        }
        if ((heldAttPoints + tempAttPoints) < statsData.freeAttPoints)
        {
            tempAttPoints = statsData.freeAttPoints - heldAttPoints;
        }
        if ((heldAttPoints + tempAttPoints) > statsData.freeAttPoints)
        {
            tempAttPoints = statsData.freeAttPoints - heldAttPoints;
        }
    } */

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

    public void UndoButton()
    {
        int tempReset = 0;
        foreach (var statItem in statItems)
        {
            tempReset += statItem.CheckTempValue();
            statItem.ResetTempValue();
            statItem.UpdateValueView();
        }

        heldAttPoints -= tempReset;
    }

    public void ConfirmButton()
    {
        int tempConsume = 0;
        foreach (var statItem in statItems)
        {
            //statsData.IncreaseAttByName((statItem.statNameText.text), statItem.CheckTempValue());
            tempConsume += statItem.CheckTempValue();
            statItem.ResetTempValue();
            statItem.UpdateValueView();
        }

        heldAttPoints -= tempConsume;
    }

    /* void PopulateStats()
    {
        for (int i = 0; i < statsData.attNames.Length; i++)
        {
            if (statsData.attNames[i] == "Level" || statsData.attNames[i] == "Gold")
            {
                continue;
            }

            GameObject newItem = Instantiate(statItemPrefab, contentPanel);
            StatItemUI itemUI = newItem.GetComponent<StatItemUI>();
            itemUI.SetStat(statsData.attNames[i], Mathf.FloorToInt(statsData.attValues[i]));
            statItems.Add(itemUI); // Add the created item to the list
        }
    } */

    public void TempUseSinglePoint()
    {
        tempAttPoints--;
        heldAttPoints++;
    }

    public void TempUndoSinglePoint()
    {
        tempAttPoints++;
        heldAttPoints--;
    }
}