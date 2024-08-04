using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyMoveSuicidal : MonoBehaviour
{
    private GameObject target;
    private Transform playerPos;

    private EntityStats stats;
    private float speed;

    void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }

        stats = GetComponent<EntityStats>();

        playerPos = target.transform;

        speed = stats.CalculateMaxSpeed();
    }

    void FixedUpdate()
    {

        Vector3 direction = playerPos.position - transform.position;
        direction.Normalize();

        transform.position += direction * speed * Time.fixedDeltaTime;
    }
}
