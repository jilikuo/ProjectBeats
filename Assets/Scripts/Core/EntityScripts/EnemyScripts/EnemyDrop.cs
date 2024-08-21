using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
    public int totalDropItems = 5;

    [SerializeField]
    private GameObject[] drop;
    [SerializeField]
    private float[] dropRate;
    [SerializeField]
    private float[] dropChance;

    private GameObject[] Drop;   // Objetos que podem dropar
    private float[] DropRate;    // Quando um objeto dropar, a chance de ser cada um desses objetos
    private float[] DropChance;  // Chance de dropar um objeto, se dropar, o próximo item da lista é a chance de dropar mais um.

    private GameObject dropped;
    private void Start()
    {
        Drop = new GameObject[totalDropItems];      
        DropRate = new float[totalDropItems];       
        DropChance = new float[totalDropItems];

        drop.CopyTo(Drop, 0);
        dropRate.CopyTo(DropRate, 0);
        dropChance.CopyTo(DropChance, 0);
    }

    public void StartDropRoutine()
    {
        int i;
        for (i = 0; i < Drop.Length; i++)
        {
            if (RollDice(DropChance[i]))
            {
                //Debug.Log("Rolagem bem-sucedida. Tentando dropar item.");
                DropOnce();
            }
            else
            {
                //Debug.Log("Rolagem falhou. Interrompendo o drop.");
                break;
            }
        }
    }

    public void DropOnce()
    {
        dropped = ChooseItemToDrop();
        Instantiate(dropped, transform.position, Quaternion.identity);
    }

    private bool RollDice(float chance)
    {
        float dice = Random.value * 100;
        if (dice <= chance)
        {
            return true;
        }
        return false;
    }

    private GameObject ChooseItemToDrop()
    {
        float totalWeight = 0;

        // Calcular a soma de todos os pesos
        for (int i = 0; i < Drop.Length; i++)
        {
            totalWeight += DropRate[i];
        }

        // Sortear um número aleatório entre 0 e a soma dos pesos
        float randomValue = Random.value * totalWeight;

        // Determinar qual item será selecionado
        float cumulativeWeight = 0f;
        for (int i = 0; i < DropRate.Length; i++)
        {
            cumulativeWeight += DropRate[i];
            if (randomValue <= cumulativeWeight)
            {
                return Drop[i];
            }
        }

        throw new MissingReferenceException("NÃO FOI POSSÍVEL ENCONTRAR UM ITEM VÁLIDO PARA DROPAR");
    }
}