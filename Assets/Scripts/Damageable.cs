using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Damageable : MonoBehaviour
{
    [field: SerializeField] protected float Health { get; private set; }
    private float _startingHealth;
    
    private bool _isDead;
    public abstract string Name { get; }

    private void Awake()
    {
        _startingHealth = Health;
        _isDead = false;
    }
    
    protected void OnEnable()
    {
        Health = _startingHealth;
        _isDead = false;
    }
    
    public virtual void TakeDamage(float damage)
    {
        if (!_isDead)
        {
            Health -= damage;
            if (Health <= 0)
            {
                _isDead = true;
                Die();
            }
        }
    }
    
    protected abstract void Die();
}
