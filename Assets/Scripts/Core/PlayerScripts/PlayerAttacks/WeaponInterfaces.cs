using System.Collections;
using UnityEngine;

namespace Jili.StatSystem.AttackSystem
{
    public interface IShootable
    {

        void BecomeDirty();

        void CleanUp();

        public bool TryShoot(float deltatime);

        public IEnumerator AimProjectiles();

        public void Shoot(Vector2 direction);
    }
}