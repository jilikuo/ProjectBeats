using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public struct EnemyComponents : IComponentData
{
    public float attackDamage;
    public float movementSpeed;
    public float health;

}
