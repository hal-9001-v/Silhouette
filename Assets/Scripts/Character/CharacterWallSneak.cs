using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWallSneak : InputComponent
{
    //Walls
    WallZone _currentWallZone;

    [Header("References")]
    [SerializeField] Transform _bodyObject;
    [SerializeField] CharacterAligner _aligner;
    [SerializeField] CharacterMovement _movement;
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] CharacterAnimationCommand _animationCommand;

    PlayerCamera _playerCamera;

    [Header("Settings")]
    [SerializeField] [Range(0.01f, 20)] float _alignmentSpeed = 10;
    [SerializeField] [Range(1, 10)] float _speed;

    public bool Apply;
    Vector2 _input;

    private void Awake()
    {
        _playerCamera = FindObjectOfType<PlayerCamera>();
    }


    private void FixedUpdate()
    {
        if (Apply)
        {
            MovePlayerInWall();
        }
    }

    public void StickToWall(WallZone wallZone)
    {
        if (_aligner != null)
        {
            Apply = true;
            _movement.Lock();

            wallZone.setCurrentPositionToClosestPoint(transform.position);

            _aligner.AlignCharacter(wallZone.GetCurrentPosition(), _alignmentSpeed, null);

            _currentWallZone = wallZone;

            _bodyObject.forward = wallZone.Direction;
        }
    }

    public void UnstickToWall()
    {
        Apply = false;
        _movement.Unlock();

        if (_animationCommand != null)
            _animationCommand.Idle();

    }


    public void MovePlayerInWall()
    {
        if (_rigidbody != null)
        {
            if (_input.x == 0)
            {
                _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);

                if (_animationCommand != null)
                    _animationCommand.WallIdle();

            }
            else
            {
                if (_animationCommand != null)
                    _animationCommand.WallMove();

                float totalTime;
                Vector3 movementDirection = _playerCamera.GetRight();
                //t = dx / Lenght
                if (_input.x > 0)
                {
                    totalTime = -Time.deltaTime * _speed / _currentWallZone.CurveLength;
                }
                else
                {
                    totalTime = Time.deltaTime * _speed / _currentWallZone.CurveLength;

                }

                _currentWallZone.ChangeTValue(movementDirection, totalTime);

                var position = _currentWallZone.GetCurrentPosition();

                position.y = transform.position.y;
                transform.position = position;

                _bodyObject.forward = _currentWallZone.GetCurrentForwardDirection();


            }


        }

    }


    public override void SetInput(PlatformMap input)
    {
        input.Character.Movement.performed += ctx =>
        {
            _input = ctx.ReadValue<Vector2>();
        };

    }
}
