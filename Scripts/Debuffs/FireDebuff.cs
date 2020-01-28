using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDebuff : Debuff
{
    private float tickTime, timeSinceTick, tickDamage;

    public FireDebuff(Monster target, float duration, float tickDamage, float tickTime) : base(target, duration)
    {
        this.tickDamage = tickDamage;
        this.tickTime = tickTime;
    }

    public override void Update()
    {
        if (target != null)
        {
            timeSinceTick += Time.deltaTime;

            if (timeSinceTick >= tickTime)
            {
                timeSinceTick = 0;
                target.TakeDamage(tickDamage, Element.FIRE);
            }
        }

        base.Update();
    }
}
