using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Navigator), typeof(Sighter), typeof(Melee))]
[RequireComponent(typeof(Health))]
public class HeavyMob : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Light _light;


    [Header("Settings")]
    [SerializeField] [Range(0.1f, 5)] float _patrolSpeed;
    [SerializeField] [Range(0.1f, 5)] float _pursueSpeed;
    [SerializeField] [Range(1f, 10)] float _maxTimeOutOfSight;

    [Header("Attack")]
    [SerializeField] [Range(1f, 10)] float _meleeRange;
    [SerializeField] [Range(1f, 10)] float _meleeDamage;
    [SerializeField] [Range(1f, 10)] float _meleeDuration;
    [SerializeField] [Range(1f, 10)] float _meleePush;
    [SerializeField] [Range(1f, 10)] float _timeToRecover;





    Navigator _navigator;
    Sighter _sighter;
    Melee _melee;
    Health _health;

    MobState _currentState;

    Transform _target;

    float _timeOutOfSight;

    enum MobState
    {
        Patrol,
        Idle,
        Pursue,
        AttackIdle,
        Stunned,
        CheckPlace

    }

    private void Awake()
    {
        _navigator = GetComponent<Navigator>();
        _sighter = GetComponent<Sighter>();

        _melee = GetComponent<Melee>();
        _melee.hitAction += TargetHit;
        _melee.endOfAttackAction += EndAttackIdle;

        _health = GetComponent<Health>();
        _health.DeadAction += Die;
        _health.HurtAction += Hurt;

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

                if (Vector3.Distance(transform.position, _target.position) <= _meleeRange)
                {
                    _melee.Attack(_meleeDamage, _meleePush, _meleeDuration);
                }

                break;

            case MobState.AttackIdle:
                break;

            case MobState.Stunned:
                break;

            default:
                Debug.LogError("No such State!");
                break;
        }
    }

    void TargetHit()
    {

        ChangeState(MobState.AttackIdle);
    }

    void EndAttackIdle()
    {
        if (_currentState == MobState.AttackIdle)
        {
            ChangeState(MobState.Pursue);
        }
    }

    bool CheckForTarget()
    {
        //Check if enemy target is on sight
        if (_sighter.IsAnyTargetOnSight())
        {
            var trigger = _sighter.GetTargetOnSight();

            if (trigger.typeOfTrigger == SightTrigger.TypeOfTrigger.Player)
            {
                _target = trigger.transform;
                return true;
            }

        }

        return false;

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
                _navigator.Stop();
                break;


            case MobState.Pursue:
                _navigator.Pursue(_pursueSpeed, _target);
                _timeOutOfSight = 0;

                _light.color = Color.red;
                break;

            case MobState.AttackIdle:
                _navigator.Stop();

                break;

            case MobState.Stunned:
                _navigator.Stop();

                break;
            default:
                Debug.LogError("No such State!");
                break;
        }

        _currentState = nextState;

    }

    void Die(Vector3 source, float push, Transform hitter)
    {
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
            enabled = false;

            _light.enabled = false;
            _navigator.Stop();

        }
    }

    void Hurt(Vector3 source, float push, Transform hitter)
    {
        if (_currentState != MobState.Pursue && _currentState != MobState.AttackIdle)
        {
            Stun(source, push, hitter);
        }
        else
        {
            _target = hitter;
            ChangeState(MobState.Pursue);
        }

    }

    void Stun(Vector3 hitSource, float push, Transform hitter)
    {
        ChangeState(MobState.Stunned);

        var angles = transform.eulerAngles;

        StartCoroutine(RecoverFromStun(_timeToRecover, angles, hitSource, hitter));

        angles.z = 0;

        transform.eulerAngles = angles;

    }

    IEnumerator RecoverFromStun(float timeToRecover, Vector3 angles, Vector3 source, Transform hitter)
    {
        yield return new WaitForSeconds(timeToRecover);

        _target = hitter;

        transform.eulerAngles = angles;
        ChangeState(MobState.Pursue);
    }

    public void GoCheckPlace(Vector3 position)
    {
        ChangeState(MobState.CheckPlace);


    }




}
