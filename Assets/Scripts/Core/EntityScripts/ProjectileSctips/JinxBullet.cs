using Jili.StatSystem.AttackSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jili.StatSystem.EntityTree
{

    public class JinxBullet : ProjectileBase
    {
        private void Awake()
        {
            tankableHits = 1;
            strenght = 1;
        }

        protected override void InitializeProjectile()
        {
            damage = source.ReturnStatValueByType(StatType.AttackDamage);
            range = source.ReturnStatValueByType(StatType.AttackRange);
        }
    }
}