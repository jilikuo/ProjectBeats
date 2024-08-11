using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Jili.StatSystem
{

    [Serializable]
    public class EntityStats
    {
        public readonly string Name;
        public float BaseValue; // Valor base do stat

        public virtual float Value      // Valor Após modificadores do stat
        {
            get
            {
                if (isDirty || BaseValue != lastBaseValue)
                {
                    lastBaseValue = BaseValue;
                    _value = CalculateFinalValue();
                    isDirty = false;
                }
                return _value;
            }
        }

        protected bool isDirty = true;    // Flag para saber se o valor final precisa ser recalculado
        protected float _value;           // registra o valor do cáculo mais recente 
        protected float lastBaseValue = float.MinValue;    // registra o valor base mais recente

        protected readonly List<StatsModifiers> statsModifiers;  // Lista de modificadores do stat
        public readonly ReadOnlyCollection<StatsModifiers> StatsModifiers; // Lista de modificadores do stat somente leitura

        public EntityStats() // Construtor que inicializa a lista de modificadores
        {
            statsModifiers = new List<StatsModifiers>();
            StatsModifiers = statsModifiers.AsReadOnly();
        }

        public EntityStats(float baseValue) : this() // Construtor que inicializa com o valor base especificado
        {
            BaseValue = baseValue;
        }

        public virtual void AddModifier(StatsModifiers modifier)   // Método para adicionar um modificador à lista
        {
            isDirty = true;                                         // Atualizamos a flag para recalcular o valor final
            statsModifiers.Add(modifier);                           // Adicionamos o modificador à lista
            statsModifiers.Sort(CompareModifierPriority);           // Ordenamos a lista de modificadores
        }

        protected virtual int CompareModifierPriority(StatsModifiers a, StatsModifiers b) // Comparador entre as prioridades dos modificadores
        {
            if (a.Priority < b.Priority)
                return -1;
            else if (a.Priority > b.Priority)
                return 1;
            return 0;
        }

        public virtual bool RemoveModifier(StatsModifiers modifier) // Método para remover um modificador da lista
        {
            if (statsModifiers.Remove(modifier)) // Se tiver removido, atualizamos a flag para recalcular o valor final
            {
                isDirty = true; 
                return true;
            }

            return false;
        }

        public virtual bool RemoveAllModifiersFromSource(object source)
        {
            bool didRemove = false;

            statsModifiers.RemoveAll(modifier =>
            {
                if (modifier.Source == source)
                {
                    isDirty = true;
                    didRemove = true;
                    return true;
                }
                return false;
            });
            return didRemove;
        }
    
        protected virtual float CalculateFinalValue()                          // Método para calcular o valor final do stat
        {
            float finalValue = BaseValue;                            // Carregamos o valor base
            float percentageSum = 0;

            for (int i = 0; i < statsModifiers.Count; i++)           // Lemos todos os modificadores e adicionamos eles ao valor final
            {
                StatsModifiers modifier = statsModifiers[i];

                if (modifier.Type == StatModType.Flat)                  // lida com modificadores flat
                    finalValue += modifier.Value;
                else if (modifier.Type == StatModType.PercentileAdditive)
                {
                    percentageSum += modifier.Value;

                    if (i + 1 >= statsModifiers.Count || statsModifiers[i + 1].Type != StatModType.PercentileAdditive)
                    {
                        finalValue *= percentageSum;
                        percentageSum = 0;
                    }
                }
                else if (modifier.Type == StatModType.PercentileCumulative)       // lida com modificadores percentuais
                    finalValue *= 1 + modifier.Value;
            }
            return (float)Math.Round(finalValue, 6);                    // retornamos o valor tratado para evitar erros
        }
    }
}
