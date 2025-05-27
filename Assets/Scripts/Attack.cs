using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    [SerializeField] private float attackStrength;

    private bool _isScaled;
    
    private Transform _target;

    private Damageable _lastTargetHit;
    private void Start()
    {
        _isScaled = false;
        _target = PlayerSingleton.Instance.transform;
        _lastTargetHit = _target.GetComponent<Damageable>();
    }

    public virtual void AttackHit()
    {
        _lastTargetHit.TakeDamage(attackStrength);
    }
    
    public virtual void AttackHitEvent()
    {
        _lastTargetHit.TakeDamage(attackStrength);
    }

    public virtual void InitScaling(float scalingValue)
    {
        if (!_isScaled)
        {
            _isScaled = true;
            attackStrength *= scalingValue;
        }
    }
}
