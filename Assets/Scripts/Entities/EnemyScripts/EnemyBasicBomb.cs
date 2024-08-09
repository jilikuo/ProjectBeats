using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer)), RequireComponent(typeof(EnemyDrop))]
public abstract class EnemyBomber : MonoBehaviour, ISuicidable, IMovable
{

    // Atributos de inimigo bomba suicida
    // Físicos
    public PhysicalAttributes physicalAttributes;
    public Strength strenght { get; set; }
    public Constitution constitution { get; set; }

    // Mobilidade
    public MobilityAttributes mobilityAttributes;
    public Agility agility { get; set; }

    // Status de inimigo bomba suicida
    public PhysicalDamage physicalDamage { get; set; }
    public MaxHealth maxHealth { get; set; }
    public Health health { get; set; }
    public MaxSpeed maxSpeed { get; set; }

    // variáveis específicas de instancias de inimigo bomba suicida
    public SpriteRenderer spriteRenderer { get; set; }
    public Color originalColor { get; set; }
    public Color darknedColor { get; set; }

    // Métodos de inimigo bomba suicida
    public abstract void VisualizeDamage();
    public abstract void Suicide();
    public abstract void CheckHealth();
    public void TakeDamage(float incomingDamage)
    { 
        health.CurrentValue -= incomingDamage;
    }
    public abstract void StartDeathRoutine();
}


public class EnemyBasicBomb : EnemyBomber
{
    public override void VisualizeDamage()
    {
        float healthPercentage = health.CurrentValue / maxHealth.Value;

        // Update opacity
        Color newColor = Color.Lerp(darknedColor, originalColor, healthPercentage);
        newColor.a = 1;
        spriteRenderer.color = newColor;
    }

    public override void Suicide()
    {
        health.CurrentValue = 0;
    }

    public override void CheckHealth()
    {
        if (health.CurrentValue <= 0)
        {
            StartDeathRoutine();
            Destroy(this.gameObject);
        }
    }

    public override void StartDeathRoutine()
    {
        gameObject.GetComponent<EnemyDrop>().CheckDrop();
    }

}