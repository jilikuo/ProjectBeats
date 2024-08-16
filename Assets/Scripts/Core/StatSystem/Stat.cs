using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Jili.StatSystem
{
    public enum StatType
    {
        //VolatileStats
        Health  = 1,
        Mana    = 2,
        Stamina = 3,
        //Physical
        AttackDamage = 1011,
        Armor = 1031,

        //Mobility
        MovementSpeed = 2011,
        Acceleration = 2012,
        AttacksPerSecond = 2013,
        AttackRange = 2041,
        
        // INDEPENDENT STATS
        IndependentBase = 9999,
        ProjectileNumber = 10001,
        ProjectileSpeed = 10002
    }

    [Serializable]
    public class Stat
    {
        protected readonly string Name;        // Nome do stat
        public StatType Type;
        protected float BaseValue;             // Valor base do stat
        private readonly List<Attribute> RelevantAtts;

        public event Action OnValueChanged; // Event to notify when the value changes

        public virtual float Value          // Valor Ap�s modificadores do stat
        {
            get
            {
                if (isDirty || BaseValue != lastBaseValue)
                {
                    if (!(((int)Type) >= (int)StatType.IndependentBase))
                    {
                        BaseValue = StatFormulas.CalculateBaseStatValue(Type, RelevantAtts);
                    }
                    else
                    {
                        BaseValue = lastBaseValue; // se o stat for independente, seu valor base n�o muda
                    }

                    lastBaseValue = BaseValue;
                    lastValue = _value;
                    _value = CalculateFinalValue();
                    isDirty = false;
                    OnValueChanged?.Invoke(); // Trigger the event when value changes

                    if (isVolatile)
                    {
                        if (CurrentVolatileValue + (_value - lastValue) < 0)
                        {
                            CurrentVolatileValue = 1;
                        }
                        else
                        {
                            CurrentVolatileValue += _value - lastValue;
                        }
                    }
                }
                return _value;
            }
        }


        protected bool isVolatile = false;
        public float CurrentVolatileValue;
        protected bool isDirty = true;
        protected float _value;
        protected float lastBaseValue = float.MinValue;
        protected float lastValue;

        protected readonly List<StatModifier> statModifiers;  // Lista de modificadores do stat
        public readonly ReadOnlyCollection<StatModifier> StatModifiers; // Lista de modificadores do stat somente leitura

        public Stat(StatType type, List<Attribute> callerAtts) // Construtor que inicializa a lista de modificadores
        {
            // se o tipo n�o for definido, lan�ar uma exce��o
            if (Enum.IsDefined(typeof(StatType), type))
                Type = type;
            else
                throw new ArgumentOutOfRangeException("O Atributo ID ", type, " N�o est� definido. Voc� queria atribuir um valor ao atributo? O construtor precisa de um ID de tipo como primeiro argumento. Verificar 'Attribute.cs'");

            // se o tipo for independente, n�o h� atributos relacionados
            if (!(((int)Type) >= (int)StatType.IndependentBase))
            {
                RelevantAtts = StatFormulas.FilterRelevantAttributes(type, callerAtts);
                BaseValue = StatFormulas.CalculateBaseStatValue(type, callerAtts);
                foreach (var att in RelevantAtts)
                {
                    att.OnValueChanged += BecomeDirty;
                }
                if (type == StatType.Health || type == StatType.Mana || type == StatType.Stamina)
                {
                    isVolatile = true;
                    CurrentVolatileValue = BaseValue;
                }
            }

            Name = type.ToString();
            statModifiers = new List<StatModifier>();
            StatModifiers = statModifiers.AsReadOnly();
        }

        public Stat(StatType type, float baseValue) : this(type, new List<Attribute>())
        {
            BaseValue = baseValue;
            lastBaseValue = BaseValue;
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