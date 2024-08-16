using System.Collections;
using UnityEngine;

namespace Jili.StatSystem.AttackSystem
{
    public interface IShootable
    {

        void BecomeDirty(Stat stat);

        public void ReadDirtiness();

        public bool TryShoot(float deltatime);

        public IEnumerator AimProjectiles();

        public void Shoot(Vector2 direction);
    }
}