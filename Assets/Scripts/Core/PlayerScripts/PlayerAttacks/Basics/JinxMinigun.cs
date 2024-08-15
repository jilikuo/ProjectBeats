using System;
using System.Collections;
using UnityEngine;

namespace Jili.StatSystem.AttackSystem
{
    public class JinxMinigun : IShootable
    {
        protected WeaponTypes Type = WeaponTypes.JinxMinigun;
        protected float damage;
        protected float cooldown;
        protected float range;
        protected float speed;
        protected Boolean isDirty = true;


        JinxMinigun() { 


        }

        public void BecomeDirty()
        {
            isDirty = true;
        }

        public void CleanUp()
        {
            isDirty = false;
        }


        public bool CoolDownManager(float deltatime)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator AimProjectiles()
        {
            throw new System.NotImplementedException();
        }

        public void Shoot(Vector2 direction)
        {
            throw new System.NotImplementedException();
        }
    }
}