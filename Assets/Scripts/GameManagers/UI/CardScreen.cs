using Jili.StatSystem.EntityTree.ConsumableSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardScreen : MonoBehaviour
{
    public GameObject CardSelectionScreen;
    public GameObject CardTemplate;

    private void Awake()
    {
        CardSelectionScreen.SetActive(false);
        LoadCard();
    }

    void OnEnable()
    {
        ConsumableUse.OpenCardPacket += HandleOpenCardPacket;
    }

    private void OnDestroy()
    {
        ConsumableUse.OpenCardPacket -= HandleOpenCardPacket;
    }

    private void HandleOpenCardPacket(int amount)
    {
        CardSelectionScreen.SetActive(true);


    }

    private void LoadCard()
    {
        GameObject newItem = Instantiate(CardTemplate, contentPanel);
        StatItemUI itemUI = newItem.GetComponent<StatItemUI>();
    }
}
