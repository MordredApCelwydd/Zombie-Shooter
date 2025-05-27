using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private float range;
    [SerializeField] private int rpm;
    [SerializeField] private int magCapacity;
    [SerializeField] private float reloadTime;
    [SerializeField] private bool isFullAuto;
    [SerializeField] private int totalAmmo;

    [SerializeField] private ParticleSystem muzzleFlash;
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private Ammobar ammoBar;
    
    [SerializeField] private Camera playerCamera;

    private float _fireRate;
    private float _nextShot;
    private bool _isFiring;
    private bool _isReloading;
    private int _ammoRemaining;
    
    private Damageable _lastEnemyHit;
    private void Start()
    {
        _ammoRemaining = magCapacity;
        _isReloading = false;
        _isFiring = false;
        _nextShot = 0f;
        _fireRate = 60f / rpm;
        ammoBar.setMaximumAmmo(magCapacity);
    }

    private void Update()
    {
        if (isFullAuto ? Input.GetKey(KeyCode.Mouse0) : Input.GetKeyDown(KeyCode.Mouse0) && !_isReloading)
        {
            if (Time.time > _nextShot && _ammoRemaining != 0)
            {
                _nextShot = Time.time + _fireRate;
                _ammoRemaining--;
                _isFiring = true;
                ammoBar.FireABullet();
                muzzleFlash.Play();
            }
        }
        else if (Input.GetKeyDown(KeyCode.R) && totalAmmo > 0 && !_isReloading)
        {
            _isReloading = true; 
            StartCoroutine(Reloading());
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
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit, 10))
        {
            hitEffect.transform.position = hit.point;
            hitEffect.transform.LookAt(transform.position);
            hitEffect.Play();
            
            _lastEnemyHit = hit.transform.GetComponent<Damageable>();
            if (_lastEnemyHit != null)
            {
                _lastEnemyHit.TakeDamage(damage);
            }
        }
    }

    private IEnumerator Reloading()
    {
        yield return new WaitForSeconds(reloadTime);
        int ammoDiff = magCapacity - _ammoRemaining;
        if (totalAmmo < ammoDiff)
        {
            _ammoRemaining += totalAmmo;
            totalAmmo = 0;
        }
        else
        {
            _ammoRemaining += ammoDiff;
            totalAmmo -= ammoDiff;
        } 
        ammoBar.SetAmmo(_ammoRemaining);
        _isReloading = false;
        Debug.Log("_isReloading = false");
    }
}
