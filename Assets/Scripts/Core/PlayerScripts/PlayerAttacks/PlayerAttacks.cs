using Jili.StatSystem.EntityTree;
using System.Threading.Tasks;
using UnityEngine;

namespace Jili.StatSystem.AttackSystem
{
    public class AttackSystem : MonoBehaviour
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
            EquipWeapon(this.gameObject.GetComponent<PlayerIdentity>().baseWeapon);
        }

        public void FixedUpdate()
        {
            //TENTA-SE ATIRAR COM TODAS AS ARMAS, POR ISSO O PROCESSAMENTO PARALELO.
            float deltaTime = Time.fixedDeltaTime;
            Parallel.ForEach(Weapons, weapon =>
            {
                if (weapon != null)
                {
                    Debug.Log("Attempting to shoot with weapon.");
                    weapon.TryShoot(deltaTime);
                }
            });
        }

        public void EquipWeapon(IShootable weapon)
        {
            Debug.Log("Equipping weapon...");
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