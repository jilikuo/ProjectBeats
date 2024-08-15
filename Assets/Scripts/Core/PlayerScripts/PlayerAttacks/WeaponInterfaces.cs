using System.Collections;
using UnityEngine;

namespace Jili.StatSystem.AttackSystem
{
    public interface IShootable
    {

        void BecomeDirty();

        void CleanUp();

        public virtual void CallCoolDownManager()
        {
            float deltatime = Time.fixedDeltaTime;
            if (CoolDownManager(deltatime))
            {
                AimProjectiles();
            }
        }

        public bool CoolDownManager(float deltatime);

        public IEnumerator AimProjectiles();

        public void Shoot(Vector2 direction);
    }
}