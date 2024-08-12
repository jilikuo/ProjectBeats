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
    private PlayerIdentity suicidalEntity;
    private TextMeshProUGUI hplabel;
    private TextMeshProUGUI mplabel;
    private TextMeshProUGUI stlabel;
    private TextMeshProUGUI explabel;


    private void Start()
    {
        suicidalEntity = GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Attribute>();
        hpbar.maxValue = suicidalEntity.maxHealth;
        hpbar.value = suicidalEntity.health;
        mpbar.maxValue = suicidalEntity.maxMana;
        mpbar.value = suicidalEntity.mana;
        stbar.maxValue = suicidalEntity.maxStamina;
        stbar.value = suicidalEntity.stamina;
        expbar.maxValue = suicidalEntity.nextLevelExp;
        expbar.value = suicidalEntity.experience;

        hplabel = hpbar.GetComponentInChildren<TextMeshProUGUI>();
        mplabel = mpbar.GetComponentInChildren<TextMeshProUGUI>();
        stlabel = stbar.GetComponentInChildren<TextMeshProUGUI>();
        explabel = expbar.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        hpbar.maxValue = suicidalEntity.maxHealth;
        hpbar.value = suicidalEntity.health;
        hplabel.text = ((Mathf.Ceil(hpbar.value * 10) / 10).ToString() + " HP");

        mpbar.maxValue = suicidalEntity.maxMana;
        mpbar.value = suicidalEntity.mana;
        mplabel.text = "Mana";

        stbar.maxValue = suicidalEntity.maxStamina;
        stbar.value = suicidalEntity.stamina;
        stlabel.text = "Stamina";

        expbar.maxValue = suicidalEntity.nextLevelExp;
        expbar.value = suicidalEntity.experience;
        explabel.text = ((Mathf.Floor(expbar.value).ToString()) + " / " + (Mathf.Floor(expbar.maxValue).ToString()) + " Experience Points");

    }
}
*/