using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform playerPos;
    public float spawnInterval  = 2f;
    public float radius         = 20f;
    public float enemyMax       = 20f;
    public float enemyMin       = 0f;
    public float maxPerSpawn    = 10f;
    public float enemyCount     = 0f;

    void Start()
    {
        if (playerPos == null)
        {
            playerPos = GameObject.FindGameObjectWithTag("Player").gameObject.transform;
        }

        InvokeRepeating("SpawnCircle", 2f, spawnInterval); 
    }

    void FixedUpdate()
    {
        CountEnemies();
    }

    void SpawnCircle()
    {
        float willSpawn = Mathf.Min(enemyMax - enemyCount, maxPerSpawn);
        if (willSpawn < 1)
        {
            willSpawn = 1;
        }

        float firstPoint = Random.Range(0f, Mathf.PI  * 2);
        float step = (Mathf.PI * 2) / willSpawn;


        float spawned = 0;
        while (spawned < willSpawn)
        {
            float x = playerPos.position.x + Mathf.Cos(firstPoint + (step * spawned)) * radius;
            float y = playerPos.position.y + Mathf.Sin(firstPoint + (step * spawned)) * radius;


            Instantiate(enemyPrefab, new Vector3(x, y, 0f), Quaternion.identity);

            spawned += 1;
        }
    }
    
    void CountEnemies()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
}
