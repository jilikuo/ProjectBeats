using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Jili.StatSystem
{
    public enum StatType
    {
        AttackDamage = 1011,
        Health = 1021,
        Armor = 1031,
        //Mobility
        MovementSpeed = 2011,
        Acceleration = 2012
    }

    [Serializable]
    public class Stat
    {
        protected readonly string Name;        // Nome do stat
        public StatType Type;
        public float BaseValue;             // Valor base do stat
        private readonly List<Attribute> RelevantAtts;



        public virtual float Value          // Valor Após modificadores do stat
        {
            get
            {
                if (isDirty || BaseValue != lastBaseValue)
                {
                    BaseValue = StatFormulas.CalculateBaseStatValue(Type, RelevantAtts);
                    lastBaseValue = BaseValue;
                    _value = CalculateFinalValue();
                    isDirty = false;
                }
                return _value;
            }
        }


        protected bool isDirty = true;
        protected float _value;
        protected float lastBaseValue = float.MinValue;

        protected readonly List<StatModifier> statModifiers;  // Lista de modificadores do stat
        public readonly ReadOnlyCollection<StatModifier> StatModifiers; // Lista de modificadores do stat somente leitura

        public Stat(StatType type, List<Attribute> callerAtts) // Construtor que inicializa a lista de modificadores
        {
            if (Enum.IsDefined(typeof(StatType), type))
                Type = type;
            else
                throw new ArgumentOutOfRangeException("O Atributo ID ", type, " Não está definido. Você queria atribuir um valor ao atributo? O construtor precisa de um ID de tipo como primeiro argumento. Verificar 'Attribute.cs'");

            statModifiers = new List<StatModifier>();
            StatModifiers = statModifiers.AsReadOnly();
            Name = type.ToString();

            RelevantAtts = StatFormulas.FilterRelevantAttributes(type, callerAtts);
            BaseValue = StatFormulas.CalculateBaseStatValue(type, callerAtts);

            foreach (var att in RelevantAtts)
            {
                att.OnValueChanged += BecomeDirty;
            }
        }

        public virtual void BecomeDirty()
        {
            isDirty = true;
        }

        public virtual void AddModifier(StatModifier modifier)
        {
            isDirty = true;
            statModifiers.Add(modifier);
            statModifiers.Sort(CompareModifierPriority);
        }

        protected virtual int CompareModifierPriority(StatModifier a, StatModifier b) // Comparador entre as prioridades dos modificadores
        {
            if (a.Priority < b.Priority)
                return -1;
            else if (a.Priority > b.Priority)
                return 1;
            return 0;
        }

        public virtual bool RemoveModifier(StatModifier modifier)
        {
            if (statModifiers.Remove(modifier))
            {
                isDirty = true;
                return true;
            }
            return false;
        }

        public virtual bool RemoveAllModifiersFromSource(object source)
        {
            bool didRemove = false;

            statModifiers.RemoveAll(statModifiers =>
            {
                if (statModifiers.Source == source)
                {
                    isDirty = true;
                    didRemove = true;
                    return true;
                }
                return false;
            });
            return didRemove;
        }

        protected virtual float CalculateFinalValue()
        {
            float finalValue = BaseValue;
            float percentageSum = 0;

            if (statModifiers != null && statModifiers.Count > 0)
            {
                for (int i = 0; i < statModifiers.Count; i++)
                {
                    StatModifier modifier = statModifiers[i];

                    if (modifier.Type == StatModType.Flat)
                        finalValue += modifier.Value;

                    else if (modifier.Type == StatModType.PercentileAdditive)
                    {
                        percentageSum += modifier.Value;

                        if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentileAdditive)
                        {
                            finalValue += finalValue * percentageSum;
                            percentageSum = 0;
                        }
                    }
                    else if (modifier.Type == StatModType.PercentileCumulative)
                        finalValue *= 1 + modifier.Value;
                }
            }
            return (float)Math.Round(finalValue, 6);
        }
    }
}