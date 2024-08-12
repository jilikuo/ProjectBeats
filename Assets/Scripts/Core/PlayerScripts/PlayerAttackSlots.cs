using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using Jili.StatSystem.EntityTree;

namespace Jili.StatSystem.AttackSystem
{
    public enum WeaponIDs
    {
        JinxMinigun = 101
    }

    [Serializable]
    [RequireComponent(typeof(PlayerIdentity))]
    public class PlayerAttackSlots : MonoBehaviour
    {
        protected List<WeaponIDs> weaponIds;
        public ReadOnlyCollection<WeaponIDs> WeaponIds;

        protected PlayerIdentity player;
        protected int maxSlots = 3;

        private void Awake()
        {
            weaponIds = new List<WeaponIDs>();
            WeaponIds = weaponIds.AsReadOnly();
        }

        private void Start()
        {
            player = GetComponent<PlayerIdentity>();
            StartFirstSlot(player.baseWeapon);
        }

        private void LateUpdate()
        {

        }

        protected virtual bool CheckForEmptySlot()
        {
            if (weaponIds.Count >= maxSlots)
                return false;
            else
                return true;
        }

        protected virtual bool AddToFreeSlot(WeaponIDs weapon)
        {
            if (CheckForEmptySlot())
            {
                weaponIds.Add(weapon);
                return true;
            }

            else return false;
        }

        protected virtual bool StartFirstSlot(WeaponIDs weapon)
        {
            if (weaponIds.Count == 0)
            {
                AddToFreeSlot(weapon);
                return true;
            }
            return false;
        }
    }
}