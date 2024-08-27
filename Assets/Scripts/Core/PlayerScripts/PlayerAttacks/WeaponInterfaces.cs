using System;
using System.Collections;
using UnityEngine;

namespace Jili.StatSystem.AttackSystem
{
    public interface IShootable
    {

        void BecomeDirty(Stat stat);

        public void ReadDirtiness();

        public float ReturnStatValueByType(StatType type);
        
        public Type ReadClassType();

        public void IncreaseTier();

        public bool TryShoot(float deltatime);

        public IEnumerator AimProjectiles();

        public void Shoot(Vector2 direction);
    }
}