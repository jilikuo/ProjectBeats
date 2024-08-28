using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Jili.StatSystem;
using Jili.StatSystem.EntityTree;
using Jili.StatSystem.LevelSystem;

// This script is responsible for updating the status bars of the UI.
// It is attached to the UI object that contains the status bars.
// It uses the PlayerIdentity script to get the player's health and mana.
// FOR FUTURE CONSIDERATION:
// Pode ser mais interessante, ao invés de no UPDATE, usar eventos e listeners
// para atualizar a UI;

public class StatusBars : MonoBehaviour
{
    public Slider hpbar;
    public Slider mpbar;
    public Slider stbar;
    public Slider expbar;
    private PlayerIdentity playerIdentity;
    private PlayerLevel levelSystem;
    private TextMeshProUGUI hplabel;
    private TextMeshProUGUI mplabel;
    private TextMeshProUGUI stlabel;
    private TextMeshProUGUI explabel;

    private void Start()
    {
        hplabel = hpbar.GetComponentInChildren<TextMeshProUGUI>();
        mplabel = mpbar.GetComponentInChildren<TextMeshProUGUI>();
        stlabel = stbar.GetComponentInChildren<TextMeshProUGUI>();
        explabel = expbar.GetComponentInChildren<TextMeshProUGUI>();

        playerIdentity = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<PlayerIdentity>();
        levelSystem = playerIdentity.GetComponent<PlayerLevel>();

        hpbar.maxValue = playerIdentity.Health.ReadValue();
        hpbar.value = playerIdentity.Health.ReadCurrentValue();
        mpbar.maxValue = playerIdentity.Mana.ReadValue();
        mpbar.value = playerIdentity.Mana.ReadCurrentValue();
        //stbar.maxValue = playerEntity.maxStamina;
        //stbar.value = playerEntity.stamina;
        //expbar.maxValue = playerEntity.nextLevelExp;
        //expbar.value = playerEntity.experience;

        
        // playerIdentity.Health.OnValueChanged += ForcedUpdate;
    }

    private void Update()
    {
        hpbar.maxValue = playerIdentity.Health.ReadValue();
        hpbar.value = playerIdentity.Health.ReadCurrentValue();
        hplabel.text = ((Mathf.Ceil(hpbar.value * 10) / 10).ToString() + " HP");

        mpbar.maxValue = playerIdentity.Mana.ReadValue();
        mpbar.value = playerIdentity.Mana.ReadCurrentValue();
        mplabel.text = ((Mathf.Ceil(mpbar.value * 10) / 10).ToString() + " MP");

        /*stbar.maxValue = playerEntity.maxStamina;
        stbar.value = playerEntity.stamina;
        stlabel.text = "Stamina";*/

        expbar.maxValue = levelSystem.ReadNextLevelExp();
        expbar.value = levelSystem.ReadExperience();
        if (expbar.value < 1)
        {
            expbar.fillRect.gameObject.SetActive(false);
        }
        else
        {
               expbar.fillRect.gameObject.SetActive(true);
        }
        explabel.text = (expbar.value.ToString() + " / " + expbar.maxValue.ToString() + " Experience Points");
    }

    /*
     * TODO:
     * this was used to fix a bug where picking up a card would not refresh the status bar
     * this should be called instead as a better way to update the status bar in the future:
     * instead of updating every frame, only update the ui when the stat changes or the player
     * takes damage.
    private void ForcedUpdate(Stat stat)
    {
        if (stat == playerIdentity.Health)
        {
            hpbar.maxValue = playerIdentity.Health.ReadValue();
            hpbar.value = playerIdentity.Health.ReadCurrentValue();
            hplabel.text = ((Mathf.Ceil(hpbar.value * 10) / 10).ToString() + " HP");
        }

        if (stat == playerIdentity.Mana)
        {
            mpbar.maxValue = playerIdentity.Mana.ReadValue();
            mpbar.value = playerIdentity.Mana.ReadCurrentValue();
            mplabel.text = ((Mathf.Ceil(mpbar.value * 10) / 10).ToString() + " MP");
        }

        return;
    }
    */
}
