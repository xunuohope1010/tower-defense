using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]  // so that Stat is visible in the inspector even though it is not a Monobehavior
public class Stat
{
    [SerializeField] private BarScript bar;
    [SerializeField] private float maxVal, currentVal;

    public float MaxVal
    {
        get { return maxVal; }

        set
        {
            maxVal = value;
            Bar.MaxValue = maxVal;
        }
    }

    // so that the bar can reflect changes
    public void Initialize()
    {
        MaxVal = maxVal;
        CurrentVal = currentVal;
    }

    public float CurrentVal
    {
        get { return currentVal; }
        set
        {
            currentVal = Mathf.Clamp(value, 0, MaxVal);
            Bar.Value = currentVal;
        }
    }

    public BarScript Bar
    {
        get
        {
            return bar;
        }
    }
}