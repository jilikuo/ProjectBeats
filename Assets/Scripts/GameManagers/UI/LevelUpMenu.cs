using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.Collections.Generic;
using Jili.StatSystem;
using Jili.StatSystem.EntityTree;
using Jili.StatSystem.LevelSystem;

public class LevelUpMenu : MonoBehaviour
{
    //constants
    private readonly string playerTag = "Player";
    private readonly string lvlUpMenuTag = "LevelUpMenu";

    // ui objects
    public GameObject levelUpMenu;
    public GameObject statItemPrefab;
    public Transform contentPanel;

    // ui control variables
    private bool isShowing = false;
    private List<StatItemUI> statItems = new List<StatItemUI>();

    // player objects
    public PlayerIdentity playerIdentity;
    public PlayerLevel LevelSystem { get; private set; }

    // player level up variables
    private int heldAttPoints;
    public int tempAttPoints;

    private void Awake()
    {
        // procura pelo menu de level up e pelo jogador, aciona o menu de level up caso encontre
        if (levelUpMenu == null)
        {
            levelUpMenu = GameObject.Find(lvlUpMenuTag);
        }
        if (levelUpMenu != null)
        {
            levelUpMenu.GetComponent<Canvas>().enabled = true;
            levelUpMenu.SetActive(false);
        }
        else
        {
            throw new System.ArgumentNullException("LevelUpMenu not found");
        }

        if (playerIdentity == null)
        {
            playerIdentity = GameObject.FindGameObjectWithTag(playerTag).gameObject.GetComponent<PlayerIdentity>();
            
        }

        LevelSystem = playerIdentity.GetComponent<PlayerLevel>();
        heldAttPoints = 0;
        tempAttPoints = LevelSystem.ReadFreeAttPoints();
    }

    void Start()
    {
        PopulateStats();
    }

    private void Update()
    {
        CheckForAttributePoints();
        if ((Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.I)) && isShowing == false)
        {
            ShowLevelUpMenu();
        }
        else if ((Input.GetKeyDown(KeyCode.L) || Input.GetKeyDown(KeyCode.J) || Input.GetKeyDown(KeyCode.I)) && isShowing == true)
        {
            CloseLevelUpMenu();
        }
    }

   void CheckForAttributePoints()
   {
       if ((heldAttPoints + tempAttPoints) == LevelSystem.ReadFreeAttPoints())
       {
           return;
       }
       if ((heldAttPoints + tempAttPoints) < LevelSystem.ReadFreeAttPoints())
       {
           tempAttPoints = LevelSystem.ReadFreeAttPoints() - heldAttPoints;
       }
       if ((heldAttPoints + tempAttPoints) > LevelSystem.ReadFreeAttPoints())
       {
           tempAttPoints = LevelSystem.ReadFreeAttPoints() - heldAttPoints;
       }
   } 

    public async void ShowLevelUpMenu()
    {
        isShowing = true;
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
        while (isShowing)
        {
            await Task.Yield();
        }
    }

    public void CloseLevelUpMenu()
    {
        isShowing = false;
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
            if (statItem.ApplyChanges())
            {
                tempConsume += statItem.CheckTempValue();
                statItem.ResetTempValue();
                statItem.UpdateValueView();
            }
        }

        heldAttPoints -= tempConsume;
    }

    void PopulateStats()
    {
        foreach (Attribute att in playerIdentity.attList)
        {
            GameObject newItem = Instantiate(statItemPrefab, contentPanel);
            StatItemUI itemUI = newItem.GetComponent<StatItemUI>();
            itemUI.SetStat(att);
            statItems.Add(itemUI); // Add the created item to the list
        }
    }

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