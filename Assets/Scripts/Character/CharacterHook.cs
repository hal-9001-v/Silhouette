using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class CharacterHook : InputComponent
{

    [Header("References")]
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] Camera _camera;
    [SerializeField] RawImage _targetAim;
    [SerializeField] CharacterMovement _characterMovement;


    [Header("Settings")]
    [SerializeField] [Range(0.1f, 20)] float _range;
    [SerializeField] [Range(0.1f, 20)] float _speed;
    [SerializeField] [Range(0.1f, 2)] float _stopDistance;
    [SerializeField] [Range(1f, 20)] float _maxDuration;
    [SerializeField] LayerMask _blockingMask;



    [Header("Gizmos")]
    [SerializeField] Color _gizmosColor = Color.yellow;

    Hook[] _hooks;

    Hook _currentHook;

    Coroutine _lifeSpan;

    bool _pulling;

    int _lockCount;

    Vector2 _screenCenter
    {
        get
        {
            return new Vector2(Screen.width, Screen.height) * 0.5f;
        }
    }

    private void Awake()
    {
        _hooks = FindObjectsOfType<Hook>();
    }
    public void Lock()
    {
        _lockCount++;
    }

    public void Unlock()
    {

        if (_lockCount > 0) _lockCount--;
        else
        {
            Debug.LogWarning("Lock Count is already 0, cant be freed!");
        }
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
                if (CanBeHooked(hook))
                {
                    hook.HookCharacter(this);

                    _currentHook = hook;

                    HookCharacter(hook);

                    return;
                }
            }
        }
    }

    void HookCharacter(Hook target)
    {
        if (_rigidbody != null && _lockCount == 0)
        {
            _characterMovement.Lock();

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

        _characterMovement.Unlock();

        StopCoroutine(_lifeSpan);
    }

    bool CanBeHooked(Hook hook)
    {
        //Check if hook is too away
        if (Vector3.Distance(hook.transform.position, transform.position) > _range) return false;

        Vector3 rawPositionWorld = _camera.WorldToScreenPoint(hook.transform.position);

        if (!(rawPositionWorld.x > _targetAim.rectTransform.position.x - Mathf.Abs(_targetAim.rectTransform.sizeDelta.x) * 0.5f)) return false;

        if (!(rawPositionWorld.x < _targetAim.rectTransform.position.x + Mathf.Abs(_targetAim.rectTransform.sizeDelta.x) * 0.5f)) return false;

        if (!(rawPositionWorld.y > _targetAim.rectTransform.position.y - Mathf.Abs(_targetAim.rectTransform.sizeDelta.y) * 0.5f)) return false;


        if (!(rawPositionWorld.y < _targetAim.rectTransform.position.y + Mathf.Abs(_targetAim.rectTransform.sizeDelta.y) * 0.5f)) return false;

        Vector3 direction = hook.transform.position - transform.position;
        direction.Normalize();

        if (Physics.Raycast(transform.position, direction, _range, _blockingMask)) return false;

        return true;
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

    private void OnDrawGizmos()
    {

        Gizmos.color = _gizmosColor;
        Gizmos.DrawWireSphere(transform.position, _range);

    }

}
