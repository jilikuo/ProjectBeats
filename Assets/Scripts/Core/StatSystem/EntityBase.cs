using System.Collections.Generic;
using UnityEngine;

namespace Jili.StatSystem
{

    public abstract class EntityBase : MonoBehaviour
    {

        public List<Attribute> attList = new List<Attribute>();
        public List<Stat> statList = new List<Stat>();

        protected virtual void attListAdd(Attribute att)
        {
            attList.Add(att);
        }

        protected virtual void statListAdd(Stat stat)
        {
            statList.Add(stat);
        }

        public abstract float ReadStatValueByType(StatType type);
    }
}