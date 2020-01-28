﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarScript : MonoBehaviour
{
    private float fillAmount;

    [SerializeField] private float lerpSpeed;

    [SerializeField] private Image content;
    [SerializeField] private Text valueText;
    [SerializeField] private Color fullColor, lowColor;
    [SerializeField] private bool lerpColors;

    public float MaxValue { get; set; }
    public float Value { set { fillAmount = Map(value, 0, MaxValue, 0, 1); } } 

    // Use this for initialization
    void Start()
    {
		if (lerpColors)
        {
            content.color = fullColor;
        }
	}
	
	// Update is called once per frame
	void Update()
    {
        HandleBar();
    }

    private void HandleBar()
    {
        if (fillAmount != content.fillAmount)
        {
            content.fillAmount = Mathf.Lerp(content.fillAmount, fillAmount, lerpSpeed * Time.deltaTime);
        }

        if (lerpColors)
        {
            content.color = Color.Lerp(lowColor, fullColor, fillAmount);
        }
    }

    private float Map(float value, float inMin, float inMax, float outMin, float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    public void Reset()
    {
        Value = MaxValue;
        content.fillAmount = 1;
    }
}
