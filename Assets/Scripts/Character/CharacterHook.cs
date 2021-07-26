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
    [SerializeField] [Range(0.1f, 10)] float _centerRange;
    [SerializeField] [Range(0.1f, 20)] float _speed;
    [SerializeField] [Range(0.1f, 20)] float _approximationValue;



    [Header("Gizmos")]
    [SerializeField] Color _gizmosColor = Color.yellow;

    Hook[] _hooks;

    Hook _currentHook;

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
        if (_rigidbody != null)
        {
            _characterMovement.Lock();

            StartCoroutine(PullCharacter(target.StoppingPlace, 0.3f));

            Debug.Log("Hooked");
        }

    }

    IEnumerator PullCharacter(Transform target, float stoppingDistance)
    {
        Vector3 currentDirection = (target.transform.position - _rigidbody.transform.position).normalized;
        Vector3 previousDirection = currentDirection;
        while (Vector3.Distance(target.position, _rigidbody.transform.position) > stoppingDistance)
        {
            //Dont let player getting stuck on aproximations
            if (Vector3.Dot(currentDirection, previousDirection) < 0)
            {
                break;
            }
            currentDirection = (target.transform.position - _rigidbody.transform.position).normalized;
            previousDirection = currentDirection;

            _rigidbody.velocity = currentDirection * _speed;


            yield return null;
        }

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


        return true;
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
