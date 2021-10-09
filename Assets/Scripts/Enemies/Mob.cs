using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Navigator), typeof(Sighter), typeof(Melee))]
[RequireComponent(typeof(Health), typeof(Listener), typeof(CharacterBodyRotation))]
public class Mob : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Light _light;
    [SerializeField] Pocket _pocket;
    [SerializeField] Renderer _renderer;

    [Header("Settings")]
    [SerializeField] [Range(0.1f, 5)] float _patrolSpeed;
    [SerializeField] [Range(0.1f, 5)] float _pursueSpeed;
    [SerializeField] [Range(1f, 10)] float _maxTimeOutOfSight;

    [SerializeField] Color _patrolColor = Color.blue;
    [SerializeField] Color _distressedColor = Color.yellow;
    [SerializeField] Color _fightColor = Color.red;

    [Header("Attack")]
    [SerializeField] [Range(1f, 10)] float _meleeRange;
    [SerializeField] [Range(0.1f, 2f)] float _idleMeleeDuration;
    [SerializeField] [Range(1f, 10)] float _timeToRecover;
    [SerializeField] [Range(1f, 10)] float _timeCheckingPlace;

    Navigator _navigator;
    Sighter _sighter;
    Melee _melee;
    Health _health;
    Listener _listener;

    [SerializeField] [Range(0, 10)] float _hitPush;

    [SerializeField] [Range(-1, 1)] float _deformation;
    [SerializeField] [Range(0, 0.5f)] float _deformationIdle;
    [SerializeField] Vector3 _deformationDirection;

    MobShaderModifyer _shaderModifyer;

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

    public Action startFightAction;

    MobState _currentState;

    Transform _target;

    //Stun State
    Vector3 _stunDirection;

    //Check Place State
    Vector3 _checkPlace;

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
        _health.deadAction += Die;
        _health.hurtAction += Hurt;


        _listener = GetComponent<Listener>();
        _listener.hearedNoiseAction += NoiseHeared;

        _currentState = MobState.Idle;

        _characterBodyRotation = GetComponent<CharacterBodyRotation>();

        if (_renderer)
        {
            _shaderModifyer = new MobShaderModifyer(_renderer);
        }

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

                if (CheckForTarget(true))
                {
                    ChangeState(MobState.Pursue);


                    if (startFightAction != null)
                        startFightAction.Invoke();

                }

                _characterBodyRotation.SetForwardRotation(_navigator.velocity);

                break;

            case MobState.Idle:
                if (CheckForTarget(true))
                {
                    ChangeState(MobState.Pursue);

                    if (startFightAction != null)
                        startFightAction.Invoke();
                }


                break;

            case MobState.Pursue:
                if (CheckForTarget(false))
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

                _characterBodyRotation.SetForwardRotation(_navigator.velocity);
                break;

            case MobState.CheckPlace:
                if (CheckForTarget(true))
                {
                    ChangeState(MobState.Pursue);


                    if (startFightAction != null)
                        startFightAction.Invoke();

                }

                if (Vector3.Distance(_checkPlace, transform.position) <= _meleeRange)
                {
                    ChangeState(MobState.CheckPlaceIdle);
                }

                _characterBodyRotation.SetForwardRotation(_navigator.velocity);
                break;

            case MobState.CheckPlaceIdle:
                if (CheckForTarget(true))
                {
                    ChangeState(MobState.Pursue);


                    if (startFightAction != null)
                        startFightAction.Invoke();

                }
                else if (_timer.UpdateFixedTimer(_timeCheckingPlace))
                {
                    ChangeState(MobState.Patrol);
                }

                _characterBodyRotation.SetTargetPositionRotation(_checkPlace);
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


                    if (startFightAction != null)
                        startFightAction.Invoke();

                }
                _characterBodyRotation.SetForwardRotation(_stunDirection);

                break;

            case MobState.Dead:
                break;

            default:
                Debug.LogError("No such State!");
                break;
        }
    }

    IEnumerator ElasticHit(float targetDeformation, Vector3 direction)
    {
        if (targetDeformation < 0)
        {
            targetDeformation *= -1;

            direction *= -1;
        }

        _shaderModifyer.deformationDirection = direction;
        while (_shaderModifyer.deformationAmmount < targetDeformation)
        {
            _shaderModifyer.deformationAmmount += targetDeformation * 0.3f;

            yield return new WaitForSeconds(0.1f);
        }

        while (_shaderModifyer.deformationAmmount > 0)
        {
            _shaderModifyer.deformationAmmount -= targetDeformation * 0.3f;

            yield return new WaitForSeconds(0.1f);
        }

        _shaderModifyer.deformationAmmount = 0;
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
            GoCheckPlace(position);
        }
    }

    public void GoCheckPlace(Vector3 position)
    {
        _checkPlace = position;

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

    bool CheckForTarget(bool spot)
    {
        //Check if enemy target is on sight
        if (_sighter.IsAnyTargetOnSight(spot))
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

                if (_pocket)
                    _pocket.canBePoked = true;

                if (_light != null)
                    _light.color = _patrolColor;
                break;

            case MobState.Idle:
                _navigator.Stop();

                if (_pocket)
                    _pocket.canBePoked = true;

                if (_light != null)
                    _light.color = _patrolColor;
                break;


            case MobState.Pursue:
                _timer.ResetFixedTimer();

                _navigator.Pursue(_pursueSpeed, _target);

                if (_pocket)
                    _pocket.canBePoked = false;


                if (_light != null)
                    _light.color = _fightColor;
                break;


            case MobState.Attack:
                _navigator.Stop();
                _melee.Attack(0);

                if (_pocket)
                    _pocket.canBePoked = false;

                if (_light != null)
                    _light.color = _fightColor;
                break;

            case MobState.AttackIdle:
                _navigator.Stop();
                _timer.ResetFixedTimer();

                if (_pocket)
                    _pocket.canBePoked = false;

                if (_light != null)
                    _light.color = _fightColor;
                break;

            case MobState.Stunned:
                _timer.ResetFixedTimer();
                //_navigator.Stop();

                if (_pocket)
                    _pocket.canBePoked = false;

                if (_light != null)
                    _light.color = _distressedColor;
                break;

            case MobState.Dead:
                _navigator.Stop();

                if (_pocket)
                    _pocket.canBePoked = false;

                break;

            case MobState.CheckPlace:
                _timer.ResetFixedTimer();
                _navigator.GoToPosition(_pursueSpeed, _checkPlace, null);

                if (_pocket)
                    _pocket.canBePoked = true;

                if (_light != null)
                    _light.color = _distressedColor;
                break;

            case MobState.CheckPlaceIdle:
                _navigator.Stop();
                _characterBodyRotation.SetTargetPositionRotation(_checkPlace);

                if (_pocket)
                    _pocket.canBePoked = true;


                if (_light != null)
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

        Destroy(gameObject);

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

    Coroutine elastic;
    void Hurt(Vector3 source, float push, Transform hitter)
    {
        if (!isFighting)
        {
            Stun(source, push, hitter);
        }

        if (_hitPush > 0)
        {
            var launchVelocity = (transform.position - source).normalized * push;

            launchVelocity.y = _hitPush;

            _navigator.Launch(launchVelocity);

        }

        if (elastic != null)
            StopCoroutine(elastic);

        elastic = StartCoroutine(ElasticHit(_deformation, Vector3.right));

        _target = hitter;

    }

    void Stun(Vector3 hitSource, float push, Transform hitter)
    {
        ChangeState(MobState.Stunned);
        _target = hitter;

        _stunDirection = hitSource - transform.position;
        _stunDirection.Normalize();
    }


    private void OnTriggerEnter(Collider other)
    {
        var sightTrigger = other.GetComponent<SightTrigger>();
        if (sightTrigger)
        {
            if (!isFighting)
            {
                if (sightTrigger.typeOfTrigger == SightTrigger.TypeOfTrigger.Player)
                {
                    _target = sightTrigger.transform;

                    ChangeState(MobState.Pursue);
                }

            }
        }
    }

}
