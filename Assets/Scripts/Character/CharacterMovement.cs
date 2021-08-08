using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class CharacterMovement : InputComponent
{
    [Header("References")]
    [SerializeField] Rigidbody _rigidbody;
    [SerializeField] Transform _bodyObject;
    [Space(5)]
    [SerializeField] Collider _standCollider;
    [SerializeField] Collider _crawlCollider;
    [Space(5)]
    [SerializeField] CharacterAnimationCommand _animationCommand;
    [SerializeField] CharacterBodyRotation _characterBodyRotation;
    [SerializeField] PlayerCamera _playerCamera;
    [SerializeField] Noiser _noiser;

    [Space(5)]
    [SerializeField] Transform _upRaycastPoint;
    [SerializeField] Transform _downRaycastPoint;
    [SerializeField] Transform _groundCheck;

    [Space(5)]
    [Header("Settings")]
    [Range(1, 20)]
    [SerializeField] float _runningSpeed;
    [Range(1, 20)]
    [SerializeField] float _sprintSpeed;

    [Range(0, 20)]
    [SerializeField] float _sprintNoiseRange;

    [SerializeField] [Range(1, 10)] float _creepingSpeed = 3;
    [SerializeField] [Range(1, 10)] float _crawlSpeed = 3;

    [Range(1, 10)]
    [SerializeField] float _inAirSpeed = 3;

    [Range(1, 10)]
    [SerializeField] float _jumpHeight;

    [Range(0.1f, 1)]
    [SerializeField] float _secondJumpFactor;

    [Range(0.01f, 1)]
    [SerializeField] float _ledgeGrabRange;


    [Range(0, 90)]
    [SerializeField] float _ledgeGrabAngle;

    [Range(0.1f, 1)]
    [SerializeField] float _jumpGizmosWidth = 0.2f;

    [Range(0.1f, 1)]
    [SerializeField]
    float _groundCheckRadius = 0.2f;

    [SerializeField]
    LayerMask _groundMask;

    [Header("Gizmos")]
    [SerializeField] Color _noiseColor = Color.yellow;
    [SerializeField] Color _jumpHeightColor = Color.blue;
    [SerializeField] Color _groundCheckColor = Color.yellow;

    public Semaphore semaphore;

    public bool isCrawling
    {
        get
        {
            return _currentState == State.CrawlIdle || _currentState == State.CrawlMove;
        }
    }

    //Logic
    Vector2 _moveInput;
    bool _inputJump;
    bool _inputCreep;
    bool _inputCrawl;
    bool _inputSprint;

    //Everytime ChangeState(nextState), _endOfStateAction.invoke(nextState)
    Action<State> _endOfStateAction;

    public State _currentState;
    public enum State
    {
        NormalIdle,
        RunMove,

        SprintMove,

        CreepIdle,
        CreepMove,

        CrawlIdle,
        CrawlMove,

        Launched,

        JumpStart,
        JumpIdle,
        SecondJump,

        AtLedge,
        Hurt
    }

    private void Awake()
    {
        semaphore = new Semaphore();
        _crawlCollider.enabled = false;
    }

    private void Start()
    {
        _currentState = State.NormalIdle;
        _animationCommand.Smile();

        if (_animationCommand != null)
        {
            _animationCommand.Idle();
        }
    }



    public bool isGrounded
    {
        get
        {
            if (_groundCheck != null && _rigidbody.velocity.y <= 0)
            {
                var groundCollisions = Physics.OverlapSphere(_groundCheck.position, _groundCheckRadius, _groundMask);


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



    }

    private void FixedUpdate()
    {
        if (semaphore.isOpen)
        {
            _characterBodyRotation.SetMovementRotation();

            //Look at next switch for state transition.
            switch (_currentState)
            {
                case State.NormalIdle:
                    CheckTransitions();

                    if (_inputJump && isGrounded)
                        ChangeState(State.JumpStart);

                    _animationCommand.Idle();
                    break;

                case State.RunMove:
                    MovePlayer(_runningSpeed, ForceMode.VelocityChange);

                    CheckTransitions();

                    if (_inputJump && isGrounded)
                        ChangeState(State.JumpStart);

                    _animationCommand.Run();
                    break;

                case State.SprintMove:
                    MovePlayer(_sprintSpeed, ForceMode.VelocityChange);
                    _noiser.MakeNoise(transform.position, _sprintNoiseRange);

                    CheckTransitions();

                    if (_inputJump && isGrounded)
                        ChangeState(State.JumpStart);

                    _animationCommand.Run();
                    break;


                case State.CreepIdle:

                    CheckTransitions();

                    _animationCommand.CreepIdle();
                    break;


                case State.CreepMove:
                    MovePlayer(_creepingSpeed, ForceMode.VelocityChange);

                    CheckTransitions();

                    if (_inputJump && isGrounded)
                        ChangeState(State.JumpStart);

                    _animationCommand.Creep();
                    break;

                case State.CrawlIdle:
                    CheckTransitions();

                    if (_inputJump && isGrounded)
                        ChangeState(State.JumpStart);

                    _animationCommand.CrawlIdle();
                    break;

                case State.CrawlMove:
                    MovePlayer(_crawlSpeed, ForceMode.VelocityChange);

                    CheckTransitions();

                    if (_inputJump && isGrounded)
                        ChangeState(State.JumpStart);

                    _animationCommand.Crawl();
                    break;


                case State.JumpStart:
                    //Make pass a frame so rigidbody.velocity gets actualized and Jump Idle doesnt detect isGrounded as true.
                    ChangeState(State.JumpIdle);

                    break;

                case State.Launched:
                    //Make pass a frame so rigidbody.velocity gets actualized and Jump Idle doesnt detect isGrounded as true.
                    ChangeState(State.JumpIdle);
                    break;

                case State.JumpIdle:
                    MovePlayer(_inAirSpeed, ForceMode.VelocityChange);

                    if (CanGrabLedge())
                    {
                        ChangeState(State.AtLedge);
                    }
                    else
                    {
                        if (isGrounded)
                        {
                            CheckTransitions();
                        }
                        else if (_inputJump)
                        {
                            ChangeState(State.SecondJump);
                        }

                    }



                    break;



                case State.SecondJump:
                    MovePlayer(_inAirSpeed, ForceMode.VelocityChange);
                    if (CanGrabLedge())
                    {
                        ChangeState(State.AtLedge);
                    }
                    else
                    {
                        if (isGrounded)
                        {
                            CheckTransitions();
                        }
                    }
                    break;

                case State.AtLedge:
                    _rigidbody.velocity = Vector3.zero;
                    _rigidbody.useGravity = false;

                    if (_inputJump)
                    {
                        _rigidbody.useGravity = true;

                        //Dont check ground for this jump
                        ChangeState(State.JumpStart);
                    }

                    break;


            }

        }


        //Reset Input Variables
        _inputJump = false;
    }

    void CheckTransitions()
    {
        if (_moveInput == Vector2.zero)
        {
            if (_inputCreep) ChangeState(State.CreepIdle);
            else if (_inputCrawl) ChangeState(State.CrawlIdle);
            else ChangeState(State.NormalIdle);
        }
        else
        {
            if (_inputSprint) ChangeState(State.SprintMove);
            else if (_inputCreep) ChangeState(State.CreepMove);
            else if (_inputCrawl) ChangeState(State.CrawlMove);
            else ChangeState(State.RunMove);
        }
    }

    void ChangeState(State nextState)
    {
        if (nextState == _currentState) return;

        if (_endOfStateAction != null)
        {
            _endOfStateAction.Invoke(nextState);

            _endOfStateAction = null;
        }

        switch (nextState)
        {
            case State.NormalIdle:

                break;

            case State.RunMove:
                break;
            case State.SprintMove:
                break;

            case State.CreepIdle:
                break;

            case State.CreepMove:
                break;

            case State.CrawlIdle:
                SetCrawlState();
                break;

            case State.CrawlMove:
                SetCrawlState();
                break;

            case State.Launched:
                break;

            case State.JumpStart:
                LaunchUp(_jumpHeight);
                _currentState = State.JumpIdle;

                break;
            case State.JumpIdle:

                break;

            case State.SecondJump:

                //Second Jump
                LaunchUp(_jumpHeight * _secondJumpFactor);

                //IMPORTANT: Overwrite LaunchUp's "_currentState = State.StartJump;" line
                _currentState = State.SecondJump;
                break;

            case State.AtLedge:
                break;

        }

        _currentState = nextState;
    }

    void SetCrawlState()
    {
        _crawlCollider.enabled = true;
        _standCollider.enabled = false;

        _endOfStateAction += (newState) =>
        {
            if (newState != State.CrawlMove && newState != State.CrawlIdle)
            {
                _crawlCollider.enabled = false;
                _standCollider.enabled = true;
            }

        };
    }

    public void MovePlayer(float speed, ForceMode mode)
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

                //_rigidbody.AddForce(totalVelocity, ForceMode.VelocityChange);

                targetVelocity = targetVelocity - _rigidbody.velocity;

                targetVelocity.y = 0;

                _rigidbody.AddForce(targetVelocity, mode);

                #endregion


            }


        }


    }

    bool CanGrabLedge()
    {
        if (_downRaycastPoint != null && _upRaycastPoint != null && _rigidbody.velocity.y < 0)
        {
            RaycastHit hit;

            for (int i = -1; i < 2; i++)
            {
                var pointForward = Quaternion.AngleAxis(_ledgeGrabAngle * 0.5f * i, _bodyObject.transform.up) * _bodyObject.transform.forward;

                //Debug.DrawLine(_downRaycastPoint.position, _downRaycastPoint.position + pointForward* 20, Color.red);
                if (Physics.Raycast(_downRaycastPoint.position, pointForward, out hit, _ledgeGrabRange, _groundMask))
                {
                    if (!Physics.Raycast(_upRaycastPoint.position, pointForward, _ledgeGrabRange, _groundMask))
                    {
                        Debug.Log("Grabbing");
                        //transform.position = hit.point;

                        return true;
                    }


                }

            }

        }



        return false;
    }

    /// <summary>
    /// Launch character on Up direction to specified height
    /// </summary>
    /// <param name="height"></param>
    public void LaunchUp(float height)
    {
        Vector3 jumpVelocity = Vector3.zero;

        float launchMagnitude = 2 * Mathf.Abs(Physics.gravity.y) * height;

        launchMagnitude = Mathf.Pow(launchMagnitude, 0.5f);

        jumpVelocity.y = launchMagnitude;

        Launch(jumpVelocity);

        ChangeState(State.Launched);
    }

    public void Push(Vector3 velocity)
    {
        Launch(velocity);

        ChangeState(State.Launched);
    }

    void Launch(Vector3 velocity)
    {
        var currentVerticalVelocity = _rigidbody.velocity;
        currentVerticalVelocity.x = 0;
        currentVerticalVelocity.z = 0;
        _rigidbody.AddForce(velocity - currentVerticalVelocity, ForceMode.VelocityChange);

        _animationCommand.Jump();
    }

    public override void SetInput(PlatformMap input)
    {

        #region Axis Movement
        input.Character.Movement.performed += ctx =>
        {
            Vector2 v = ctx.ReadValue<Vector2>();

            //Set Latest Input Direction. Movement is applied in FixedUpdate
            _moveInput = v;

        };


        input.Character.Movement.canceled += ctx =>
        {
            _moveInput = Vector2.zero;
        };

        #endregion

        input.Character.Jump.performed += ctx =>
        {
            _inputJump = true;
        };

        input.Character.Creep.performed += ctx =>
        {
            _inputCreep = true;
        };

        input.Character.Creep.canceled += ctx =>
        {
            _inputCreep = false;
        };

        input.Character.Crawl.performed += ctx =>
        {
            _inputCrawl = true;

        };

        input.Character.Crawl.canceled += ctx =>
        {
            _inputCrawl = false;

        };

        input.Character.Sprint.performed += ctx =>
        {
            _inputSprint = true;
        };

        input.Character.Sprint.canceled += ctx =>
        {
            _inputSprint = false;
        };

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        var cubePosition = transform.position;
        cubePosition += new Vector3(0, 0.5f * _jumpHeight, 0);

        Gizmos.color = _jumpHeightColor;
        Gizmos.DrawWireCube(cubePosition, new Vector3(_jumpGizmosWidth, _jumpHeight, _jumpGizmosWidth));

        Gizmos.color = _groundCheckColor;
        Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);

        Gizmos.color = _noiseColor;
        Gizmos.DrawWireSphere(transform.position, _sprintNoiseRange);
    }
#endif
}
