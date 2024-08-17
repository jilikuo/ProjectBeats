using Jili.StatSystem.AttackSystem;
using System;
using UnityEngine;

namespace Jili.StatSystem.EntityTree
{

    public abstract class ProjectileBase : MonoBehaviour, IProjectile
    {
        public readonly string playerTag = "Player";
        public readonly string enemyTag = "Enemy";

        public GameObject Parent; // The GameObject that should not collide with this
        public GameObject Target; // if player, then target is enemy, if enemy, then target is player
        public IShootable source; // The Weapon/Spell/Skill/Thing that fired this projectile
        public float damage;      // The Wepon/Spell/Skill/Thing's damage value when this was fired
        public float range;       // The Weapon/Spell/Skill/Thing's range value when this was fired
        public int tankableHits;
        public int strenght;
        private Vector3 startingPoint;

        public virtual void Start()
        {
            ValidateProjectile();
            FindTarget();
            CalculateStartingPoint();
            InitializeProjectile();
        }

        protected virtual void ValidateProjectile()
        {
            if (Parent == null) 
            {
                throw new ArgumentNullException("tried to fire a projectile without naming a parent gameobject (we need it in order to calculate the targetTransform)");
            }
            if (source == null) 
            {
                throw new ArgumentNullException("tried to fire a projectile without naming a source object (we need it to calculate projectile damage and range)");
            }
        }

        void FixedUpdate()
        {
            VerifyRange();
        }

        //Currently not implemented, even tho we already find the target in the Start method
        protected virtual void FindTarget()
        {
            if (Parent.tag == playerTag)
            {
                Target = null; // TODO: Find the closest enemy to mouse pointer
                               // OR maybe consider it any enemy in the scene?
                               // worry about it later; for now, just set it to null
            }
            if (Parent.tag == enemyTag)
            {
                Target = GameObject.FindGameObjectWithTag(playerTag); //TODO: Figure out how to deal
                                                                      //with targetting player allies
            }
        }

        protected virtual void CalculateStartingPoint()
        {
            startingPoint = this.transform.position;
        }

        protected virtual void InitializeProjectile()
        {
            throw new NotImplementedException("PROJECTILE EXPECTS A METHOD TO FIND ITS ATTACK DAMAGE AND RANGE");
        }

        // try stop s� � acionada quando um proj�til colide com o outro. neste momento um objeto ativa o trystop do outro.
        public virtual bool TryStop(int strenght)
        {
            // strenght = a for�a do proj�til colidindo com este
            tankableHits -= strenght;
            if (tankableHits > 0)
            {
                return false;
            }
            else
            {
                GetStopped();
                return true;
            }    
        }

        public virtual void GetStopped()
        {
            Destroy(gameObject);
        }

        public virtual void VerifyRange()
        {
            float distanceTravelled = Vector3.Distance(startingPoint, transform.position);
            if (distanceTravelled > range)
            {
                GetStopped();
            }
        }

        public virtual void OnCollisionEnter2D(Collision2D collision)
        {
            GameObject other = collision.gameObject;

            //THE PROJECTILE SHOULD NOT COLLIDE WITH THE PARENT OBJECT
            if (other.CompareTag(Parent.tag))
            {
                Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
                return;
            }

            //THE PROJECTILE SHOULD NOT COLLIDE WITH CONSUMABLES (CONSUMABLES SHOULD ONLY BE PICKED UP BY THE PLAYER)
            if (other.CompareTag("Consumable"))
            {
                Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
                return;
            }


            // SE O PROJ�TIL COLIDIR COM OUTRO PROJ�TIL, DUAS COISAS PODEM ACONTECER:
            // SE O PROJ�TIL COLIDIR COM UM PROJ�TIL DO MESMO PAI, IGNORAR A COLIS�O
            // Se o proj�til colidir com um proj�til de outro pai, TRY STOP ao outro proj�til;
            if (other.CompareTag("Projectile"))
            {   
                ProjectileBase collidingProjectile = other.GetComponent<ProjectileBase>();

                if (collidingProjectile.Parent == this.Parent)
                {
                    Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
                    return;
                }
                else
                {
                    if (collidingProjectile.TryStop(strenght))  //TODO: IMPLEMENTAR L�GICA ONDE, CASO O OUTRO OBJETO
                    {                                           // SEJA DESTRU�DO, O PROJ�TIL SEGUE SUA TRAJET�RIA NORMALMENTE,
                        return;                                 // CASO CONTR�RIO, SE O PROJ�TIL FOR PERFURANTE IGNORA A COLIS�O E SEGUE
                    };                                          // DIMINUINDO A VELOCIDADE DO OUTRO PROJ�TIL, CASO CONTR�RIO, O PROJ�TIL
                    return;                                     // REFLETE E MUDA DE DIRE��O.
                }
            }

            //Se o alvo for uma entidade que n�o o pai, aplique dano e
            // pare o proj�til. TODO: IMPLEMENTAR L�GICA DE PENETRA��O/REFLEX�O
            if (!other.CompareTag(Parent.tag))
            {
                if (!(other.GetComponent<IDamageable>() == null))
                {
                    other.GetComponent<IDamageable>().TakeDamage(damage);
                }
                GetStopped();
            }
        }
    }
}