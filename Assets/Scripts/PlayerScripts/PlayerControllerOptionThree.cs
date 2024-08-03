using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerOptionThree : MonoBehaviour
{
    public float maxSpeed;

    void Start()
    {
        maxSpeed = this.gameObject.GetComponent<EntityStats>().CalculateMaxSpeed();
    }

    void FixedUpdate()
    {
        
    }
}
