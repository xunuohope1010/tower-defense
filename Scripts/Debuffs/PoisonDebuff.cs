﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDebuff : Debuff
{
    private float tickTime, timeSinceTick;

    private PoisonSplash splashPrefab;
    private int splashDamage;

    public PoisonDebuff(Monster target, int splashDamage, float tickTime, PoisonSplash splashPrefab, float duration) : base(target,duration)
    {
        this.splashDamage = splashDamage;
        this.tickTime = tickTime;
        this.splashPrefab = splashPrefab;
    }

    public override void Update()
    {
        if (target != null)
        {
            timeSinceTick += Time.deltaTime;

            if (timeSinceTick >= tickTime)
            {
                timeSinceTick = 0;
                Splash();
            }
        }

        base.Update();
    }

    private void Splash()
    {
        PoisonSplash tmp = GameObject.Instantiate(splashPrefab, target.transform.position, Quaternion.identity);
        tmp.Damage = splashDamage;

        Physics2D.IgnoreCollision(target.GetComponent<Collider2D>(), tmp.GetComponent<Collider2D>());
    }
}
