using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ammobar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    
    public void setMaximumAmmo(int ammo)
    {
        slider.maxValue = ammo;
        slider.value = ammo;
    }
    
    public void FireABullet()
    {
        slider.value--;
    }

    public void SetAmmo(int ammo)
    {
        slider.value = ammo;
    }
}
