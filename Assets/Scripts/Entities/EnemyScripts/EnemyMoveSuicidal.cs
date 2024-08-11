/* using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyMoveSuicidal : MonoBehaviour
{
    private GameObject target;
    private Transform playerPos;

    private Attribute stats;
    private float speed;
    private Rigidbody2D enemyRb;

    void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }

        stats = GetComponent<Attribute>();
        enemyRb = GetComponent<Rigidbody2D>();

        playerPos = target.transform;

        speed = (stats.CalculateMaxSpeed()) * 80/100;
    }

    void FixedUpdate()
    {
        Vector3 direction = (playerPos.position - transform.position).normalized;

        enemyRb.velocity = direction * speed;
    }
}
*/ 