using System.Collections.Generic;
using UnityEngine;

namespace Jili.StatSystem
{

    public abstract class EntityBase : MonoBehaviour
    {

        public List<Attribute> attlist = new List<Attribute>();

        protected virtual void attListAdd(Attribute att)
        {
            attlist.Add(att);
        }
    }
}