using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CharacterMovement : InputComponent
{
    [Header("References")]
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] Transform _groundCheck;
    [SerializeField] Transform _bodyObject;
    [SerializeField] CharacterAnimationCommand _animationCommand;
    [SerializeField] PlayerCamera _playerCamera;
    [SerializeField] CinemachineVirtualCamera _gameCamera;
    [SerializeField] CharacterWallSneak _wallSneak;

    [SerializeField] Transform _upRaycastPoint;
    [SerializeField] Transform _downRaycastPoint;


    [Space(5)]
    [Header("Settings")]
    [Range(1, 20)]
    [SerializeField] float _runningSpeed;

    [Range(1, 10)]
    [SerializeField] float _creepingSpeed = 3;

    [Range(1, 10)]
    [SerializeField] float _inAirSpeed = 3;

    [Range(1, 10)]
    [SerializeField] float _jumpHeight;
    
    [Range(0.1f, 1)]
    [SerializeField] float _secondJumpFactor;

    [Range(0.01f, 1)]
    [SerializeField] float _ledgeGrabRange;


    [Range(0.1f, 1)]
    [SerializeField] float _jumpGizmosWidth = 0.2f;

    [Range(0.1f, 1)]
    [SerializeField]
    float _groundCheckRadius = 0.2f;

    [SerializeField]
    LayerMask _groundMask;

    [Range(0.5f, 10)]
    [SerializeField] float _lerpFactor = 5;

    bool _readyForSecondJump;

    int _lockCount;

    //Logic
    Vector2 _currentInput;
    bool _inputJump;

    State _currentState;
    bool _isCreeping;

    enum State
    {
        Idle,
        Running,
        Creeping,
        Jumping,
        AtLedge
    }

    private void Start()
    {
        _currentState = State.Idle;
        _animationCommand.Smile();

        if (_animationCommand != null)
        {
            _animationCommand.Idle();
        }
    }

    public override void SetInput(PlatformMap input)
    {

        #region Axis Movement
        input.Character.Movement.performed += ctx =>
        {
            Vector2 v = ctx.ReadValue<Vector2>();

            //Set Latest Input Direction. Movement is applied in FixedUpdate
            _currentInput = v;

        };


        input.Character.Movement.canceled += ctx =>
        {
            _currentInput = Vector2.zero;
        };

        #endregion

        input.Character.Jump.performed += ctx =>
        {
            _inputJump = true;
        };

        input.Character.Creep.performed += ctx =>
        {
            _isCreeping = true;
        };

        input.Character.Creep.canceled += ctx =>
        {
            _isCreeping = false;
        };

    }

    private void Jump()
    {
        if (IsGrounded(_groundCheckRadius))
        {
            Launch(_jumpHeight);

        }

    }

    private bool IsGrounded(float radius)
    {
        if (_groundCheck != null)
        {
            var groundCollisions = Physics.OverlapSphere(_groundCheck.position, radius, _groundMask);


            if (groundCollisions.Length != 0)
                return true;
            /*foreach (Collider c in groundCollisions)
            {
                return true;
            }
            */


        }

        return false;
    }

    private void FixedUpdate()
    {
        if (_lockCount == 0)
        {
            //Look at next switch for state transition.
            switch (_currentState)
            {
                case State.Idle:

                    if (_inputJump)
                        Jump();

                    _animationCommand.Idle();
                    break;

                case State.Running:
                    MovePlayer(_runningSpeed);

                    if (_inputJump)
                        Jump();

                    _animationCommand.Run();
                    break;

                case State.Creeping:
                    MovePlayer(_creepingSpeed);

                    if (_inputJump)
                        Jump();

                    _animationCommand.Creep();
                    break;

                case State.Jumping:
                    MovePlayer(_inAirSpeed);

                    //Double Jump!
                    if (_inputJump && _readyForSecondJump) {
                        Launch(_jumpHeight * _secondJumpFactor);
                        
                        //Only two jumps!
                        _readyForSecondJump = false;
                    }
                    break;

                case State.AtLedge:
                    _rigidbody.velocity = Vector3.zero;
                    _rigidbody.useGravity = false;

                    if (_inputJump)
                    {
                        _rigidbody.useGravity = true;
                        //Dont check ground for this jump
                        Launch(_jumpHeight);
                    }

                    break;
            }

            //Reset Input Variables
            _inputJump = false;

            //States transitions
            switch (_currentState)
            {
                case State.Idle:

                    if (_currentInput != Vector2.zero)
                    {

                        if (_isCreeping)
                        {
                            _currentState = State.Creeping;
                        }
                        else
                        {
                            _currentState = State.Running;
                        }

                    }

                    break;

                case State.Running:
                    if (_currentInput == Vector2.zero)
                    {
                        _currentState = State.Idle;
                    }
                    else
                    {
                        if (_isCreeping)
                        {
                            _currentState = State.Creeping;
                        }
                    }
                    break;

                case State.Creeping:
                    if (_currentInput == Vector2.zero)
                    {
                        _currentState = State.Idle;
                    }
                    else
                    {
                        if (!_isCreeping)
                        {
                            _currentState = State.Running;
                        }
                    }
                    break;

                case State.Jumping:

                    //Grab ledge when falling
                    if (_rigidbody.velocity.y <= 0)
                    {
                        if (GrabLedge())
                        {
                            _currentState = State.AtLedge;
                        }
                        else if (IsGrounded(_groundCheckRadius))
                        {
                            _currentState = State.Idle;
                            _animationCommand.Land();

                        }
                    }


                    break;

                case State.AtLedge:
                    break;


            }

        }
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
    private void MovePlayer(float speed)
    {
        if (_rigidbody != null && _playerCamera != null)
        {

            if (_currentInput == Vector2.zero)
            {
                _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);

            }
            else
            {
                #region Apply movement in camera Direction
                Vector3 v = new Vector3();
                Vector3 v2;

                v2 = _playerCamera.GetForward();
                v2.y = 0;

                v2.Normalize();
                v2 *= speed * _currentInput.y;

                v += v2;

                v2 = _playerCamera.GetRight();
                v2.y = 0;

                v2.Normalize();
                v2 *= speed * _currentInput.x;

                v += v2;

                v.y = _rigidbody.velocity.y;

                _rigidbody.velocity = v;


                #endregion

                #region Rotate Character Towards Direction

                if (_bodyObject != null)
                {
                    Quaternion prevRotation = _bodyObject.rotation;
                    Quaternion actualRot = Quaternion.LookRotation(_rigidbody.velocity);

                    var rot = Quaternion.Lerp(prevRotation, actualRot, Time.deltaTime * _lerpFactor).eulerAngles;

                    rot.z = 0;
                    rot.x = 0;
                    _bodyObject.transform.eulerAngles = rot;
                }

                #endregion

            }


        }


    }

    bool GrabLedge()
    {
        if (_downRaycastPoint != null && _upRaycastPoint != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(_downRaycastPoint.position, _bodyObject.forward, out hit, _ledgeGrabRange, _groundMask))
            {
                if (!Physics.Raycast(_upRaycastPoint.position, _bodyObject.forward, _ledgeGrabRange, _groundMask))
                {
                    Debug.Log("Grabbing");
                    //transform.position = hit.point;

                    return true;
                }


            }


        }

        return false;
    }
    public void Launch(float height)
    {
        var newVelocity = _rigidbody.velocity;

        float launchMagnitude = 2 * Mathf.Abs(Physics.gravity.y) * height;

        launchMagnitude = Mathf.Pow(launchMagnitude, 0.5f);

        newVelocity.y = launchMagnitude;

        _rigidbody.velocity = newVelocity;

        _readyForSecondJump = true;

        _animationCommand.Jump();
        _currentState = State.Jumping;
    }

    public void MovePlayerToPositionInPipe(Vector3 position)
    {
        transform.position = position;

        StartCoroutine(CountdownToAction(2, () =>
        {

            _currentState = State.Idle;

        }));

    }

    IEnumerator CountdownToAction(float time, Action action)
    {

        yield return new WaitForSeconds(time);
        action.Invoke();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;


        var cubePosition = transform.position;
        cubePosition += new Vector3(0, 0.5f * _jumpHeight, 0);


        Gizmos.DrawWireCube(cubePosition, new Vector3(_jumpGizmosWidth, _jumpHeight, _jumpGizmosWidth));

        Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
    }
#endif


}
