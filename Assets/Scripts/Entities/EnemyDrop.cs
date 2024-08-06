using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
    public float dropRate;
    public GameObject expOrb;
    private GameObject dropped;
    private void Start()
    {
        dropRate = 0.25f;
    }

    public void CheckDrop()
    {
        if (Random.value <= dropRate)
        {
            dropped = Instantiate(expOrb, transform.position, Quaternion.identity);
        }
    }
}
