using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeneralTools;


[RequireComponent(typeof(Mover))]
public class CharacterNavigator : InputComponent
{

    [Header("Settings")]
    [SerializeField] [Range(0.1f, 1)] float _stoppingDistance = 0.1f;

    Mover _mover;

    PlatformMap _input;

    bool _moving;
    Vector3 _targetPosition;
    float _speed;

    Action _arrivingAction;

    private void Awake()
    {
        _mover = GetComponent<Mover>();

    }

    private void FixedUpdate()
    {
        if (_moving)
        {
            var direction = _targetPosition - transform.position;

            _mover.Move(direction.normalized * _speed);

            if (PhysicsMath.NonVerticalDistance(_targetPosition, transform.position) < _stoppingDistance)
            {
                _moving = false;

                _mover.StopMovement();

                if (_input != null)
                    _input.Enable();

                if (_arrivingAction != null)
                {
                    _arrivingAction.Invoke();
                }
            }
        }
    }

    public void GoToDestination(Vector3 position, float speed, Action onArrivingAction)
    {
        if (_input != null)
            _input.Character.Disable();

        _moving = true;

        _targetPosition = position;
        _speed = speed;

        _arrivingAction = onArrivingAction;
    }

    public override void SetInput(PlatformMap input)
    {
        _input = input;
    }
}
