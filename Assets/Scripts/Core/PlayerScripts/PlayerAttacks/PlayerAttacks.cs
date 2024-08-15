using UnityEngine;

namespace Jili.StatSystem.AttackSystem
{
    public class AttackSystem : MonoBehaviour
    {
        protected IShootable[] Weapons;
        public readonly int MaxSlots = 3;

        public void Awake()
        {
            Weapons = new IShootable[MaxSlots];
        }

        public void Update()
        {
            for (int i = 0; i < Weapons.Length; i++)
            {
                if (Weapons[i] != null)
                {
                    Weapons[i].CallCoolDownManager();
                }
            }
        }

        public void EquipWeapon(IShootable weapon)
        {
            for (int i = 0; i < Weapons.Length; i++)
            {
                if (Weapons[i] == null)
                {
                    Weapons[i] = weapon;
                    break;
                }
            }
        }

        public void UnequipWeapon(IShootable weapon)
        {
            for (int i = 0; i < Weapons.Length; i++)
            {
                if (Weapons[i] == weapon)
                {
                    Weapons[i] = null;
                    break;
                }
            }
        }

        public void UnequipAllWeapons()
        {
            for (int i = 0; i < Weapons.Length; i++)
            {
                Weapons[i] = null;
            }
        }
    }
}