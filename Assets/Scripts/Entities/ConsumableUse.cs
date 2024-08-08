using System.Collections;
using UnityEngine;

public class ConsumableUse : MonoBehaviour
{
    private ConsumableType consumableType;
    private string playerTag = "Player";
    private GameObject player;
    private EntityStats stats;

    private void Start()
    {
        consumableType = GetComponent<ConsumableType>();
        player = GameObject.FindGameObjectWithTag(playerTag);
        stats = player.GetComponent<EntityStats>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject != player)
        {

            Collider2D notPlayer = collision.gameObject.GetComponent<Collider2D>();
            Collider2D consumable = gameObject.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(notPlayer, consumable);
            return;
        }

        switch (consumableType.category)
        {
            case ConsumableType.Category.Health:
                

                Debug.Log("Consumed Health");
                break;

            case ConsumableType.Category.Mana:
                

                Debug.Log("Consumed Mana");
                break;

            case ConsumableType.Category.Exp:
                ConsumeExpOrb();

                Debug.Log("Consumed Experience");
                break;

            case ConsumableType.Category.Gold:


                Debug.Log("Consumed Gold");
                break;

            case ConsumableType.Category.Card:


                Debug.Log("Consumed Card");
                break;

            default:


                Debug.Log("Unknown consumable type");
                break;
        }

       
        Destroy(gameObject);
    }

    void ConsumeExpOrb()
    {
        switch (consumableType.size)
        {
            case ConsumableType.Size.Small:
                stats.GainExperience(3);

                Debug.Log("Size: Small");
                break;

            case ConsumableType.Size.Medium:
                stats.GainExperience(15);

                Debug.Log("Size: Medium");
                break;

            case ConsumableType.Size.Big:
                stats.GainExperience(50);

                Debug.Log("Size: Big");
                break;

            case ConsumableType.Size.Super:
                stats.GainExperience(250);

                Debug.Log("Size: Super");
                break;

            case ConsumableType.Size.Legendary:
                stats.GainExperience(1000);

                Debug.Log("Size: Legendary");
                break;

            default:


                Debug.Log("Unknown size");
                break;
        }
    }
}
