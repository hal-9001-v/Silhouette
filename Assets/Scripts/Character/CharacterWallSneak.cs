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


    public LineRenderer Line;

    PlayerCamera _playerCamera;

    int _lineIndex;

    [Header("Settings")]
    [SerializeField] [Range(0.01f, 20)] float _alignmentSpeed = 10;
    [SerializeField] Vector3 _alignmentOffset;
    [SerializeField] [Range(0.1f, 3)] float _speed;

    public bool Apply;
    Vector2 _input;

    public bool atRightLimit;
    public bool atLeftLimit;

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
            _movement.Lock();

            _currentWallZone = wallZone;

            _bodyObject.forward = wallZone.Direction;

            _lineIndex = wallZone.GetClosestIndex(transform.position);

            _aligner.AlignCharacter(wallZone.GetPoint(ref _lineIndex) + _alignmentOffset, _alignmentSpeed, EndOfAligment);
        }
    }

    void EndOfAligment()
    {
        Apply = true;
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

                if (_animationCommand != null)
                    _animationCommand.WallIdle();

            }
            else
            {
                Vector3 destination;
                int nextIndex;
                if (_input.x * _playerCamera.GetRight().x > 0)
                {
                    if (atRightLimit) return;

                    nextIndex = _lineIndex + 1;
                }
                else
                {
                    if (atLeftLimit) return;

                    nextIndex = _lineIndex - 1;
                }

                destination = _currentWallZone.GetPoint(ref nextIndex);


                var direction = destination - transform.position;

                direction.y = 0;
                direction.Normalize();
                _rigidbody.AddForce(direction.normalized * _speed - _rigidbody.velocity, ForceMode.VelocityChange);

                //Ignore verticality
                destination.y = transform.position.y;
                if (Vector3.Distance(destination, transform.position) < 0.01f)
                {
                    _lineIndex = nextIndex;
                }

                if (_animationCommand != null)
                    _animationCommand.WallMove();
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
