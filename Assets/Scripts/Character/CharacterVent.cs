using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CharacterVent : InputComponent
{
    [Header("References")]
    [SerializeField] CinemachineVirtualCamera _camera;
    [SerializeField] Transform _cameraPosition;
    [SerializeField] Mover _mover;

    [SerializeField] CharacterMovement _characterMovement;
    [SerializeField] CharacterMelee _characterMelee;
    [SerializeField] SightTrigger _sightTrigger;
    [SerializeField] CharacterBodyRotation _characterBodyRotation;

    [SerializeField] Collider _basicCollider;
    [SerializeField] Collider _ventCollider;

    [Header("Settings")]
    [SerializeField] [Range(1, 10)] float _speed;

    PlayerCamera _playerCamera;

    Vector2 _moveInput;

    public Semaphore semaphore;

    int _ventCounter;

    public bool isOnVent { get; private set; }

    void Awake()
    {
        _playerCamera = FindObjectOfType<PlayerCamera>();
        semaphore = new Semaphore();
    }

    public void AddVentCounter()
    {
        _ventCounter++;
    }

    public void RemoveVentCounter()
    {
        if (_ventCounter > 0)
        {
            _ventCounter--;

            if (_ventCounter == 0)
            {
                ExitVent();
            }
        }
    }

    void EnterVent()
    {
        if (_ventCounter > 0)
        {
            isOnVent = true;
            _playerCamera.SetActiveCamera(_camera, PlayerCamera.TypeOfActiveCamera.Vent);
            _camera.transform.position = _cameraPosition.position;
            _characterBodyRotation.SetMovementRotation();


            _basicCollider.enabled = false;
            _ventCollider.enabled = true;

            _characterMovement.semaphore.Lock();
            _characterMelee.semaphore.Lock();

            if (_sightTrigger != null) {
                _sightTrigger.canBeSeen = false;
            }
        }
    }

    public void ExitVent()
    {
        if (isOnVent)
        {
            _ventCounter = 0;

            isOnVent = false;
            _playerCamera.ResetCamera();

            _basicCollider.enabled = true;
            _ventCollider.enabled = false;

            _characterMovement.semaphore.Unlock();
            _characterMelee.semaphore.Unlock();

            if (_sightTrigger != null)
            {
                _sightTrigger.canBeSeen = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (semaphore.isOpen && isOnVent)
        {
            if (_mover)
                _mover.Move(_moveInput, _speed);
        }
    }

    public override void SetInput(PlatformMap input)
    {
        input.Character.Movement.performed += ctx =>
        {
            _moveInput = ctx.ReadValue<Vector2>();
        };

        input.Character.Movement.canceled += ctx =>
        {
            _moveInput = Vector2.zero;
        };

        input.Character.Interact.performed += ctx =>
        {
            EnterVent();
        };

    }
}
