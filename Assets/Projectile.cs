using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float lifeSpan;
    private bool _isDying;

    private void OnEnable()
    {
        _isDying = false;
    }

    private void Update()
    {
        if (!_isDying)
        {
            _isDying = true;
            StartCoroutine(LifeTimer());
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Damageable damageable = other.gameObject.GetComponent<Damageable>();
        if (damageable)
        {
            damageable.TakeDamage(damage);
            gameObject.SetActive(false);
        }
    }

    private IEnumerator LifeTimer()
    {
        yield return new WaitForSeconds(lifeSpan);
        gameObject.SetActive(false);
    }
}
