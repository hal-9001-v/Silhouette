using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterBodyRotation), typeof(CharacterAligner), typeof(CharacterMovement))]
[RequireComponent(typeof(Mover), typeof(CharacterAnimationCommand))]
public class CharacterWallSneak : InputComponent
{
    //Walls
    WallZone _currentWallZone;

    CharacterBodyRotation _bodyRotation;
    CharacterAligner _aligner;
    CharacterMovement _movement;
    CharacterAnimationCommand _animationCommand;
    Mover _mover;

    public LineRenderer line;

    PlayerCamera _playerCamera;

    int _lineIndex;

    Vector3 pointAtLeft
    {
        get
        {
            return _currentWallZone.GetPoint(_lineIndex - 1);
        }
    }

    Vector3 pointAtRight
    {
        get
        {
            return _currentWallZone.GetPoint(_lineIndex + 1);
        }
    }

    [Header("Settings")]
    [SerializeField] [Range(0.01f, 20)] float _alignmentSpeed = 10;
    [SerializeField] Vector3 _alignmentOffset;
    [SerializeField] [Range(0.1f, 3)] float _speed;

    bool _sticked;

    Vector2 _input;

    public bool atRightLimit;
    public bool atLeftLimit;

    public Semaphore semaphore;

    private void Awake()
    {
        _bodyRotation = GetComponent<CharacterBodyRotation>();
        _aligner = GetComponent<CharacterAligner>();
        _movement = GetComponent<CharacterMovement>();
        _animationCommand = GetComponent<CharacterAnimationCommand>();
        _mover = GetComponent<Mover>();


        _playerCamera = FindObjectOfType<PlayerCamera>();

        semaphore = new Semaphore();
    }

    private void FixedUpdate()
    {
        if (_sticked)
        {
            MovePlayerInWall();
        }
    }

    public void StickToWall(WallZone wallZone)
    {
        if (semaphore.isOpen)
        {
            _movement.semaphore.Lock();

            _currentWallZone = wallZone;

            _bodyRotation.SetForward(wallZone.Direction);

            _lineIndex = wallZone.GetClosestIndex(transform.position);

            _sticked = false;

            _aligner.AlignCharacter(wallZone.GetPoint(_lineIndex) + _alignmentOffset, _alignmentSpeed, EndOfAligment);
        }
    }

    void EndOfAligment()
    {
        _sticked = true;

    }

    public void UnstickToWall()
    {
        _movement.semaphore.Unlock();

        _sticked = false;

        if (_animationCommand != null)
            _animationCommand.Idle();

    }


    public void MovePlayerInWall()
    {
        if (_input.x == 0)
        {

            if (_animationCommand != null)
                _animationCommand.WallIdle();

            _mover.StopMovement();
        }
        else
        {
            int nextIndex;
            Vector3 direction;
            Vector3 destination;
            if (_input.x * _playerCamera.GetRight().x > 0)
            {
                if (atRightLimit) return;

                nextIndex = _lineIndex + 1;

                destination = _currentWallZone.GetPoint(_lineIndex + 1);
                nextIndex = _lineIndex + 1;
            }
            else
            {
                if (atLeftLimit) return;

                nextIndex = _lineIndex - 1;

                destination = _currentWallZone.GetPoint(_lineIndex - 1);
                nextIndex = _lineIndex - 1;
            }

            direction = destination - transform.position;

            direction.y = 0;
            direction.Normalize();
            _mover.Move(direction * _speed);


            //Ignore verticality
            destination.y = transform.position.y;
            if (Vector3.Distance(destination, transform.position) < 0.05f)
            {
                _lineIndex = nextIndex;
            }


            if (_animationCommand != null)
                _animationCommand.WallMove();
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
