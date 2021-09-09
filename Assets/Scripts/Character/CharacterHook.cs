using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;


[RequireComponent(typeof(ScreenTarget))]
public class CharacterHook : InputComponent
{
    [Header("References")]
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] CharacterMovement _characterMovement;


    [Header("Settings")]
    [SerializeField] [Range(0.1f, 20)] float _range;
    [SerializeField] [Range(0.1f, 20)] float _speed;
    [SerializeField] [Range(0.1f, 2)] float _stopDistance;
    [SerializeField] [Range(1f, 20)] float _maxDuration;

    ScreenTarget _screenTarget;

    Hook[] _hooks;

    Hook _currentHook;

    Coroutine _lifeSpan;

    bool _pulling;

    public bool hey;

    public Semaphore semaphore;

    private void Awake()
    {
        _hooks = FindObjectsOfType<Hook>();
        semaphore = new Semaphore();

        _screenTarget = GetComponent<ScreenTarget>();
    }

    private void FixedUpdate()
    {
        if (_pulling && _currentHook != null)
        {

            if (Vector3.Distance(_currentHook.transform.position, transform.position) > _stopDistance)
            {
                PullToTarget(_currentHook.transform);

            }
            else
            {
                StopHook();
            }
        }


        hey = false;

        foreach (Hook hook in _hooks)
        {
            if (Vector3.Distance(hook.transform.position, transform.position) < _range)
            {
                if (_screenTarget.IsObjectInAim(hook.transform))
                {
                    hey = true;

                }

            }
        }
    }

    void PullToTarget(Transform target)
    {
        Vector3 currentDirection = (target.transform.position - _rigidbody.transform.position).normalized;

        var targetVelocity = currentDirection * _speed - _rigidbody.velocity;
        _rigidbody.AddForce(targetVelocity, ForceMode.VelocityChange);
    }

    void CheckHooks()
    {
        if (_hooks != null)
        {
            foreach (Hook hook in _hooks)
            {
                if (Vector3.Distance(hook.transform.position, transform.position) < _range)
                {
                    if (_screenTarget.IsObjectInAim(hook.transform))
                    {

                        hook.HookCharacter(this);

                        _currentHook = hook;

                        HookCharacter(hook);

                        return;
                    }
                }
            }
        }
    }

    void HookCharacter(Hook target)
    {
        if (_rigidbody != null && semaphore.isOpen)
        {
            _characterMovement.semaphore.Lock();

            _pulling = true;
            _currentHook = target;

            //Debug.Log("Hooked");

            _rigidbody.detectCollisions = false;

            _lifeSpan = StartCoroutine(HookLifeSpan());
        }

    }

    void StopHook()
    {
        //Character is in place
        _pulling = false;
        _rigidbody.detectCollisions = true;

        //What to do once hook has finished
        switch (_currentHook.EndingVelocity)
        {
            //Stop pull
            case Hook.EndVelocity.stop:
                _rigidbody.velocity = Vector3.zero;
                break;

            //Keep pull and let player fly
            case Hook.EndVelocity.keep:
                break;
        }

        _characterMovement.semaphore.Unlock();

        StopCoroutine(_lifeSpan);
    }

    IEnumerator HookLifeSpan()
    {
        yield return new WaitForSeconds(_maxDuration);

        StopHook();
    }

    public override void SetInput(PlatformMap input)
    {
        input.Character.Interact.performed += ctx =>
        {
            CheckHooks();
        };
    }

}
