using Jili.StatSystem.EntityTree;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Jili.StatSystem.AttackSystem
{
    // APLICAR F�RMULAS ESPECIFICAS DE ARMAS NO RETURN DE CADA ARMA � UMA BOA IDEIA? 
    // PARECE QUE SERIA MAIS PR�TICO TER UMA CLASSE EST�TICA PARA C�LCULO DE F�RMULAS, 
    // TALVEZ IMPLEMENTE DESSA FORMA NO FUTURO;

    public class JinxMinigun : IShootable
    {
        // CONSTANTES DE CONFIGURA��O DA ARMA
        private readonly int BaseProjectiles        = 3;                            // TR�S PROJ�TEIS
        private readonly int BaseProjectileSpeed    = 5;                            // VELOCIDADE BASE DE 5
        private readonly int TriggerSpeedFactor     = 6;                            // DIVIDE PELO COOLDOWN ( X / 6 = 0,166x) A CADA DISPARO
        private readonly int MaxProjectileDuration  = 5;                            // DURA��O M�XIMA DE 5 SEGUNDOS
        private readonly float OffsetValue          = 0.15f;                         // DISPERS�O DE 0.15 UNIDADES
        protected readonly WeaponTypes Type         = WeaponTypes.JinxMinigun;      // TIPO DE ARMA

        //projectile damage
        private float _damage;
        protected float Damage 
        { 
            get {
                if (isDirty && DirtyStat.Contains(Player.AttackDamage))
                {
                    //Debug.Log("LOADING BULLET DAMAGE");
                    _damage = Player.AttackDamage.ReadValue();
                    DirtyStat.Remove(Player.AttackDamage);
                    ReadDirtiness();
                }
                return _damage / 2f; // o dano do proj�til � 50% do dano do jogador
            }
        }

        //attack cooldown
        private float _cooldown;
        protected float Cooldown 
        {
            get
            {
                if (isDirty && DirtyStat.Contains(Player.AttacksPerSecond))
                {
                    _cooldown = Player.AttacksPerSecond.ReadValue();
                    DirtyStat.Remove(Player.AttacksPerSecond);
                    ReadDirtiness();
                }
                return 1 / _cooldown; // if dex = 0 && finesse = 0, attacks per second = 0.333, para fazer o jogador atacar uma vez a cada 3 segundos
                                      // a f�rmula precisa ser ajustada como 1 / 0.333 ~= 3 segundos (?)
            }
        }
        protected float CooldownTimer;

        // projectile range
        private float _range;
        protected float Range
        {
            get
            {
                if (isDirty && DirtyStat.Contains(Player.AttackRange))
                {
                    _range = Player.AttackRange.ReadValue();
                    DirtyStat.Remove(Player.AttackRange);
                    ReadDirtiness();
                }
                return _range;
            }
        }

        // projectile speed
        private float _projectileSpeed;
        protected float ProjectileSpeed
        {
            get
            {
                if (isDirty && DirtyStat.Contains(Player.ProjectileSpeed))
                {
                    _projectileSpeed = Player.ProjectileSpeed.ReadValue();
                    DirtyStat.Remove(Player.ProjectileSpeed);
                    ReadDirtiness();
                }
                return _projectileSpeed + BaseProjectileSpeed;
            }
        }

        // number of projectiles
        private float   _projectileNumber;        // n�mero de proj�teis do jogador
        protected float ProjectileNumber
        {
            get
            {
                if (isDirty && DirtyStat.Contains(Player.ProjectileNumber))
                {
                    _projectileNumber = Player.ProjectileNumber.ReadValue();
                    DirtyStat.Remove(Player.ProjectileNumber);
                    ReadDirtiness();
                }
                return _projectileNumber + BaseProjectiles;
            }
        }

        protected float TriggerSpeed { get; set; }
        protected List<Stat> DirtyStat;
        protected Boolean isDirty = true;
        protected GameObject Projectile;
        protected PlayerIdentity Player;
        protected Transform PlayerTransform;

        public JinxMinigun(GameObject projectile, PlayerIdentity player)
        {
            this.Projectile = projectile;
            this.Player = player.GetComponent<PlayerIdentity>();
            this.PlayerTransform = player.transform;
            this.DirtyStat = new List<Stat>();

            // Inicia os ouvintes para altera��es em cada stat relevante
            Player.AttackDamage.OnValueChanged      += BecomeDirty;
            Player.AttacksPerSecond.OnValueChanged  += BecomeDirty;
            Player.AttackRange.OnValueChanged       += BecomeDirty;
            Player.ProjectileNumber.OnValueChanged  += BecomeDirty;
            Player.ProjectileSpeed.OnValueChanged   += BecomeDirty;

            // Adiciona os stats � lista de DirtyStat
            DirtyStat.Add(Player.AttackDamage);
            DirtyStat.Add(Player.AttacksPerSecond);
            DirtyStat.Add(Player.AttackRange);
            DirtyStat.Add(Player.ProjectileNumber);
            DirtyStat.Add(Player.ProjectileSpeed);

            this.CooldownTimer = this.Cooldown;
            this.TriggerSpeed = (this.Cooldown / TriggerSpeedFactor) / this.ProjectileNumber; // A VELOCIDADE DE GATILHO � 20% DO COOLDOWN DIVIDIDO ENTRE O TOTAL DE PROJ�TEIS A SEREM DISPARADOS
        }

        public JinxMinigun(PlayerIdentity player) : this(GameObject.FindGameObjectWithTag("ProjectileManager").GetComponent<ProjectileManager>().JinxBullet, player) { }

        // TODO: implementar l�gica de recarregar os valores dos status relevantes conforme necess�rio
        // caso o jogador tenha um item que aumente o dano, por exemplo, o dano do proj�til deve ser recalculado
        // caso o jogador suba de n�vel e aumente qualquer um dos status relevantes, as vari�veis associadas a esse status
        // devem ser recalculadas
        public void BecomeDirty(Stat stat) 
        {
            //Debug.Log("Sujando..." + stat.Type);
            isDirty = true;
            if (DirtyStat != null )
            {
                if (!DirtyStat.Contains(stat))
                {
                    //Debug.Log("adicionado aa lista de sujeira: " + stat.Type);
                    DirtyStat.Add(stat);
                }
            }
            else
            {
                //Debug.Log("criando lista de sujeira e adicionando..." + stat.Type);
                DirtyStat = new List<Stat> { stat };
            }
        }

        public void ReadDirtiness()
        {
            if (DirtyStat.Count > 0)
            {
                isDirty = true;
            }
            else
            {
                //Debug.Log("JINX MINIGUN NAO EST� MAIS SUJO");
                isDirty = false;
            }
        }

        public float ReturnStatValueByType(StatType type)
        {
            float value = 0f;
            switch (type)
            {
                case StatType.AttackDamage:
                    value = Damage;
                    break;
                case StatType.AttackRange:
                    value = Range;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            return value;
        }

        public bool TryShoot(float deltaTime)
        {
            //OPERA O COOLDOWN
            CooldownTimer -= deltaTime;

            //SE O COOLDOWN ESTIVER ZERADO, ATIRA
            if (CooldownTimer <= 0)
            {
                CooldownTimer = Cooldown; // ZERA O COOLDOWN NOVAMENTE
                //Debug.Log("AIMING PROJECTILES");
                Player.StartCoroutine(AimProjectiles()); // INICIA A CORROTINA DE MIRA
                return true;
            }
            return false;
        }

        public IEnumerator AimProjectiles()
        {
            //calcula onde o mouse est� mirando
            Vector3 shootPos = PlayerTransform.position;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            Vector2 direction = (mousePosition - shootPos).normalized;

            //atira na dire��o calculada, uma vez para cada proj�til dispon�vel, considerando o tempo de gatilho
            for (int i = 0; i < ProjectileNumber; i++)
            {
                Shoot(direction);
                yield return new WaitForSeconds(TriggerSpeed);
            }
        }

        public void Shoot(Vector2 direction)
        {
            //adiciona um pequeno spread para o disparo
            float offset = OffsetValue;
            float offsetX = Random.Range(-offset, offset);
            float offsetY = Random.Range(-offset, offset);
            Vector3 spawnPosition = PlayerTransform.position + new Vector3(offsetX, offsetY, 0);


            //instancia e configura o proj�til
            GameObject NewProjectile = UnityEngine.Object.Instantiate(Projectile, spawnPosition, Quaternion.identity);
            ProjectileBase projectile = NewProjectile.GetComponent<ProjectileBase>();
            if (projectile != null)
            {
                projectile.Parent = Player.gameObject;
                projectile.source = this;
                Collider2D parentCollider = projectile.Parent.GetComponent<Collider2D>();
                Collider2D projectileCollider = NewProjectile.GetComponent<Collider2D>();
                if (parentCollider != null && projectileCollider != null)
                {
                    Physics2D.IgnoreCollision(projectileCollider, parentCollider);
                }
            }

            //aplica a velocidade ao proj�til   
            Rigidbody2D rb = NewProjectile.GetComponent<Rigidbody2D>();
            rb.velocity = direction.normalized * ProjectileSpeed;
        
            //destr�i o proj�til ap�s 5 segundos
            UnityEngine.Object.Destroy(NewProjectile, MaxProjectileDuration);
        }
    }
}
