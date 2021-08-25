using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBodyRotation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform _body;
    Rigidbody _rigidbody;

    [Header("Settings")]
    [SerializeField] [Range(0.1f, 10)] float _lerpFactor;

    RotationType _rotationType;

    public Semaphore semaphore;

    Transform _target;
    Vector3 _targetPosition;

    Vector3 _forward;

    public enum RotationType
    {
        Movement,
        LookAtTarget,
        LookAtPosition,
        TargetForward,
        Forward,
        None
    }

    private void Awake()
    {
        semaphore = new Semaphore();
    }

    private void FixedUpdate()
    {
        if (semaphore.isOpen)
        {
            switch (_rotationType)
            {
                case RotationType.Movement:
                    MovementRotation();
                    break;

                case RotationType.LookAtTarget:
                    TargetRotation(_target);
                    break;

                case RotationType.TargetForward:
                    TargetForwardRotation(_target);
                    break;

                case RotationType.LookAtPosition:
                    TargetPositionRotation(_targetPosition);
                    break;

                case RotationType.Forward:
                    ForwardRotation(_forward);
                    break;

                case RotationType.None:

                    break;
            }
        }
    }

    /// <summary>
    /// Body will rotate towards assigned rigidbody's velocity.
    /// </summary>
    public void SetMovementRotation(Rigidbody rigidbody)
    {
        _rotationType = RotationType.Movement;

        _rigidbody = rigidbody;
    }

    /// <summary>
    /// Body will face the target.
    /// </summary>
    /// <param name="target"></param>
    public void SetTargetRotation(Transform target)
    {
        _rotationType = RotationType.LookAtTarget;

        _target = target;
    }

    /// <summary>
    /// Body will copy the target's forward normalized.
    /// </summary>
    /// <param name="target"></param>
    public void SetTargetForward(Transform target)
    {
        _rotationType = RotationType.TargetForward;

        _target = target;
    }

    /// <summary>
    /// Body will face provided forward normalized.
    /// </summary>
    /// <param name="forward"></param>
    public void SetForward(Vector3 forward)
    {
        _rotationType = RotationType.Forward;

        _forward = forward;
    }

    /// <summary>
    /// Body will face provided position
    /// </summary>
    public void SetTargetPosition(Vector3 position)
    {
        _rotationType = RotationType.LookAtPosition;

        _targetPosition = position;
    }

    public void DisableRotation()
    {
        _rotationType = RotationType.None;
    }

    void MovementRotation()
    {
        if (_body != null && _rigidbody != null)
        {
            if (_rigidbody.velocity.x != 0 && _rigidbody.velocity.z != 0)
            {
                Quaternion prevRotation = _body.rotation;

                Quaternion actualRot = Quaternion.LookRotation(_rigidbody.velocity);

                var rot = Quaternion.Lerp(prevRotation, actualRot, Time.deltaTime * _lerpFactor).eulerAngles;

                rot.z = 0;
                rot.x = 0;
                _body.transform.eulerAngles = rot;
            }
        }


    }

    void TargetRotation(Transform target)
    {
        if (_body != null && target != null)
        {
            var direction = target.position - _body.position;

            ForwardRotation(direction);

        }

    }

    void TargetPositionRotation(Vector3 position)
    {
        if (_body != null)
        {
            var direction = position - _body.position;

            ForwardRotation(direction);

        }
    }

    private void TargetForwardRotation(Transform target)
    {
        if (_body != null && target != null)
        {
            ForwardRotation(target.forward);

        }

    }

    private void ForwardRotation(Vector3 forward)
    {
        if (_body != null)
        {
            var direction = forward;

            var newForward = Vector3.Lerp(_body.forward, direction.normalized, Time.deltaTime * _lerpFactor);

            newForward.y = 0;

            _body.transform.forward = newForward.normalized;

        }
    }

}


