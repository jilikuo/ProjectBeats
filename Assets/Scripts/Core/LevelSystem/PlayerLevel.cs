    using Jili.StatSystem;
    using System;
    using UnityEngine;

    namespace Jili.StatSystem.LevelSystem
    {
        public class PlayerLevel : MonoBehaviour
        {
            public int attributePointsPerLevel = 5;
            private int Level { get; set; }
            private float experience;
            private float Experience
            {
                get
                {
                    CheckLevelUp();
                    return experience;
                }
                set
                {
                    experience = value;
                    CheckLevelUp();
                }
            }
            private float ExperienceToNextLevel { get; set; }
            private float TotalExperience { get; set; }
            private AttributePoints attributePoints;

            private class AttributePoints
            {
                public int Free { get; private set; }
                private int Spent { get; set; }
                private int Total { get; set; }

                private readonly int perLevelGain;
                public AttributePoints(int perLevel)
                {
                    perLevelGain = perLevel;
                    Free = 0;
                    Spent = 0;
                    Total = 0;
                }

                public bool hasFreePoints()
                {
                    bool test = Free > 0;
                    if ((test == false) && (Spent != Total))
                    {
                        throw new System.Exception("ERROR OCURRED, ATTRIBUTES POINTS DID NOT CONSUME CORRECTLY, " +
                                                   "SPENT POINTS = " + Spent + " WHEN IT SHOULD HAVE BEEN = " + Total);
                    }

                    return test;
                }

                public void AddFreePoints(int points = 0)
                {
                    if (points > 0)
                    {
                        Console.WriteLine("CAUTION: You are adding Attribute Points Manually, if this was not supposed to happen, " +
                                          "you probably should remove the value from AddFreePoints() Method.");
                        Free = points;
                        Total = points;
                    }
                    else
                    {
                        Free = perLevelGain;
                        Total = perLevelGain;
                    }
                }

                public void SpendFreePoints() 
                {
                    Free--;
                    Spent++;
                }
            
            }

            public void Awake()
            {
                Level = 1;
                Experience = 0;
                ExperienceToNextLevel = 100;
                TotalExperience = 0;
                attributePoints = new AttributePoints(attributePointsPerLevel);   
            }

            public void GainExp(float amount)
            {
                Experience += amount;
            }

            private void CheckLevelUp()
            {
                while (experience >= ExperienceToNextLevel)
                {
                    LevelUp();
                    experience -= ExperienceToNextLevel;
                }
            }

            public void LevelUp()
            {
                Level++;
                RecalculateNextLevelExp();
            }

            private void RecalculateNextLevelExp()
            {
                ExperienceToNextLevel = 100 + 10 * Level + (Mathf.FloorToInt(Level/10) * 100) + MathF.Sqrt(Level) + (MathF.PI + Level);
            }

            public int ReadFreeAttPoints()
            {
                return attributePoints.Free;
            }

            public void SpendAttPoints(int amount = 1)
            {
                if (attributePoints.hasFreePoints())
                {
                    int i = 0;
                    while ((i < amount) && (attributePoints.hasFreePoints()))
                    {
                        attributePoints.SpendFreePoints();
                        i++;
                    }
                    return;
                }
                return;
            }
        }
    }