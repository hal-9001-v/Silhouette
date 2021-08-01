using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Shooter), typeof(Patroller), typeof(Sighter))]
public class Tank : Enemy
{
    [Header("References")]
    [SerializeField] Light _spotLight;
    [SerializeField] Animator _animator;

    [Space(5)]
    [SerializeField] Transform _head;

    [Header("Settings")]
    [SerializeField] TankState _currentState;
    [SerializeField] Color _shootingLightColor;

    [Header("Shooting")]
    [SerializeField] [Range(0.1f, 90)] float _degreesPerSecond = 0.5f;
    [SerializeField] [Range(0.1f, 5)] float _timeAfterShoot = 2;

    Shooter _shooter;
    Patroller _patroller;
    Sighter _sighter;


    enum TankState
    {
        Patrolling,
        Aiming,
        Shooting,
        Idle
    }

    //Aiming State Variables
    float _aimTimeCounter;
    float _aimTotalTime;
    Vector3 _aimForward;
    Vector3 _projectileDirection;
    Vector3 _aimStartingForward;

    Color _startingLightColor;

    private void Start()
    {
        _patroller = GetComponent<Patroller>();

        if (_spotLight != null)
        {
            _startingLightColor = _spotLight.color;

            _shooter = GetComponent<Shooter>();
            _sighter = GetComponent<Sighter>();
        }

        _currentState = TankState.Patrolling;
    }

    private void FixedUpdate()
    {

        switch (_currentState)
        {
            case TankState.Patrolling:
                Transform target = _sighter.GetTargetOnSight();

                //Check If player is in front. If so, calculate aiming time and direction for next State, which is Aiming
                if (target != null)
                {
                    //Direction
                    _aimStartingForward = _head.transform.forward;
                    _aimForward = target.position- _head.transform.position;
                    _projectileDirection = _aimForward.normalized;
                    _aimForward.y = 0;
                    _aimForward.Normalize();

                    //Time
                    _aimTotalTime = Vector3.Angle(_aimStartingForward, _aimForward) / _degreesPerSecond;
                    _aimTimeCounter = 0;

                    _animator.enabled = false;
                    _spotLight.color = _shootingLightColor;
                    _currentState = TankState.Aiming;
                }
                else
                {
                    _patroller.ApplyPatrol();
                }
                break;

            case TankState.Aiming:

                //Move Barrel Towards player
                if (_aimTimeCounter < _aimTotalTime)
                {
                    _aimTimeCounter += Time.deltaTime;

                    _head.forward = Vector3.Lerp(_aimStartingForward, _aimForward, _aimTimeCounter / _aimTotalTime);

                }
                else
                {
                    _currentState = TankState.Shooting;
                }


                break;

            case TankState.Shooting:
                //Shoot Projectile
                ShootPlayer();

                //Countdown to get back to Patrol State
                CountDownToAction(_timeAfterShoot, () =>
                {
                    _currentState = TankState.Patrolling;
                    _spotLight.color = _startingLightColor;
                    _animator.enabled = true;
                });

                //Get Inactive after shooting with Idle State
                _currentState = TankState.Idle;

                break;


            case TankState.Idle:
                //Inactive
                break;
        }


    }

    void ShootPlayer()
    {
        if (_shooter != null)
        {
            _shooter.ShootInDirection(_projectileDirection);
            //Debug.Log("Shoot");
        }

    }


}
