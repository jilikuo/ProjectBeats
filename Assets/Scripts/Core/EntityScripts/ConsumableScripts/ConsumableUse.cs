using Jili.StatSystem.LevelSystem;
using System;
using UnityEngine;

namespace Jili.StatSystem.EntityTree.ConsumableSystem
{
    public class ConsumableUse : MonoBehaviour
    {
        private readonly string playerTag = "Player";
        private readonly string consumableTag = "Consumable"; 
        private readonly string collectionAuraTag = "CollectionAura";

        private ConsumableType consumableType;

        private GameObject player;
        private PlayerIdentity playerIdentity;
        private PlayerLevel levelSystem;
        private Rigidbody2D rb;

        private float acceleration = 0.01f;
        private bool moveTowardsPlayer = false;

        public static event Action<int> OpenCardPacket;

        private void Start()
        {
            consumableType = GetComponent<ConsumableType>();
            player = GameObject.FindGameObjectWithTag(playerTag);
            playerIdentity = player.GetComponent<PlayerIdentity>();
            levelSystem = player.GetComponent<PlayerLevel>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (moveTowardsPlayer)
            {
                Vector2 direction = player.transform.position - transform.position;
                direction.Normalize();
                rb.velocity = direction * acceleration;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag(collectionAuraTag))
            {
                //TODO: ignorar todas as colisões, exceto com o jogador

                moveTowardsPlayer = true;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(consumableTag))
            {
                return;
            }

            if (collision.gameObject != player)
            {

                Collider2D notPlayer = collision.gameObject.GetComponent<Collider2D>();
                Collider2D consumable = gameObject.GetComponent<Collider2D>();
                Physics2D.IgnoreCollision(notPlayer, consumable);
                return;
            }

            switch (consumableType.category)
            {
                case ConsumableCategory.Health:


                    Debug.Log("Consumed Health");
                    break;

                case ConsumableCategory.Mana:


                    Debug.Log("Consumed Mana");
                    break;

                case ConsumableCategory.Exp:
                    ConsumeExpOrb();

                    break;

                case ConsumableCategory.Gold:


                    Debug.Log("Consumed Gold");
                    break;

                case ConsumableCategory.Card:


                    ConsumeCard();
                    break;

                default:


                    throw new ArgumentOutOfRangeException("INVALID CONSUMABLE USE");
            }


            Destroy(gameObject);
        }

        void ConsumeExpOrb()
        {


            switch (consumableType.GetValueType())
            {
                case CVType.Flat:
                    levelSystem.GainExp(consumableType.ReadValue(CVType.Flat));
                    break;

                case CVType.Percentile:
                    levelSystem.GainExp(consumableType.ReadValue(CVType.Percentile));
                    break;

                case CVType.Hybrid:

                    // tenta descobrir se vale mais a pena aumentar a exp em % ou flat antes de consumir, e então consome a melhor opção
                    float tempRemainingExp;
                    tempRemainingExp = levelSystem.ReadNextLevelExp() - levelSystem.ReadExperience();

                    if (consumableType.ReadValue(CVType.Flat) > tempRemainingExp)
                    {
                        levelSystem.GainExp(consumableType.ReadValue(CVType.Flat));
                        levelSystem.GainExp(consumableType.ReadValue(CVType.Percentile));
                        break;
                    }

                    levelSystem.GainExp(consumableType.ReadValue(CVType.Percentile));
                    levelSystem.GainExp(consumableType.ReadValue(CVType.Flat));
                    break;

                default:
                    throw new ArgumentOutOfRangeException("UNKNOW ISSUE, CVTYPE NOT FOUND");
            }
        }

        void ConsumeCard()
        {
            OpenCardPacket?.Invoke((int)this.consumableType.ReadValue(CVType.Flat));
        }

    }
}