using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionSuicidalEnemy : MonoBehaviour
{
    public GameObject target;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == target)
        {
            this.gameObject.SetActive(false);
        }
    }
}
