using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GeneralTools;

[RequireComponent(typeof(NavMeshAgent), typeof(GroundChecker))]
public class Navigator : MonoBehaviour
{
    const int JumpMeshLink = 2;
    
    [Header("References")]
    [SerializeField] NavMeshAgent _navMeshAgent;
    [SerializeField] PatrolRoute _patrolRoute;

    [Header("Settings")]
    [SerializeField] [Range(0.01f, 1)] float _patrolDistance = 0.1f;
    [SerializeField] [Range(0.01f, 5)] float _pursueDistance = 0.1f;
    [SerializeField] [Range(0.01f, 5)] float _checkingPlaceDistance = 0.1f;
    [SerializeField] [Range(0.01f, 5)] float _jumpLineDistance = 0.1f;
    [SerializeField] [Range(0.01f, 5)] float _ImpulseToLinePoint = 0.1f;

    public Action patrolPointReachedAction;
    public Action pursueReachedAction;
    Action _targetPositionReachedAction;

    Rigidbody _rigidbody;
    GroundChecker _groundChecker;

    public Vector3 velocity
    {
        get
        {
            return _navMeshAgent.velocity;
        }
    }

    float _desiredSpeed;

    Vector3 _groundPoint;
    Vector3 _roofPoint;

    int _currentPatrolPoint;

    Transform _target;
    Pursuable _pursuable;

    //Used for an specific position with no changes
    Vector3 _targetPosition;

    NavState _currentState;

    enum NavState
    {
        Idle,

        Pursue,
        Patrol,
        GoingToPosition,

        GoToJumpPoint,
        Jump,
        ImpulseToRoofPoint,

        GoToFallPoint,
        Fall,

        Land
    }

    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _groundChecker = GetComponent<GroundChecker>();

