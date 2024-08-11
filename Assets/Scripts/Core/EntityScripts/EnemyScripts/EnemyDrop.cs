using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrop : MonoBehaviour
{
    public float expRateP;
    public float expRateM;
    public float expRateG;
    public float expRateS;
    public float expRateL;

    public GameObject expOrbP;
    public GameObject expOrbM;
    public GameObject expOrbG;
    public GameObject expOrbS;
    public GameObject expOrbL;
    private GameObject dropped;
    private void Start()
    {

    }

    public void CheckDrop()
    {
        if (Random.value <= expRateP)
        {
            float rangeX = ((Random.value * 0.10f) - (Random.value * 0.2f));
            float rangeY = ((Random.value * 0.10f) - (Random.value * 0.2f)); 
            Vector3 range = new(transform.position.x + rangeX, transform.position.y + rangeY, transform.position.z);
            dropped = Instantiate(expOrbP, range, Quaternion.identity);
        }

        if (Random.value <= expRateM)
        {
            float rangeX = ((Random.value * 0.10f) - (Random.value * 0.2f));
            float rangeY = ((Random.value * 0.10f) - (Random.value * 0.2f));
            Vector3 range = new(transform.position.x + rangeX, transform.position.y + rangeY, transform.position.z);
            dropped = Instantiate(expOrbM, range, Quaternion.identity);
        }

        if (Random.value <= expRateG)
        {
            float rangeX = ((Random.value * 0.10f) - (Random.value * 0.2f));
            float rangeY = ((Random.value * 0.10f) - (Random.value * 0.2f));
            Vector3 range = new(transform.position.x + rangeX, transform.position.y + rangeY, transform.position.z);
            dropped = Instantiate(expOrbG, range, Quaternion.identity);
        }

        if (Random.value <= expRateS)
        {
            float rangeX = ((Random.value * 0.10f) - (Random.value * 0.2f));
            float rangeY = ((Random.value * 0.10f) - (Random.value * 0.2f));
            Vector3 range = new(transform.position.x + rangeX, transform.position.y + rangeY, transform.position.z);
            dropped = Instantiate(expOrbS, range, Quaternion.identity);
        }

        if (Random.value <= expRateL)
        {
            float rangeX = ((Random.value * 0.10f) - (Random.value * 0.2f));
            float rangeY = ((Random.value * 0.10f) - (Random.value * 0.2f));
            Vector3 range = new(transform.position.x + rangeX, transform.position.y + rangeY, transform.position.z);
            dropped = Instantiate(expOrbL, range, Quaternion.identity);
        }
    }
}