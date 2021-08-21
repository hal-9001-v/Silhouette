using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GeneralTools;

[RequireComponent(typeof(NavMeshAgent))]
public class Navigator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] NavMeshAgent _navMeshAgent;
    [SerializeField] PatrolRoute _patrolRoute;
    [SerializeField] [Range(0.01f, 1)] float _patrolDistance = 0.1f;
    [SerializeField] [Range(0.01f, 5)] float _pursueDistance = 0.1f;
    [SerializeField] [Range(0.01f, 5)] float _checkingPlaceDistance = 0.1f;
    [SerializeField] [Range(0.01f, 5)] float _jumpLineDistance = 0.1f;

    public Action patrolPointReachedAction;
    public Action pursueReachedAction;
    Action _targetPositionReachedAction;

    Rigidbody _rigidbody;

    public Vector3 velocity
    {
        get
        {
            return _navMeshAgent.velocity;
        }
    }

    Vector3 _linePoint;
    Vector3 _jumpPoint;

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

        GoToFallPoint,
        Fall,
    }

    // Start is called before the first frame update
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
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
            if (_currentState == NavState.Pursue)
            {
                _currentState = NavState.GoToJumpPoint;

                _linePoint = rooftop.GetClosestJumpPoint(transform.position);

            }
        }
    }

    private void FixedUpdate()
    {

        if (!_navMeshAgent.isOnNavMesh)
        {
            _navMeshAgent.Warp(transform.position);
            return;
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

                _navMeshAgent.SetDestination(_linePoint);

                if (HorizontalDistance(_navMeshAgent.transform.position, _linePoint) < _jumpLineDistance)
                {
                    ChangeState(NavState.Jump);
                }

                break;

            case NavState.Jump:
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

                _jumpPoint = _pursuable.currentRooftop.GetClosestFallPoint(transform.position);
                Vector3 jumpVelocity = Vector3.zero;
                jumpVelocity.y = PhysicsMath.LaunchSpeedForHeight(_jumpPoint.y - transform.position.y) * 1.25f;

                _rigidbody.AddForce(jumpVelocity, ForceMode.VelocityChange);

                break;

            case NavState.GoToJumpPoint:
                _navMeshAgent.enabled = true;

                break;

            case NavState.GoToFallPoint:
                _navMeshAgent.enabled = true;

                break;

            case NavState.Fall:
                _navMeshAgent.enabled = false;

                break;

        }



    }

}
