namespace Jili.StatSystem
{
    // Enumeração que define os tipos de modificadores de estatísticas
    public enum AttributeModType
    {
        Flat = 100,                   // Modificador flat, prioridade 0 = quer sempre ser o primeiro; Exemplo + 5 de dano
        PercentileAdditive = 200,     // Modificador percentual, prioridade 1 = quer afetar apenas os valores flat; Exemplo + 10% de dano base
        PercentileCumulative = 300,   // Modificador percentual, prioridade 2 = quer afetar o total; Exemplo + 10% de dano total
    }

    // Classe que define um modificador de stats
    public class AttributeModifier
    {
        public readonly float Value;        // Valor do modificador
        public readonly AttributeModType Type;   // Tipo do modificador
        public readonly int Priority;       // Prioridade do modificador (por padrão é o tipo, mas pode ser alterado no construtor)
        public readonly object Source;      // Fonte do modificador

        public AttributeModifier(float value, AttributeModType type, int priority, object source) // CONSTRUTOR DO MODIFICADOR
        {
            Value = value;
            Type = type;
            Priority = priority;
            Source = source;
        }

        // CONSTRUTOR QUE USA O CONSTRUTOR PRINCIPAL E PASSA A PRIORIDADE PADRÃO DO TIPO
        public AttributeModifier(float value, AttributeModType type) : this(value, type, (int)type, null) { }

        public AttributeModifier(float value, AttributeModType type, int priority) : this(value, type, priority, null) { }

        public AttributeModifier(float value, AttributeModType type, object source) : this(value, type, (int)type, source) { }
    }
}