﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceDebuff : Debuff
{
    private float slowingFactor;

    private bool applied;

    public IceDebuff(Monster target, float slowingFactor, float duration) : base(target,duration)
    {
        this.slowingFactor = slowingFactor;
    }

    public override void Update()
    {
        if (target != null)
        {
            if (!applied)
            {
                applied = true;
                target.Speed -= (target.MaxSpeed * slowingFactor) / 100;
            }
        }

        base.Update();
    }

    public override void Remove()
    {
        if (target != null)
        {
            target.Speed = target.MaxSpeed;
            base.Remove();
        }
    }
}
