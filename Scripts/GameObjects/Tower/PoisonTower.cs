using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonTower : Tower
{
    [SerializeField] private float tickTime;
    public float TickTime { get { return tickTime; } }

    [SerializeField] private PoisonSplash splashPrefab;

    [SerializeField] private int splashDamage;
    public int SplashDamage { get { return splashDamage; } }

    private void Start()
    {
        ElementType = Element.POISON;

        Upgrades = new TowerUpgrade[]
            {
                new TowerUpgrade(2,1,0.5f,5,-0.2f,1),
                new TowerUpgrade(5,1,0.5f,5,-0.2f,1),
            };
    }

    public override Debuff GetDebuff()
    {
        return new PoisonDebuff(Target, SplashDamage, TickTime, splashPrefab, DebuffDuration);
    }

    public override string GetStats()
    {
        if (NextUpgrade != null) // if the next upgrade is available
        {
            return string.Format("<color=#00ff00ff><size=20><b>{0}</b></size></color>{1} \nTick time: {2}sec <color=#00ff00ff>{4}sec</color> \nSplash damage: {3} <color=#00ff00ff>+{5}</color>", "Poison Tower", base.GetStats(), TickTime, SplashDamage, NextUpgrade.TickTime, NextUpgrade.SpecialDamage);
        }

        // if not
        return string.Format("<color=#00ff00ff><size=20><b>{0}</b></size></color>{1} \nTick time: {2}sec \nSplash damage: {3}", "Poison Tower (Maxed Out)", base.GetStats(), TickTime, SplashDamage);
    }

    public override void Upgrade()
    {
        splashDamage += NextUpgrade.SpecialDamage;
        tickTime += NextUpgrade.TickTime;
        base.Upgrade();
    }
}
