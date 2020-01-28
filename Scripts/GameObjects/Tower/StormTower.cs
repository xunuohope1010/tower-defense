using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormTower : Tower
{
    private void Start()
    {
        ElementType = Element.STORM;

        Upgrades = new TowerUpgrade[]
            {
                new TowerUpgrade(2,2,1,5),
                new TowerUpgrade(5,3,1,5)
            };
    }

    public override Debuff GetDebuff()
    {
        return new StormDebuff(Target, DebuffDuration);
    }

    public override string GetStats()
    {
        if (NextUpgrade != null)    // if the next upgrade is available
        {
            return string.Format("<color=#add8e6ff><size=20><b>{0}</b></size></color>{1}", "Storm Tower", base.GetStats());
        }

        // if not
        return string.Format("<color=#add8e6ff><size=20><b>{0}</b></size></color>{1}", "Storm Tower (Maxed Out)", base.GetStats());
    }
}
