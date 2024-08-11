namespace Jili.StatSystem
{
    public enum StatModType
    {
        Flat = 100,
        PercentileAdditive = 200,
        PercentileCumulative = 300,
    }
    public class StatModifier
    {
        public readonly float Value;
        public readonly StatModType Type;
        public readonly int Priority;
        public readonly object Source;

        public StatModifier(float value, StatModType type, int priority, object source)
        {
            Value = value;
            Type = type;
            Priority = priority;
            Source = source;
        }
        public StatModifier(float value, StatModType type) : this(value, type, (int)type, null) { }

        public StatModifier(float value, StatModType type, int priority) : this(value, type, priority, null) { }

        public StatModifier(float value, StatModType type, object source) : this(value, type, (int)type, source) { }
    }
}