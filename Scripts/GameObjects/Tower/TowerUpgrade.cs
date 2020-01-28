using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUpgrade
{
    public int Price { get; private set; }
    public int Damage { get; private set; }
    public float DebuffDuration { get; private set; }
    public float ProcChance { get; private set; }
    public float SlowingFactor { get; private set; }
    public float TickTime { get; private set; }
    public int SpecialDamage { get; private set; }

    // upgrade constructor for the storm tower
    public TowerUpgrade(int price, int damage, float debuffDuration, float procChance)
    {
        Price = price;
        Damage = damage;
        DebuffDuration = debuffDuration;
        ProcChance = procChance;
    }

    // upgrade constructor for the ice tower
    public TowerUpgrade(int price, int damage, float debuffDuration, float procChance, float slowingFactor)
    {
        Price = price;
        Damage = damage;
        DebuffDuration = debuffDuration;
        ProcChance = procChance;
        SlowingFactor = slowingFactor;
    }

    // upgrade constructor for the fire and poison tower
    public TowerUpgrade(int price, int damage, float debuffDuration, float procChance, float tickTime, int specialDamage)
    {
        Price = price;
        Damage = damage;
        DebuffDuration = debuffDuration;
        ProcChance = procChance;
        TickTime = tickTime;
        SpecialDamage = specialDamage;
    }
}
