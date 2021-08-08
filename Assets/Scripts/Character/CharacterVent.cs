using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CharacterVent : InputComponent
{
    [Header("References")]
    [SerializeField] CinemachineVirtualCamera _camera;
    [SerializeField] CharacterMovement _characterMovement;
    [SerializeField] Transform _bodyObject;
    [SerializeField] Rigidbody _rigidbody;

    [SerializeField] Collider _basicCollider;
    [SerializeField] Collider _ventCollider;

    [Header("Settings")]
    [SerializeField] [Range(1, 10)] float _speed;
    [SerializeField] [Range(1, 10)] float _rotationLerp;

    PlayerCamera _playerCamera;

    Vector2 _moveInput;

    public Semaphore semaphore;

    public bool isOnVent { get; private set; }

    void Awake()
    {
        _playerCamera = FindObjectOfType<PlayerCamera>();

        semaphore = new Semaphore();
    }

    public void EnterVent(Vector3 position)
    {
        if (isOnVent == false)
        {
            isOnVent = true;
            _playerCamera.SetActiveCamera(_camera, PlayerCamera.TypeOfActiveCamera.Vent);

            _characterMovement.semaphore.Lock();

            _basicCollider.enabled = false;
            _ventCollider.enabled = true;

            transform.position = position;
        }

    }

    public void ExitVent()
    {
        if (isOnVent == true)
        {
            isOnVent = false;
            _playerCamera.ResetCamera();
            _characterMovement.semaphore.Unlock();

            _basicCollider.enabled = true;
            _ventCollider.enabled = false;
        }

    }

    private void FixedUpdate()
    {
        if (semaphore.isOpen && isOnVent)
        {
            MovePlayer(_speed);
        }
    }

    void MovePlayer(float speed)
    {
        if (_rigidbody != null && _playerCamera != null)
        {

            if (_moveInput == Vector2.zero)
            {
                _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);

            }
            else
            {
                #region Apply movement in camera Direction
                Vector3 targetVelocity;

                targetVelocity = _playerCamera.GetForward() * _moveInput.y + _playerCamera.GetRight() * _moveInput.x;

                targetVelocity.y = 0;
                targetVelocity.Normalize();
                targetVelocity *= speed;

                targetVelocity = targetVelocity - _rigidbody.velocity;

                targetVelocity.y = 0;

                _rigidbody.AddForce(targetVelocity, ForceMode.VelocityChange);

                #endregion

                #region Rotate Character Towards Direction

                if (_bodyObject != null)
                {
                    Quaternion prevRotation = _bodyObject.rotation;
                    if (_rigidbody.velocity != Vector3.zero)
                    {

                        Quaternion actualRot = Quaternion.LookRotation(_rigidbody.velocity);

                        var rot = Quaternion.Lerp(prevRotation, actualRot, Time.deltaTime * _rotationLerp).eulerAngles;

                        rot.z = 0;
                        rot.x = 0;
                        _bodyObject.transform.eulerAngles = rot;
                    }
                }

                #endregion

            }


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

    }
}
