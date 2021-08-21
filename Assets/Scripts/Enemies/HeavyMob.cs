using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Navigator), typeof(Sighter), typeof(Melee))]
[RequireComponent(typeof(Health), typeof(Listener), typeof(CharacterBodyRotation))]
[RequireComponent(typeof(Rigidbody))]
public class HeavyMob : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Light _light;

    [Header("Settings")]
    [SerializeField] [Range(0.1f, 5)] float _patrolSpeed;
    [SerializeField] [Range(0.1f, 5)] float _pursueSpeed;
    [SerializeField] [Range(1f, 10)] float _maxTimeOutOfSight;

    [SerializeField] Color _patrolColor = Color.blue;
    [SerializeField] Color _distressedColor = Color.yellow;
    [SerializeField] Color _fightColor = Color.red;

    [Header("Attack")]
    [SerializeField] [Range(1f, 10)] float _meleeRange;
    [SerializeField] [Range(1f, 10)] float _meleeDamage;
    [SerializeField] [Range(1f, 10)] float _meleeDuration;
    [SerializeField] [Range(0.1f, 2f)] float _idleMeleeDuration;
    [SerializeField] [Range(1f, 10)] float _meleePush;
    [SerializeField] [Range(1f, 10)] float _timeToRecover;
    [SerializeField] [Range(1f, 10)] float _timeCheckingPlace;

    Navigator _navigator;
    Sighter _sighter;
    Melee _melee;
    Health _health;
    Listener _listener;
    Rigidbody _rigidbody;

    public bool isFighting
    {
        get
        {
            return _currentState == MobState.Pursue || _currentState == MobState.AttackIdle || _currentState == MobState.Attack;
        }
    }
    public Listener listener
    {
        get
        {
            return _listener;
        }
    }

    CharacterBodyRotation _characterBodyRotation;

    MobState _currentState;

    Transform _target;

    //Stun State
    Vector3 _stunDirection;

    //Check Place State
    Vector3 _checkPlace;
    float _checkTime;

    Timer _timer;

    public bool avaliableForPatrol { get; private set; }

    [SerializeField] TypeOfMob _typeOfMob;

    enum TypeOfMob
    {
        heavy,
        light
    }

    enum MobState
    {
        Patrol,
        Idle,
        Pursue,

        Attack,
        AttackIdle,

        Stunned,

        CheckPlace,
        CheckPlaceIdle,
        Dead

    }

    private void Awake()
    {
        _navigator = GetComponent<Navigator>();

        _sighter = GetComponent<Sighter>();

        _melee = GetComponent<Melee>();
        _melee.hitAction += TargetHit;
        _melee.endOfAttackAction += EndAttack;

        _health = GetComponent<Health>();
        _health.DeadAction += Die;
        _health.HurtAction += Hurt;


        _listener = GetComponent<Listener>();
        _listener.hearedNoiseAction += NoiseHeared;

        _rigidbody = GetComponent<Rigidbody>();

        _currentState = MobState.Idle;

        _characterBodyRotation = GetComponent<CharacterBodyRotation>();

        _timer = new Timer();

        avaliableForPatrol = true;

        //Hide();
    }

    private void Start()
    {
        var register = FindObjectOfType<MobRegister>();

        if (register != null)
        {
            register.mobs.Add(this);
        }

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

                _characterBodyRotation.SetForward(_navigator.velocity);

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
                    _timer.ResetFixedTimer();
                }
                else
                {
                    if (_timer.UpdateFixedTimer(_maxTimeOutOfSight))
                    {
                        var enviroment = _target.GetComponent<CharacterEnviroment>();
                        if (enviroment != null)
                        {
                            enviroment.discoverCount--;
                        }

                        ChangeState(MobState.Patrol);
                    }
                }

                if (Vector3.Distance(transform.position, _target.position) <= _meleeRange)
                {
                    ChangeState(MobState.Attack);
                }

                _characterBodyRotation.SetForward(_navigator.velocity);
                break;

            case MobState.CheckPlace:
                if (CheckForTarget())
                {
                    ChangeState(MobState.Pursue);
                }

                _characterBodyRotation.SetForward(_navigator.velocity);
                break;

            case MobState.CheckPlaceIdle:
                if (CheckForTarget())
                {
                    ChangeState(MobState.Pursue);
                }
                else if (_timer.UpdateFixedTimer(_checkTime))
                {
                    ChangeState(MobState.Patrol);
                }

                _characterBodyRotation.SetTargetPosition(_checkPlace);
                break;


            case MobState.Attack:
                _characterBodyRotation.SetTargetRotation(_target);
                break;

            case MobState.AttackIdle:
                _characterBodyRotation.SetTargetRotation(_target);

                if (_timer.UpdateFixedTimer(_idleMeleeDuration))
                {
                    if (Vector3.Distance(_target.position, transform.position) < _meleeRange)
                    {
                        ChangeState(MobState.Attack);
                    }
                    else
                    {
                        ChangeState(MobState.Pursue);
                    }
                }

                break;

            
            case MobState.Stunned:
                if (_timer.UpdateFixedTimer(_timeToRecover))
                {
                    ChangeState(MobState.Pursue);
                }
                _characterBodyRotation.SetForward(_stunDirection);

                break;

            case MobState.Dead:
                break;

            default:
                Debug.LogError("No such State!");
                break;
        }
    }

    public void SetPatrolRoute(PatrolRoute route)
    {
        if (avaliableForPatrol)
        {
            _navigator.SetPatrolRoute(route);

            ChangeState(MobState.Patrol);

            avaliableForPatrol = false;

            //Show();
        }
    }

    void NoiseHeared(Vector3 position, Noiser source)
    {
        if (!isFighting)
        {
            GoCheckPlace(position, _timeCheckingPlace);
        }
    }

    public void GoCheckPlace(Vector3 position, float checkTime)
    {
        _checkPlace = position;
        _checkTime = checkTime;

        ChangeState(MobState.CheckPlace);

    }

    void TargetHit()
    {
        ChangeState(MobState.AttackIdle);
    }

    void EndAttack()
    {
        if (_currentState == MobState.Attack)
        {
            ChangeState(MobState.AttackIdle);
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

                _light.color = _patrolColor;
                break;

            case MobState.Idle:
                _navigator.Stop();

                _light.color = _patrolColor;
                break;


            case MobState.Pursue:
                _timer.ResetFixedTimer();

                _navigator.Pursue(_pursueSpeed, _target);

                _light.color = _fightColor;
                break;


            case MobState.Attack:
                _navigator.Stop();
                _melee.Attack(0);


                _light.color = _fightColor;
                break;

            case MobState.AttackIdle:
                _navigator.Stop();
                _timer.ResetFixedTimer();

                _light.color = _fightColor;
                break;

            case MobState.Stunned:
                _timer.ResetFixedTimer();
                _navigator.Stop();

                _light.color = _distressedColor;
                break;

            case MobState.Dead:
                _navigator.Stop();
                break;

            case MobState.CheckPlace:
                _timer.ResetFixedTimer();
                _navigator.GoToPosition(_pursueSpeed, _checkPlace, () =>
                {
                    ChangeState(MobState.CheckPlaceIdle);
                });

                _light.color = _distressedColor;
                break;

            case MobState.CheckPlaceIdle:
                _navigator.Stop();
                _characterBodyRotation.SetTargetPosition(_checkPlace);

                _light.color = _distressedColor;

                break;
            default:
                Debug.LogError("No such State!");
                break;
        }

        _currentState = nextState;

    }

    void Die(Vector3 source, float push, Transform hitter)
    {

        Hide();

        avaliableForPatrol = true;

    }

    void Hide()
    {
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = false;
        }

        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
        }

        enabled = false;

        _light.enabled = false;
        _navigator.Stop();
    }

    void Show()
    {
        foreach (Renderer renderer in GetComponentsInChildren<Renderer>())
        {
            renderer.enabled = true;
        }

        foreach (Collider collider in GetComponentsInChildren<Collider>())
        {
            collider.enabled = true;
        }

        enabled = true;

        _light.enabled = true;
        _navigator.Continue();
    }

    void Hurt(Vector3 source, float push, Transform hitter)
    {
        if (!isFighting)
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
        _target = hitter;

        _stunDirection =  hitSource - transform.position;
        _stunDirection.Normalize();
    }

}
