using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jili.StatSystem;

public class PlayerIdentity : EntityBase
{
    public Attribute Strength;
    public Attribute Constitution;
    public Attribute Agility;
    public Stat AttackDamage;
    public Stat Health;

    public float somadeataques = 0;
    public float somadestr = 0;
    void Awake()
    {
        attListAdd(Constitution);
        attListAdd(Strength);
        AttackDamage  = new Stat(StatType.AttackDamage, attlist);
        Health = new Stat(StatType.Health, attlist);
    }

    void Start()
    {

    }

    void Update()
    {
        somadeataques += AttackDamage.Value/100000;
        somadestr += Strength.Value/100000;
    }
}
