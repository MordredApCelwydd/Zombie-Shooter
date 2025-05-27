using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient sliderColor;
    [SerializeField] private Image fill;
    
    [SerializeField] private Image healthFull;
    [SerializeField] private Image healthTwoThirds;
    [SerializeField] private Image healthOneThird;
    
    public void SetMaximumHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = sliderColor.Evaluate(1f);
        healthFull.enabled = true;
        healthTwoThirds.enabled = false;
        healthOneThird.enabled = false;
    }
    
    public void SetHealth(float health)
    {
        slider.value = health;
        
        fill.color = sliderColor.Evaluate(slider.normalizedValue);

        if (slider.normalizedValue > 0.6f)
        {
            healthFull.enabled = true;
            healthTwoThirds.enabled = false;
            healthOneThird.enabled = false;
        }
        else if (slider.normalizedValue > 0.3f)
        {
            healthFull.enabled = false;
            healthTwoThirds.enabled = true;
            healthOneThird.enabled = false;
        }
        else
        {
            healthFull.enabled = false;
            healthTwoThirds.enabled = false;
            healthOneThird.enabled = true;
        }
    }
}
