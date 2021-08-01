using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Navigator), typeof(Sighter))]
public class HeavyMob : MonoBehaviour
{
    [Header("References")]
    [SerializeField]  Light _light;


    [Header("Settings")]
    [SerializeField] [Range(0.1f, 5)] float _patrolSpeed;
    [SerializeField] [Range(0.1f, 5)] float _pursueSpeed;
    [SerializeField] [Range(1f, 10)] float _maxTimeOutOfSight;

    Navigator _navigator;
    Sighter _sighter;

    MobState _currentState;

    Transform _target;

    float _timeOutOfSight;

    enum MobState
    {
        Patrol,
        Idle,
        Pursue

    }

    private void Awake()
    {
        _navigator = GetComponent<Navigator>();
        _sighter = GetComponent<Sighter>();

        _currentState = MobState.Patrol;
    }

    private void Start()
    {
        _navigator.Patrol(_patrolSpeed);
    }

    private void FixedUpdate()
    {
        switch (_currentState)
        {
            case MobState.Patrol:

                if (CheckForTarget())
                {
                    ChangeState(MobState.Pursue);
                }

                break;

            case MobState.Idle:
                if (CheckForTarget())
                {
                    ChangeState(MobState.Pursue);
                }
                break;

            case MobState.Pursue:
                if (CheckForTarget())
                {
                    _timeOutOfSight = 0;
                }
                else
                {
                    _timeOutOfSight += Time.fixedDeltaTime;

                    if (_timeOutOfSight >= _maxTimeOutOfSight)
                    {
                        ChangeState(MobState.Patrol);
                    }
                }

                break;

            default:
                break;
        }
    }

    bool CheckForTarget()
    {
        //Check if enemy target is on sight
        if (_sighter.IsAnyTargetOnSight())
        {
            _target = _sighter.GetTargetOnSight();
            return true;
        }
        else
        {
            return false;
        }
    }

    void ChangeState(MobState nextState)
    {
        switch (nextState)
        {
            case MobState.Patrol:
                _navigator.Patrol(_patrolSpeed);
                
                _light.color = Color.white;
                break;

            case MobState.Idle:
                break;


            case MobState.Pursue:
                _navigator.Pursue(_pursueSpeed, _target);
                _timeOutOfSight = 0;

                _light.color = Color.red;
                break;

            default:
                break;
        }

        _currentState = nextState;

    }

}
