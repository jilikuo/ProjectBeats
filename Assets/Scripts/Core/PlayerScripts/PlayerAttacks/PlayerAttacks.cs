using Jili.StatSystem.EntityTree;
using System.Threading.Tasks;
using UnityEngine;

namespace Jili.StatSystem.AttackSystem
{
    public class PlayerAttacks : MonoBehaviour
    {
        [SerializeField]
        protected IShootable[] Weapons;
        public readonly int MaxSlots = 3;

        public void Awake()
        {
            // inicia o array de armas com o tamanho máximo de slots
            Weapons = new IShootable[MaxSlots];
        }

        private void Start()
        {
            // equipa a arma base do jogador
            // EquipWeapon(this.gameObject.GetComponent<PlayerIdentity>().baseWeapon); // >> Ler obs @PlayerIdentity sobre baseweapon <<
        }

        public void FixedUpdate()
        {
            foreach(IShootable weapon in Weapons)
            {
                if (weapon != null)
                {
                    weapon.TryShoot(Time.fixedDeltaTime);
                }
            };
        }

        public void EquipWeapon(IShootable weapon)
        {
            Debug.Log("Equipping weapon..." + weapon.GetType());
            for (int i = 0; i < Weapons.Length; i++)
            {
                if (Weapons[i] == null)
                {
                    Weapons[i] = weapon;
                    Debug.Log($"Weapon equipped in slot {i}.");
                    break;
                }
            }
        }

        public void UnequipWeapon(IShootable weapon)
        {
            Debug.Log("Unequipping weapon...");
            for (int i = 0; i < Weapons.Length; i++)
            {
                if (Weapons[i] == weapon)
                {
                    Weapons[i] = null;
                    Debug.Log($"Weapon unequipped from slot {i}.");
                    break;
                }
            }
        }

        public void UnequipAllWeapons()
        {
            Debug.Log("Unequipping all weapons...");
            for (int i = 0; i < Weapons.Length; i++)
            {
                Weapons[i] = null;
                Debug.Log($"Weapon slot {i} cleared.");
            }
        }
    }
}