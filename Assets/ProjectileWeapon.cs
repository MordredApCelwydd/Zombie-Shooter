using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : MonoBehaviour
{
    [SerializeField] private float range;
    [SerializeField] private float manaCapacity;
    [SerializeField] private int rpm; //60 for 1 shot/sec
    
    //[SerializeField] private ParticleSystem muzzleFlash;
    //[SerializeField] private ParticleSystem hitEffect;
    //[SerializeField] private Ammobar ammoBar;
    
    [SerializeField] private Camera playerCamera;
    
    [SerializeField] private GameObject projectile;
    [SerializeField] private int poolDefaultSize;
    [SerializeField] private bool isPoolExpandable;
    [SerializeField] private float projectileSpeed;
    
    private ObjectPool<MonoBehaviour> _projectiles;
    private Vector3 _projectileDestination;
    
    private float _fireRate;
    private float _nextShot;
    private bool _isFiring;
    
    private void Start()
    {
        _nextShot = 0f;
        _fireRate = 60f/ rpm;
        _isFiring = false;
        
        _projectiles = new ObjectPool<MonoBehaviour>(projectile.GetComponent<Projectile>(), isPoolExpandable,
            poolDefaultSize);
        _projectileDestination = new Vector3();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Mouse2))
        {
            if (Time.time > _nextShot)
            {
                _nextShot = Time.time + _fireRate;
                _isFiring = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (_isFiring)
        {
            _isFiring = false;
            Shoot();
        }
    }

    private void Shoot()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            _projectileDestination = hit.point;
        }
        else
        {
            _projectileDestination = ray.GetPoint(range);
        }
        
        MonoBehaviour proj= _projectiles.GetElement(transform.position);
        proj.GetComponent<Rigidbody>().velocity = (_projectileDestination - transform.position).normalized * projectileSpeed;
    }
}