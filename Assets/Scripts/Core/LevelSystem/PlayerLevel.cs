using Jili.StatSystem;
using System;
using UnityEngine;

namespace Jili.StatSystem.LevelSystem
{
    [Serializable]
    public struct AttributePoints
    {
        public int free;
        public int spent;
        public int total;

        public AttributePoints(int startingValue = 0)
        {
            free = startingValue;
            spent = 0;
            total = startingValue;
        }
    }

    public class PlayerLevel : MonoBehaviour
    {
        public Action OnLevelUp;

        [SerializeField] private int attributePointsPerLevel = 5;
        [SerializeField] private int level = 1;
        [SerializeField] private float experience = 0;
        [SerializeField] private float totalExp = 0; // I could consider not using that.
        [SerializeField] private float nextLevelExp = 100;
        [SerializeField] private AttributePoints attributePoints = new();

        public void AddExp(float amount)
        {
            experience += amount;
            CheckLevelUp();
        }
        
        private void CheckLevelUp()
        {
            bool hasLeveled = false;
            if (experience > nextLevelExp)
            {
                while (experience >= nextLevelExp)
                {
                    experience -= nextLevelExp;
                    LevelUp();
                    hasLeveled = true;
                }
            }
            if (hasLeveled)
            {
                OnLevelUp?.Invoke();
            }
        }

        private void LevelUp()
        {
            ++level;
            AddFreeAttPoints(attributePointsPerLevel);
            UpdateNextLevelExp();
        }

        // TODO: Create a readable formula.
        private void UpdateNextLevelExp()
        {
            nextLevelExp = 100 + ((MathF.PI * MathF.PI * MathF.PI) * level) + (Mathf.FloorToInt(level / 2) * 22)
            + (Mathf.FloorToInt(level / 3) * 33) + (Mathf.FloorToInt(level / 5) * 55) + (Mathf.FloorToInt(level / 7) * 77)
            + (Mathf.FloorToInt(level / 9) * 99) + (Mathf.FloorToInt(level / 11) * 1111) + (Mathf.FloorToInt(level / 13) * 1313)
            + (Mathf.FloorToInt(level / 17) * 1717) + (Mathf.FloorToInt(level / 19) * 1919) + (Mathf.FloorToInt(level / 23) * 2323)
            + (Mathf.FloorToInt(level / 10) * 250) + (Mathf.FloorToInt(level / 25) * 1000) + (Mathf.FloorToInt(level / 50) * 3500)
            + (level * level) + MathF.Sqrt(level * level * level) + MathF.Sqrt(level);
        }

        public void AddFreeAttPoints(int value)
        {
            attributePoints.free += value;
            attributePoints.total += value;
        }

        public void SpendAttPoints(int value)
        {
            attributePoints.free -= value;
            attributePoints.spent += value;
        }

        public int GetFreeAttPoints()
        {
            return attributePoints.free;
        }

        public int GetLevel()
        {
            return level;
        }

        public int GetNextLevelExp()
        {
            return Mathf.FloorToInt(nextLevelExp);
        }

        public int GetExperience()
        {
            return Mathf.FloorToInt(experience);
        }

        public bool HasFreeAttributePoints()
        {
            return attributePoints.free > 0;
        }
    }
}