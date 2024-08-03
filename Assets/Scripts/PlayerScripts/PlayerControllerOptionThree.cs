using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerOptionThree : MonoBehaviour
{
    public float maxSpeed;

    void Start()
    {
        maxSpeed = this.gameObject.GetComponent<PlayerStats>().CalculateMaxSpeed();
    }

    void FixedUpdate()
    {
        
    }
}
