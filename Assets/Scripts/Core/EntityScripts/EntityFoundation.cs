using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class EntityFoundation : MonoBehaviour
{
    protected internal EntityManager entityManager; 

    private void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
        Entity entity = entityManager.CreateEntity(typeof(EnemyComponents));

        entityManager.SetComponentData(entity, new EnemyComponents { attackDamage = 10, health = 125, movementSpeed = 3});
    }
}
