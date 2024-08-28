using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace Jili.StatSystem
{
    public enum StatType
    {
        NULL = 0,
        //VolatileStats
        Health  = 1,
        Mana    = 2,
        Stamina = 3,

        //Physical
        AttackDamage = 1011,
        Armor = 1031,
        HealthRegen = 1041,

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

        public event Action<Stat> OnValueChanged; // Event to notify when the value changes

        public virtual float Value          // Valor Após modificadores do stat
        {
            get
            {
                if (isDirty || BaseValue != lastBaseValue)
                {
                    //Debug.Log(this.Name + " dirtiness is: " + isDirty + " BaseValue is: " + BaseValue + " lastBaseValue is: " + lastBaseValue);
                    if (((int)Type) < (int)StatType.IndependentBase)
                    {
                        BaseValue = StatFormulas.CalculateBaseStatValue(Type, RelevantAtts);
                    }
                    else
                    {
                        BaseValue = lastBaseValue; // se o stat for independente, seu valor base não muda
                    }

                    lastBaseValue = BaseValue;
                    lastValue = _value;
                    _value = CalculateFinalValue();
                    isDirty = false;
                    TriggerOnValueChanged(); // Trigger the event when value changes

                    if (isVolatile)
                    {
                        if (constructing)
                        {
                            //do nothing
                        }
                        else if (CurrentVolatileValue + (_value - lastValue) < 0)
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
        protected bool constructing;
        protected float _value;
        protected float lastBaseValue = float.MinValue;
        protected float lastValue;

        protected readonly List<StatModifier> statModifiers;            // Lista de modificadores do stat
        public readonly ReadOnlyCollection<StatModifier> StatModifiers; // Lista de modificadores do stat somente leitura

        public Stat(StatType type, List<Attribute> callerAtts)          // Construtor que inicializa a lista de modificadores
        {
            constructing = true;
            // se o tipo não for definido, lançar uma exceção
            if (Enum.IsDefined(typeof(StatType), type))
                Type = type;
            else
                throw new ArgumentOutOfRangeException("O Atributo ID ", type, " Não está definido. Você queria atribuir um valor ao atributo? O construtor precisa de um ID de tipo como primeiro argumento. Verificar 'Attribute.cs'");

            // se o tipo for independente, não há atributos relacionados
            if (!(((int)Type) >= (int)StatType.IndependentBase))
            {
                RelevantAtts = StatFormulas.FilterRelevantAttributes(type, callerAtts);
                BaseValue = StatFormulas.CalculateBaseStatValue(type, callerAtts);
                foreach (var att in RelevantAtts)
                {
                    att.OnValueChanged += BecomeDirty;
                    att.ReadValue();
                }
                if (type == StatType.Health || type == StatType.Mana || type == StatType.Stamina)
                {
                    isVolatile = true;
                    CurrentVolatileValue = ReadValue(); // aqui precisamos ler o valor através do método, para garantir que ele não esteja Dirty ao começar
                                                           // (o que duplicaria o valor do currentvolatilevalue)
                                                           // manter atenção - pode causar problemas?
                }
            }

            Name = type.ToString();
            statModifiers = new List<StatModifier>();
            StatModifiers = statModifiers.AsReadOnly();
            constructing = false;
        }

        public Stat(StatType type, float baseValue) : this(type, new List<Attribute>())
        {
            BaseValue = baseValue;
            lastBaseValue = BaseValue;
        }

        public virtual void TriggerOnValueChanged()
        {
            OnValueChanged?.Invoke(this);
        }

        public virtual void BecomeDirty()
        {
            isDirty = true;
            // avisar aos ouvintes que o valor do stat precisa ser recarregado (não deveria ser necessário...
            // TODO: DESCOBRIR POR QUE O VALOR NÃO ESTÁ SENDO RECARREGADO SÓ DE O STAT ESTAR SUJO)
            TriggerOnValueChanged();
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

        public float ReadValue()
        {
            return this.Value;
        }

        public float ReadCurrentValue()
        {
            if (!isVolatile)
            {
                throw new Exception("This stat is not volatile. You should not try to access its current volatile value.");
            }
            return this.CurrentVolatileValue;
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