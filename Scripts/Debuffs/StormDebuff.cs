using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StormDebuff : Debuff
{
    public StormDebuff(Monster target, float duration) : base(target,duration)
    {
        
    }

    public override void Update()
    {
        if (target != null)
        {
            target.Speed = 0;
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
