using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Jili.StatSystem.EntityTree;

public class StatusBars : MonoBehaviour
{
    public Slider hpbar;
    public Slider mpbar;
    public Slider stbar;
    public Slider expbar;
    private PlayerIdentity playerIdentity;
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
        hpbar.maxValue = playerIdentity.Health.ReadMaxValue();
        hpbar.value = playerIdentity.Health.ReadCurrentValue();
        //mpbar.maxValue = playerEntity.maxMana;
        //mpbar.value = playerEntity.Mana.Value;
        //stbar.maxValue = playerEntity.maxStamina;
        //stbar.value = playerEntity.stamina;
        //expbar.maxValue = playerEntity.nextLevelExp;
        //expbar.value = playerEntity.experience;
    }

    private void Update()
    {
        hpbar.maxValue = playerIdentity.Health.ReadMaxValue();
        hpbar.value = playerIdentity.Health.ReadCurrentValue();
        hplabel.text = ((Mathf.Ceil(hpbar.value * 10) / 10).ToString() + " HP");

        /*mpbar.maxValue = playerEntity.maxMana;
        mpbar.value = playerEntity.mana;
        mplabel.text = "Mana";

        stbar.maxValue = playerEntity.maxStamina;
        stbar.value = playerEntity.stamina;
        stlabel.text = "Stamina";

        expbar.maxValue = playerEntity.nextLevelExp;
        expbar.value = playerEntity.experience;
        explabel.text = ((Mathf.Floor(expbar.value).ToString()) + " / " + (Mathf.Floor(expbar.maxValue).ToString()) + " Experience Points");*/

    }
}
