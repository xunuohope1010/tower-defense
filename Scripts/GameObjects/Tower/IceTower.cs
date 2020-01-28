using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTower : Tower
{
    [SerializeField] private float slowingFactor;
    public float SlowingFactor { get { return slowingFactor; } }

    private void Start()
    {
        ElementType = Element.ICE;

        Upgrades = new TowerUpgrade[]
            {
                new TowerUpgrade(2,1,1,2,10f),
                new TowerUpgrade(2,1,1,2,20f)
            };
    }

    public override Debuff GetDebuff()
    {
        return new IceDebuff(Target, slowingFactor, DebuffDuration);
    }

    public override string GetStats()
    {
        if (NextUpgrade != null)  //if the next upgrade is available
        {
            return string.Format("<color=#00ffffff><size=20><b>{0}</b></size></color>{1} \nSlowing factor: {2}% <color=#00ff00ff>+{3}%</color>", "Ice Tower", base.GetStats(), SlowingFactor, NextUpgrade.SlowingFactor);
        }

        //if not
        return string.Format("<color=#00ffffff><size=20><b>{0}</b></size></color>{1} \nSlowing factor: {2}%", "Ice Tower (Maxed Out)", base.GetStats(), SlowingFactor);
    }

    public override void Upgrade()
    {
        slowingFactor += NextUpgrade.SlowingFactor;
        base.Upgrade();
    }
}