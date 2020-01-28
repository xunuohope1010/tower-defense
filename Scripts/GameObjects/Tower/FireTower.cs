using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTower : Tower
{
    [SerializeField] private float tickTime, tickDamage;

    public float TickTime { get { return tickTime; } }
    public float TickDamage { get { return tickDamage; } }

    private void Start()
    {
        ElementType = Element.FIRE;

        Upgrades = new TowerUpgrade[]
            {
                new TowerUpgrade(2,2,0.5f,5f,-0.2f,1),
                new TowerUpgrade(5,3,0.5f,5f,-0.2f,1)
            };
    }

    public override Debuff GetDebuff()
    {
        return new FireDebuff(Target, DebuffDuration, TickDamage, TickTime);
    }

    public override string GetStats()
    {
        if (NextUpgrade != null)    // if the next upgrade is available
        {
            return string.Format("<color=#ffa500ff><size=20><b>{0}</b></size></color>{1} \nTick Time: {2}sec <color=#00ff00ff>{4}sec</color> \nTick Damage: {3} <color=#00ff00ff>+{5}</color>", "Fire Tower", base.GetStats(), TickTime, TickDamage, NextUpgrade.TickTime, NextUpgrade.SpecialDamage);
        }

        // if not
        return string.Format("<color=#ffa500ff><size=20><b>{0}</b></size></color>{1} \nTick Time: {2}sec \nTick Damage: {3}", "Fire Tower (Maxed Out)", base.GetStats(), TickTime, TickDamage);
    }

    public override void Upgrade()
    {
        tickTime += NextUpgrade.TickTime;
        tickDamage += NextUpgrade.SpecialDamage;
        base.Upgrade(); 
    }
}