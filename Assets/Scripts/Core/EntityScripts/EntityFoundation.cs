using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Collections;
using Unity.Rendering;

public class EntityFoundation : MonoBehaviour
{
    [SerializeField] private Mesh mesh;
    [SerializeField] private Material material;
    protected internal EntityManager entityManager; 


    private void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        EntityArchetype entityArchetype = entityManager.CreateArchetype(
            typeof(EnemyComponents), 
            typeof(LocalTransform),
            typeof(RenderMeshUnmanaged),
            typeof(LocalToWorld)
            );

        NativeArray<Entity> entityArray = new NativeArray<Entity>(1, Allocator.Temp);
        entityManager.CreateEntity(entityArchetype, entityArray);

        foreach (Entity entity in entityArray)
        {
            entityManager.SetComponentData(entity, new EnemyComponents
            {
                attackDamage = Random.Range(1f, 20f),
                movementSpeed = Random.Range(0.1f, 10f),
                health = Random.Range(100f, 200f)
            });

            entityManager.SetComponentData(entity, new RenderMeshUnmanaged
            {
                mesh = mesh,
                materialForSubMesh = material
            });

        }

        entityArray.Dispose();
    }
}
