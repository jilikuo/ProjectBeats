using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jili.StatSystem.EntityTree.ConsumableSystem
{
    public class ConsumableType : MonoBehaviour
    {
        public ConsumableCategory category; 
        public ConsumableSize size; // keeping for debug reasons only
        public float FlatAmount;
        public float PercentageAmount; 
            

        private CVType ValueType; // ConsumableValueType

        private void Start()
        {
            ValueType = SetValueType();
        }

        private CVType SetValueType()
        {
            if ((FlatAmount > 0) && (PercentageAmount > 0))
                return CVType.Hybrid; // both flat and percentile
            else if (FlatAmount > 0)
                return CVType.Flat;
            else if (PercentageAmount > 0)
                return CVType.Percentile;
            throw new MissingReferenceException("NENHUM VALOR ATRIBUÍDO AO CONSUMÍVEL " +  category + " " + size);
        }

        public CVType GetValueType()
        {
            return ValueType; 
        }
        
        public float ReadValue(CVType option)
        {
            switch (option)
            {
                case CVType.Flat:
                    return FlatAmount;
                case CVType.Percentile:
                    return PercentageAmount;
                case CVType.Hybrid:
                    throw new ArgumentOutOfRangeException("NÃO DEVE-SE LER DIRETAMENTE O VALOR HÍBRIDO DE " + category + " " + size);
                default:
                    throw new MissingReferenceException("ERRO AO LER O TIPO DE " + category + " " + size);
            }
        }
    }

    // CVType stands for ConsumableValueType
    public enum CVType
    {
        Flat,
        Percentile,
        Hybrid
    }
}