        _currentPatrolPoint = -1;
        SetNextPatrolPoint();


    }

    Transform SetNextPatrolPoint()
    {
        if (_patrolRoute != null && _patrolRoute.patrolPoints.Length != 0)
        {
            if (_currentPatrolPoint >= _patrolRoute.patrolPoints.Length - 1)
            {
                _currentPatrolPoint = 0;
            }
            else
            {
                _currentPatrolPoint++;
            }

            return _patrolRoute.patrolPoints[_currentPatrolPoint];
        }

        return null;
    }

    public void SetPatrolRoute(PatrolRoute route)
    {
        _patrolRoute = route;
        _currentPatrolPoint = 0;

        transform.position = route.patrolPoints[_currentPatrolPoint].position;
    }

    public void WarpNavMesh()
    {
        _navMeshAgent.Warp(transform.position);
    }

    public void Patrol(float speed)
    {
        if (_patrolRoute != null)
        {
            _desiredSpeed = speed;
            _navMeshAgent.speed = speed;

            ChangeState(NavState.Patrol);

            Continue();
        }
        else
        {
            ChangeState(NavState.Idle);
        }
    }

    public void Pursue(float speed, Transform target)
    {
        _desiredSpeed = speed;
        _navMeshAgent.speed = speed;

        _target = target;
        _pursuable = target.GetComponent<Pursuable>();

        ChangeState(NavState.Pursue);

        Continue();
    }

    public void GoToPosition(float speed, Vector3 position, Action arrivedAction)
    {
        _navMeshAgent.speed = speed;

        _targetPosition = position;

        _targetPositionReachedAction = arrivedAction;

        ChangeState(NavState.GoingToPosition);

        Continue();

    }

    public void Stop()
    {
        if (_navMeshAgent.isOnNavMesh)
            _navMeshAgent.isStopped = true;

        enabled = false;

        ChangeState(NavState.Idle);
    }

    public void Continue()
    {
        if (_navMeshAgent.isOnNavMesh)
            _navMeshAgent.isStopped = false;

        enabled = true;
    }

    float HorizontalDistance(Vector3 a, Vector3 b)
    {
        return Vector2.Distance(new Vector2(a.x, a.z), new Vector2(b.x, b.z));
    }

    public void JumpIntoRooftop(Rooftop rooftop)
    {
        if (_pursuable != null && _pursuable.currentRooftop == rooftop)
        {
            if (_currentState != NavState.GoToJumpPoint && _currentState != NavState.Jump)
            {
                ChangeState(NavState.GoToJumpPoint);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!_navMeshAgent.isOnNavMesh)
        {
            _navMeshAgent.Warp(transform.position);
        }

        switch (_currentState)
        {
            //PATROL STATE
            case NavState.Patrol:
                if (_patrolRoute == null) return;

                if (HorizontalDistance(_navMeshAgent.transform.position, _targetPosition) < _patrolDistance)
                {
                    _targetPosition = SetNextPatrolPoint().position;
                    _navMeshAgent.SetDestination(_targetPosition);

                    if (patrolPointReachedAction != null)
                    {
                        patrolPointReachedAction.Invoke();
                    }
                }
                break;

            //PURSUE STATE
            case NavState.Pursue:
                _navMeshAgent.SetDestination(_target.position);

                if (HorizontalDistance(_navMeshAgent.transform.position, _target.position) < _pursueDistance)
                {
                    if (patrolPointReachedAction != null)
                        pursueReachedAction.Invoke();
                }

                if (_navMeshAgent.isOnOffMeshLink)
                {
                    if (_navMeshAgent.currentOffMeshLinkData.offMeshLink.area == JumpMeshLink)
                    {
                        _navMeshAgent.speed = _desiredSpeed * 2;
                    }
                }
                else
                {
                    _navMeshAgent.speed = _desiredSpeed;
                }

                break;



            case NavState.GoingToPosition:

                _navMeshAgent.SetDestination(_targetPosition);

                if (HorizontalDistance(_navMeshAgent.transform.position, _targetPosition) < _checkingPlaceDistance)
                {
                    if (_targetPositionReachedAction != null)
                    {
                        _targetPositionReachedAction.Invoke();
                    }
                    ChangeState(NavState.Idle);
                }

                break;

            case NavState.GoToJumpPoint:

                _navMeshAgent.SetDestination(_groundPoint);

                var distance = HorizontalDistance(_navMeshAgent.transform.position, _groundPoint);
                if (HorizontalDistance(_navMeshAgent.transform.position, _groundPoint) < _jumpLineDistance)
                {
                    ChangeState(NavState.Jump);
                }

                break;

            case NavState.Jump:

                if (transform.position.y >= _roofPoint.y)
                {
                    ChangeState(NavState.ImpulseToRoofPoint);
                }

                break;

            case NavState.ImpulseToRoofPoint:

                if (_groundChecker.isGrounded)
                {
                    ChangeState(NavState.Land);
                }

                break;


            default:
                break;
        }


    }

    void ChangeState(NavState state)
    {
        if (state == _currentState) return;

        _currentState = state;

        switch (state)
        {
            case NavState.Idle:
                _navMeshAgent.velocity = Vector3.zero;
                _navMeshAgent.enabled = false;
                break;

            case NavState.Patrol:
                _navMeshAgent.enabled = true;

                _targetPosition = _patrolRoute.patrolPoints[_currentPatrolPoint].position;

                break;

            case NavState.Pursue:
                _navMeshAgent.enabled = true;

                Continue();
                break;

            case NavState.GoingToPosition:
                _navMeshAgent.enabled = true;

                break;

            case NavState.Jump:
                _navMeshAgent.enabled = false;
                //_navMeshAgent.isStopped = true;

                Vector3 jumpVelocity = Vector3.zero;
                jumpVelocity.y = PhysicsMath.LaunchSpeedForHeight(_roofPoint.y - transform.position.y) * 1.25f;

                _rigidbody.isKinematic = false;
                _rigidbody.useGravity = true;
                _rigidbody.AddForce(jumpVelocity, ForceMode.VelocityChange);

                break;

            case NavState.GoToJumpPoint:
                _navMeshAgent.enabled = true;

                _groundPoint = _pursuable.currentRooftop.GetClosestGroundPoint(transform.position, true);
                _roofPoint = _pursuable.currentRooftop.GetClosestRoofPoint(_groundPoint, true);

                break;

            case NavState.ImpulseToRoofPoint:

                Vector3 toPointVelocity = _roofPoint - transform.position;
                toPointVelocity.y = 0;

                toPointVelocity = toPointVelocity.normalized * 3;

                _rigidbody.AddForce(toPointVelocity, ForceMode.VelocityChange);

                break;

            case NavState.GoToFallPoint:
                _navMeshAgent.enabled = true;

                break;

            case NavState.Fall:
                _navMeshAgent.enabled = false;
                break;

            case NavState.Land:
                _navMeshAgent.enabled = true;
                _navMeshAgent.isStopped = false;


                _rigidbody.isKinematic = true;
                _rigidbody.useGravity = false;

                ChangeState(NavState.Pursue);
                break;

        }



    }

}
