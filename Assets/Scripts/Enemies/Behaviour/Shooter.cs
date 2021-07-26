using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("References")]
    [SerializeReference] Transform _spawn;
    [SerializeReference] GameObject _projectilePrefab;

    [Header("Settings")]
    [SerializeReference] [Range(1, 10)] int _numberOfBullets;
    [SerializeReference] [Range(0.1f, 1)] float _bulletDelay;
    [SerializeReference] [Range(1, 20)] float _bulletSpeed;
    [SerializeReference] [Range(1, 20)] float _bulletDamage;

    Projectile[] _projectiles;
    int _projectileCounter;
    Coroutine _shootingCoroutine;

    /// <summary>
    /// Action invoked when shooter STARTS shooting
    /// </summary>
    public Action StartAction;
    /// <summary>
    /// Action invoked every shoot
    /// </summary>
    public Action ShootAction;
    /// <summary>
    /// Action invoked when shooter STOPS shooting
    /// </summary>
    public Action StopAction;


    private void Awake()
    {
        if (_projectilePrefab.GetComponent<Projectile>() != null)
        {
            _projectiles = new Projectile[_numberOfBullets];

            for (int i = 0; i < _projectiles.Length; i++)
            {
                _projectiles[i] = Instantiate(_projectilePrefab).GetComponent<Projectile>();
                _projectiles[i].name = name + "'s Bullet " + i;
            }
        }
        else
        {
            Debug.Log("Projectile Prefab has no Projectile Component");
        }

    }

    /// <summary>
    /// Shoot Projectiles to target. Direction gets updated on every Shoot
    /// </summary>
    /// <param name="target"></param>
    public void ShootToTarget(Transform target)
    {
        StartCoroutine(ShootProjectilesToTarget(target));
    }

    /// <summary>
    /// Shoot Projectiles in direction. 
    /// </summary>
    /// <param name="direction"></param>
    public void ShootInDirection(Vector3 direction)
    {
        if (_shootingCoroutine != null) {
            StopCoroutine(_shootingCoroutine);
        }
        _shootingCoroutine = StartCoroutine(ShootProjectilesInDirection(direction));

    }

    //Shoot Projectiles, updating direction on every shoot
    IEnumerator ShootProjectilesToTarget(Transform target)
    {
        StartAction.Invoke();

        for (int i = 0; i < _projectiles.Length; i++)
        {
            ShootProjectile(target.position - _spawn.transform.position);
            yield return new WaitForSeconds(_bulletDelay);
        }

        if (StopAction != null)
            StopAction.Invoke();
    }

    //Shoot projectiles in one direction
    IEnumerator ShootProjectilesInDirection(Vector3 direction)
    {
        if (StartAction != null)
            StartAction.Invoke();

        for (int i = 0; i < _projectiles.Length; i++)
        {
            ShootProjectile(direction);
            yield return new WaitForSeconds(_bulletDelay);
        }

        if (StopAction != null)
            StopAction.Invoke();
    }

    void ShootProjectile(Vector3 direction)
    {
        //Get first projectile if end of array
        if (_projectileCounter >= _projectiles.Length)
        {
            _projectileCounter = 0;
        }

        //Shoot next projectile in array
        _projectiles[_projectileCounter].Launch(_spawn.position, direction.normalized * _bulletSpeed, _bulletDamage, 0);

        //Set next projectile to be shot
        _projectileCounter++;

        if (ShootAction != null)
            ShootAction.Invoke();
    }
}
