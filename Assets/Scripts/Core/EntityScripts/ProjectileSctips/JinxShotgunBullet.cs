using Jili.StatSystem.AttackSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jili.StatSystem.EntityTree
{

    public class JinxShotgunBullet : ProjectileBase
    {
        private void Awake()
        {
            tankableHits = 1;
            strenght = 0.5f;
        }

        protected override void InitializeProjectile()
        {
            damage = source.ReturnStatValueByType(StatType.AttackDamage);
            range = source.ReturnStatValueByType(StatType.AttackRange);
        }
    }
}