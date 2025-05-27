using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyDamageable : Damageable
{
    [SerializeField] private string type;
    public string Type => type;
    
    public override string Name => "enemy";
    public override void TakeDamage(float damage)
    {
        Debug.Log("Some additional damage functional, i.e. calculating damage resistance");
        base.TakeDamage(damage);
    }
    protected override void Die()
    {
        gameObject.SetActive(false);
    }
}
