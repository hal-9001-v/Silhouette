using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBodyRotation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform _body;
    [SerializeField] Rigidbody _rigidbody;

    [Header("Settings")]
    [SerializeField] [Range(0.1f, 10)] float _lerpFactor;

    RotationType _rotationType;

    public Semaphore semaphore;

    Transform _target;

    public enum RotationType
    {
        Movement,
        LookAtTarget,
        TargetForward
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
                    TargetForward(_target);
                    break;
            }
        }
    }

    private void TargetForward(Transform target)
    {
        if (_body != null && target != null)
        {
            var direction = target.forward;

            var newForward = Vector3.Lerp(_body.forward, direction.normalized, Time.deltaTime * _lerpFactor);

            newForward.z = 0;
            newForward.x = 0;

            _body.transform.forward = newForward.normalized;

        }

    }

    public void SetMovementRotation()
    {
        _rotationType = RotationType.Movement;
    }

    public void SetTargetRotation(Transform target)
    {
        _rotationType = RotationType.LookAtTarget;

        _target = target;
    }
    
    public void SetTargetForwar(Transform target)
    {
        _rotationType = RotationType.TargetForward;

        _target = target;
    }



    void MovementRotation()
    {

        if (_body != null)
        {
            if (_rigidbody.velocity != Vector3.zero)
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

            var newForward = Vector3.Lerp(_body.forward, direction.normalized, Time.deltaTime * _lerpFactor);

            newForward.z = 0;
            newForward.x = 0;

            _body.transform.forward = newForward.normalized;

        }

    }
}


