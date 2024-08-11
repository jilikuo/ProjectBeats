using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jili.OldStatSystem
{

    public class EntityAttributes : MonoBehaviour
    {
        void Start()
        {

        }

        void Update()
        {

        }
    }

    public abstract class AttributeCategory
    {
        public string Name { get; private set; }
        public List<EntityAttribute> Attributes { get; private set; }

        protected AttributeCategory(string name)
        {
            Name = name;
            Attributes = new List<EntityAttribute>();
        }

        public void AddAttribute(EntityAttribute attribute)
        {
            Attributes.Add(attribute);
            attribute.Category = this;
        }

        public virtual float CalculateTotalPoints()
        {
            float totalPoints = 0;
            foreach (var attribute in Attributes)
            {
                totalPoints += attribute.Value;
            }
            return totalPoints;
        }
    }

    public class PhysicalAttributes : AttributeCategory
    {
        public PhysicalAttributes() : base("Physical")
        {
            AddAttribute(new Strength(this));
            AddAttribute(new Resistance(this));
            AddAttribute(new Constitution(this));
            AddAttribute(new Vigor(this));
        }
    }

    public class MobilityAttributes : AttributeCategory
    {
        public MobilityAttributes() : base("Mobility")
        {
            AddAttribute(new Agility(this));
            AddAttribute(new Dextery(this));
            AddAttribute(new Accuracy(this));
            AddAttribute(new Finesse(this));
        }
    }

    public class MagicalAttributes : AttributeCategory
    {
        public MagicalAttributes() : base("Magical")
        {
            AddAttribute(new Intelligence(this));
            AddAttribute(new Willpower(this));
            AddAttribute(new Wisdom(this));
            AddAttribute(new Spirit(this));
        }
    }

    public class SocialAttributes : AttributeCategory
    {
        public SocialAttributes() : base("Social")
        {
            AddAttribute(new Charisma(this));
            AddAttribute(new Influence(this));
            AddAttribute(new Leadership(this));
            AddAttribute(new Presence(this));
        }
    }

    public class SpecialAttributes : AttributeCategory
    {
        public SpecialAttributes() : base("Special")
        {
            AddAttribute(new Luck(this));
            AddAttribute(new Level(this));
            AddAttribute(new Gold(this));
        }

        public override float CalculateTotalPoints()
        {
            float totalPoints = 0;

            foreach (var Luck in Attributes)
            {
                totalPoints += Luck.Value;
            }
            foreach (var Level in Attributes)
            {
                totalPoints += Level.Value;
            }
            return totalPoints;
        }
    }

    [System.Serializable]
    public abstract class EntityAttribute
    {
        public event Action<float> OnValueChanged;
        private float storedvalue;

        [SerializeField, HideInInspector] public string Name { get; private set; }
        [SerializeField] public float tempvalue { get; set; }

        public AttributeCategory Category { get; set; }

        public float Value
        {
            get => storedvalue;
            set
            {
                if (storedvalue != tempvalue)
                {
                    storedvalue = tempvalue;
                    OnValueChanged?.Invoke(storedvalue); // Emit the value change signal
                }
            }
        }

        protected EntityAttribute(string name, float initialValue, AttributeCategory category)
        {
            Name = name;
            tempvalue = initialValue;
            Category = category;
        }
    }


    ///* PHYSICAL ATTRIBUTES *///
    public class Strength : EntityAttribute
    {
        public Strength(AttributeCategory category) : base("Strength", 5, category) { }
    }

    public class Resistance : EntityAttribute
    {
        public Resistance(AttributeCategory category) : base("Resistance", 5, category) { }
    }

    public class Constitution : EntityAttribute
    {
        public Constitution(AttributeCategory category) : base("Constitution", 5, category) { }
    }

    public class Vigor : EntityAttribute
    {
        public Vigor(AttributeCategory category) : base("Vigor", 5, category) { }
    }


    ///* COORDINATION (MOBILITY) ATTRIBUTES *///
    public class Agility : EntityAttribute
    {
        public Agility(AttributeCategory category) : base("Agility", 5, category) { }
    }

    public class Dextery : EntityAttribute
    {
        public Dextery(AttributeCategory category) : base("Dextery", 5, category) { }
    }

    public class Accuracy : EntityAttribute
    {
        public Accuracy(AttributeCategory category) : base("Accuracy", 5, category) { }
    }

    public class Finesse : EntityAttribute
    {
        public Finesse(AttributeCategory category) : base("Finesse", 5, category) { }
    }


    ///* MAGICAL ATTRIBUTES *///
    public class Intelligence : EntityAttribute
    {
        public Intelligence(AttributeCategory category) : base("Intelligence", 5, category) { }
    }

    public class Willpower : EntityAttribute
    {
        public Willpower(AttributeCategory category) : base("Willpower", 5, category) { }
    }

    public class Wisdom : EntityAttribute
    {
        public Wisdom(AttributeCategory category) : base("Wisdom", 5, category) { }
    }

    public class Spirit : EntityAttribute
    {
        public Spirit(AttributeCategory category) : base("Spirit", 5, category) { }
    }

    ///* SOCIAL ATTRIBUTES *///
    public class Charisma : EntityAttribute
    {
        public Charisma(AttributeCategory category) : base("Charisma", 5, category) { }
    }

    public class Influence : EntityAttribute
    {
        public Influence(AttributeCategory category) : base("Influence", 5, category) { }
    }

    public class Leadership : EntityAttribute
    {
        public Leadership(AttributeCategory category) : base("Leadership", 5, category) { }
    }

    public class Presence : EntityAttribute
    {
        public Presence(AttributeCategory category) : base("Presence", 5, category) { }
    }

    ///* SPECIAL ATTRIBUTES *///
    public class Luck : EntityAttribute
    {
        public Luck(AttributeCategory category) : base("Luck", 5, category) { }
    }

    public class Level : EntityAttribute
    {
        public Level(AttributeCategory category) : base("Level", 0, category) { }
    }

    public class Gold : EntityAttribute
    {
        public Gold(AttributeCategory category) : base("Gold", 0, category) { }
    }
}