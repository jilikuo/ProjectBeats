/*using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusBars : MonoBehaviour
{
    public Slider hpbar;
    public Slider mpbar;
    public Slider stbar;
    public Slider expbar;
    private PlayerIdentity stats;
    private TextMeshProUGUI hplabel;
    private TextMeshProUGUI mplabel;
    private TextMeshProUGUI stlabel;
    private TextMeshProUGUI explabel;


    private void Start()
    {
        stats = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<EntityStats>();
        hpbar.maxValue = stats.maxHealth;
        hpbar.value = stats.health;
        mpbar.maxValue = stats.maxMana;
        mpbar.value = stats.mana;
        stbar.maxValue = stats.maxStamina;
        stbar.value = stats.stamina;
        expbar.maxValue = stats.nextLevelExp;
        expbar.value = stats.experience;

        hplabel = hpbar.GetComponentInChildren<TextMeshProUGUI>();
        mplabel = mpbar.GetComponentInChildren<TextMeshProUGUI>();
        stlabel = stbar.GetComponentInChildren<TextMeshProUGUI>();
        explabel = expbar.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        hpbar.maxValue = stats.maxHealth;
        hpbar.value = stats.health;
        hplabel.text = ((Mathf.Ceil(hpbar.value * 10) / 10).ToString() + " HP");

        mpbar.maxValue = stats.maxMana;
        mpbar.value = stats.mana;
        mplabel.text = "Mana";

        stbar.maxValue = stats.maxStamina;
        stbar.value = stats.stamina;
        stlabel.text = "Stamina";

        expbar.maxValue = stats.nextLevelExp;
        expbar.value = stats.experience;
        explabel.text = ((Mathf.Floor(expbar.value).ToString()) + " / " + (Mathf.Floor(expbar.maxValue).ToString()) + " Experience Points");

    }
}
*/