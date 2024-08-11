using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Jili.StatSystem
{
    public enum AttributeType
    {
        //Physical
        Strength = 101,
        Constitution = 102,
        Resistance = 103,
        //Mobility
        Agility = 201
    }

    [Serializable]
    public class Attribute
    {
        private readonly string Name;       // Nome do stat
        public AttributeType Type;          // Tipo do stat
        public float BaseValue;             // Valor base do stat

        public event Action OnValueChanged; // Event to notify when the value changes

        public virtual float Value          // Valor Ap�s modificadores do stat
        {
            get
            {
                if (isDirty || BaseValue != lastBaseValue)
                {
                    lastBaseValue = BaseValue;
                    _value = CalculateFinalValue();
                    isDirty = false;
                    OnValueChanged?.Invoke(); // Trigger the event when value changes
                }
                return _value;
            }
        }

        protected bool isDirty = true;    // Flag para saber se o valor final precisa ser recalculado
        protected float _value;           // registra o valor do c�culo mais recente 
        protected float lastBaseValue = float.MinValue;    // registra o valor base mais recente

        protected readonly List<AttributeModifier> attributeModifiers;  // Lista de modificadores do stat
        public readonly ReadOnlyCollection<AttributeModifier> AttributeModifiers; // Lista de modificadores do stat somente leitura

        public Attribute(AttributeType type) // Construtor que inicializa a lista de modificadores
        {
            if (Enum.IsDefined(typeof(AttributeType), type))
                Type = type;
            else
                throw new ArgumentOutOfRangeException("O Atributo ID ", type, " N�o est� definido. Voc� queria atribuir um valor ao atributo? O construtor precisa de um ID de tipo como primeiro argumento. Verificar 'Attribute.cs'");

            attributeModifiers = new List<AttributeModifier>();
            AttributeModifiers = attributeModifiers.AsReadOnly();
            Name = type.ToString();
        }

        public Attribute(AttributeType type, float baseValue) : this(type) // Construtor que inicializa com o valor base especificado
        {
            BaseValue = baseValue;
        }

        public virtual void AddModifier(AttributeModifier modifier)   // M�todo para adicionar um modificador � lista
        {
            isDirty = true;                                         // Atualizamos a flag para recalcular o valor final
            attributeModifiers.Add(modifier);                           // Adicionamos o modificador � lista
            attributeModifiers.Sort(CompareModifierPriority);           // Ordenamos a lista de modificadores
        }

        protected virtual int CompareModifierPriority(AttributeModifier a, AttributeModifier b) // Comparador entre as prioridades dos modificadores
        {
            if (a.Priority < b.Priority)
                return -1;
            else if (a.Priority > b.Priority)
                return 1;
            return 0;
        }

        public virtual bool RemoveModifier(AttributeModifier modifier) // M�todo para remover um modificador da lista
        {
            if (attributeModifiers.Remove(modifier)) // Se tiver removido, atualizamos a flag para recalcular o valor final
            {
                isDirty = true; 
                return true;
            }

            return false;
        }

        public virtual bool RemoveAllModifiersFromSource(object source)
        {
            bool didRemove = false;

            attributeModifiers.RemoveAll(modifier =>
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
    
        protected virtual float CalculateFinalValue()                          // M�todo para calcular o valor final do stat
        {
            float finalValue = BaseValue;                            // Carregamos o valor base
            float percentageSum = 0;

            if (attributeModifiers != null && attributeModifiers.Count > 0)
            {
                for (int i = 0; i < attributeModifiers.Count && attributeModifiers != null; i++)           // Lemos todos os modificadores e adicionamos eles ao valor final
                {
                    AttributeModifier modifier = attributeModifiers[i];

                    if (modifier.Type == AttributeModType.Flat)                  // lida com modificadores flat
                        finalValue += modifier.Value;

                    else if (modifier.Type == AttributeModType.PercentileAdditive)
                    {
                        percentageSum += modifier.Value;

                        if (i + 1 >= attributeModifiers.Count || attributeModifiers[i + 1].Type != AttributeModType.PercentileAdditive)
                        {
                            finalValue *= percentageSum;
                            percentageSum = 0;
                        }
                    }
                    else if (modifier.Type == AttributeModType.PercentileCumulative)       // lida com modificadores percentuais
                        finalValue *= 1 + modifier.Value;
                }
            }
            return (float)Math.Round(finalValue, 6);                    // retornamos o valor tratado para evitar erros
        }
    }
}
