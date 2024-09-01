/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jili.OldStatSystem
{
    public class PlayerEntity : MonoBehaviour, IMovable, IAccerable
    {
        public MobilityAttributes mobility;
        public Agility agility { get; set; }
        public Dextery dextery { get; set; }

        public MaxSpeed maxSpeed { get; set; }
        public Acceleration acceleration { get; set; }

        void Awake()
        {
            // Inicialize as propriedades aqui
            agility = new Agility(mobility);
            dextery = new Dextery(mobility);
            maxSpeed = new MaxSpeed(agility);
            acceleration = new Acceleration(dextery);
        }
    }
}*/