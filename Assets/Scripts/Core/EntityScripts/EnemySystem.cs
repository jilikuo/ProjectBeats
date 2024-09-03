using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public partial class EnemySystem : SystemBase
{
    protected override void OnUpdate()
    {
        Entities.ForEach((ref EnemyComponents enemy) =>
            {
                enemy.health -= enemy.attackDamage;
            }).Run();
    }
}
