using Jili.StatSystem.EntityTree;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//ESTE SCRIPT NAO DEVE SER ALEAT�RIO, ELE PRECISA PREVER O MOVIEMNTO DO JOGADOR
//PARA SPAWNAR INIMIGOS EM POSI��ES QUE ATRAPALHEM O JOGADOR SEMPRE QUE POSS�VEL.
// EM HIP�TESE ALGUMA DEVEMOS ADICIONAR FATOR DE ALEATORIEDADE. ESTE INIMIGO
//PRECISA SER PREVIS�VEL.
public class SpawnBigSuicidalEnemy : MonoBehaviour
{
    //entities
    public GameObject BigEnemyPrefab;

    //cached playerinfo
    public GameObject Player;
    public PlayerMovement PlayerMovementScript;
    public Transform PlayerPos;

    //properties
    public float SpawnInterval = 2f;
    public float SpawnDistance = 20f;
    public int EnemyMax = 20;

    //operating variables
    private Vector2 PredictedVector;
    private int EnemyCount;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerMovementScript = Player.GetComponent<PlayerMovement>();
        PlayerPos = Player.transform;

        InvokeRepeating("SpawnAtPredictedPos", SpawnInterval, SpawnInterval);
    }

    public Vector2 PredictPlayerMovement()
    {
        float maxSpeed = PlayerMovementScript.maxSpeed;
        float baseSpeed = PlayerMovementScript.baseSpeed;

        Vector2 currentSpeed = new Vector2(PlayerMovementScript.horizontalSpeed, PlayerMovementScript.verticalSpeed);
        if (currentSpeed == Vector2.zero)
        {
            // Se o jogador n�o estiver se movendo, gere uma posi��o aleat�ria ao redor do jogador
            return (Vector2)PlayerPos.position + Random.insideUnitCircle;
        }

        // Normaliza a velocidade atual em rela��o � velocidade m�xima
        float horizontalRate = (Mathf.Abs(PlayerMovementScript.horizontalSpeed) - baseSpeed) / (maxSpeed - baseSpeed);
        float verticalRate = (Mathf.Abs(PlayerMovementScript.verticalSpeed) - baseSpeed) / (maxSpeed - baseSpeed);

        // Garante que a propor��o esteja entre 0 e 1
        horizontalRate = Mathf.Clamp(horizontalRate, 0, 1);
        verticalRate = Mathf.Clamp(verticalRate, 0, 1);

        Vector2 normalizedSpeed = new Vector2(horizontalRate * Mathf.Sign(PlayerMovementScript.horizontalSpeed), verticalRate * Mathf.Sign(PlayerMovementScript.verticalSpeed));
        Vector2 predictedPosition = (Vector2)PlayerPos.position + (normalizedSpeed * maxSpeed);

        return predictedPosition;
    }

    public void SpawnAtPredictedPos()
    {
        int currentEnemyCount = CountEnemies();
        if (currentEnemyCount >= EnemyMax)
        {
            return;
        }

        Vector2 predictedPosition = PredictPlayerMovement();
        Vector2 direction = (predictedPosition - (Vector2)PlayerPos.position).normalized;
        Vector2 spawnPosition = (Vector2)PlayerPos.position + direction * SpawnDistance;

        GameObject newEnemy = Instantiate(BigEnemyPrefab, spawnPosition, Quaternion.identity);
    }

    private int CountEnemies()
    {
        SuicidalEnemyEntity[] allEnemies = FindObjectsOfType<SuicidalEnemyEntity>();
        return allEnemies.Count(enemy => enemy.type == SuicidalEnemyType.Big);
    }
}